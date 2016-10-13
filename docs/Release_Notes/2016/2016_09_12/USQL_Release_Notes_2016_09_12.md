# U-SQL Release Notes 2016-09-12
--------------------------
## Pending and Upcoming Deprecations

Please review your code and make sure you are cleaning your existing code to be ready for the following 
deprecations (follow links for more details).

**This is the last release where both old and new syntax will work. Starting with the next release, only the new syntax will be supported!**

Follow our [Azure Data Lake Blog](http://blogs.msdn.microsoft.com/azuredatalake) for more details and future annoucements of deprecation timelines.

#### [Start of Deprecation of old `PARTITIONED BY` Syntax.](https://github.com/Azure/AzureDataLake/blob/master/docs/Release_Notes/2016/2016_08_01/USQL_Release_Notes_2016_08_01.md#start-of-deprecation-of-old-partitioned-by-syntax)

 
In order to adjust our syntax with other SQL products and to more clearly differentiate the addressable partitions from the internal data distributions in a table, we are changing the `PARTITIONED BY` syntax in the following way:

#### [DateTime file set pattern will require `HH` instead of `hh` for the hour pattern to align with 24h clock semantics](https://github.com/Azure/AzureDataLake/blob/master/docs/Release_Notes/2016/2016_08_01/USQL_Release_Notes_2016_08_01.md#datetime-file-set-pattern-will-require-hh-instead-of-hh-for-the-hour-pattern-to-align-with-24h-clock-semantics)

Currently U-SQL supports both forms with 24h semantics. In the future, support for `hh` in file set patterns will be dropped.

#### [Deprecation of `{col:*}` File Set pattern](https://github.com/Azure/AzureDataLake/blob/master/docs/Release_Notes/2016/2016_07_14/USQL_Release_Notes_2016_07_14.md)

Instead of `{col:*}` use `{col}`.

## Breaking Changes

#### `ALTER TABLE ADD/DROP COLUMN`: The `COLUMN` keyword is mandatory now.

U-SQL is making the `COLUMN` keyword mandatory in the [`ALTER TABLE ADD/DROP COLUMN` statement](https://github.com/Azure/AzureDataLake/blob/master/docs/Release_Notes/2016/2016_07_14/USQL_Release_Notes_2016_07_14.md#u-sql-now-supports-adding-and-removing-columns-on-u-sql-tables) to allow introduction of more options in subsequent releases. Impact should be minimal since the published syntax was always making it mandatory.

## Major U-SQL Bug Fixes, Performance and Scale Improvements

#### Fixed order of REFERENCE and USING in binding
Starting in this release, U-SQL will require the following ordering:

1. Any expression referring to custom code has to be preceeded by the reference to the custom code's assembly. 
2. If that expression makes use of a short form, the relevant `USING` statement needs to  
    a. follow the `REFERENCE ASSEMBLY` statement that provides the namespace, and
	b. preceed the expression that depends on the `USING` statement. 

Example: 

    USING N1;
    REFERENCE ASSEMBLY Asm1;		

This will now fail with the error `"error CS0246: The type or namespace name 'N1' could not be found (are you missing a USING statement or an assembly reference?)" on "### USING"` 
if N1 is only defined in Asm1.

#### `CREATE FUNCTION` and `CREATE PROCEDURE` require fully qualify type names in table type definitions

Previously `TABLE` type declarations in function or procedure parameters or function return types allowed partially named user-defined types. This lead to issues when calling the function because the USING clause was not preserved.
Now you have to fully qualify the C# name. 

If a name is not fully qualified, such as in 

    CREATE FUNCTION F1(@c TABLE(a MyType, b int)) RETURNS @r AS ...

it now will fail with the following error:

    E_CSC_USER_NOTFULLYQUALIFIEDCOLUMNTYPENAME: USING is not available in this context. […] 'MyType' cannot be used as column type. The type or namespace name 'MyType' could not be found. […] CREATE FUNCTION F1(@c TABLE(a  ### MyType, b int))

assuming `MyType` is in namespace `MyNamespace` with a `USING MyNamespace` (or an alias for `MyType`) in the parent script (before the `CREATE FUNCTION`).


#### Input File Set scales orders of magnitudes better (requires opt-in)

Previously, U-SQL's file set pattern on `EXTRACT` expressions ran into compile time time-outs around 800 to 5000 files. 

Starting in this release, U-SQL's file set pattern scales to many more files and generates more efficient plans.

For example, a U-SQL script querying over 2500 files in our telemetry system previously took over 10 minutes to compile 
now compiles in 1 minute and the script now executes in 9 minutes instead of over 35 minutes using a lot less AUs.

We also have compiled scripts that access 30'000 files.

**Since we are still testing some of the limits of the feature, you are required to opt in. Please [contact us](mailto:usql@microsoft.com) if you want to explore this capability.**

#### Job manager performance got improved which helps short vertex executions

Previously, the job manager created new containers for each vertex that could add a considerable overhead to a vertex execution if the vertex was only doing seconds worth of work. 
Now the job manager is recycling containers which can reduce the overhead considerably on short vertex executions.

#### Increase `IN` elements count max

The U-SQL scalar `IN` expression now accepts up to 20000 values in the right-hand list instead of the previous 500 values.

		
## New U-SQL capabilities

#### New `DECLARE` statement options

U-SQL added the following two options that helps with script parameterization and early discovery of constant-folding expressions.

##### `DECLARE EXTERNAL`

`DECLARE EXTERNAL` allows the declaration of a script scalar expression variable that can be overwritten by a previous DECLARE statement without failing compilation.

For example, the following script:

    DECLARE EXTERNAL @value string = "external declaration";

    @r = SELECT * FROM (VALUES(@value)) AS T(x);
    OUTPUT @r TO "/output/test.csv" USING Outputters.Csv();


will produce the specified file with content `"external declaration"`.

The following script on the other hand will produce the file with the content `"overwritten declaration"`.

    DECLARE @value string = "overwritten declaration";
    DECLARE EXTERNAL @value string = "external declaration";

    @r = SELECT * FROM (VALUES(@value)) AS T(x);
    OUTPUT @r TO "/output/test.csv" USING Outputters.Csv();

This allows users to parameterize scripts with a default and allows tools (such as Azure Data Factory) to provide a parameter model and overwrite the default parameter values.

##### `DECLARE CONST`

U-SQL currently requires that in certain syntactic contexts expressions are evaluated at compile time. Examples are the `EXTRACT`'s `TO` clause or the `IF` statement's condition.
Often the parser will provide a compile time error if a non-constant-foldable expression is being used.

However, in some syntactic context, we do support non-constant-foldable expressions but do something more clever if an expression can be evaluated at compile time, such as during partition elimination.

The `DECLARE CONST` allows a U-SQL programmer to both assert and check if an expression is actually constant-foldable.

For example:

    DECLARE CONST @f string = String.Join("/", new String[]{"output", "test.csv"});

    @r = SELECT * FROM (VALUES(@f)) AS T(i);
    OUTPUT @r TO @f USING Outputters.Csv();

works since the `String.Join` expression is constant-folded in U-SQL, while

    DECLARE CONST @f string = ((Func<string,string>)( s => String.Join(s, new String[]{"output", "test.csv"})))("/");

    @r = SELECT * FROM (VALUES(@f)) AS T(i);
    OUTPUT @r TO @f USING Outputters.Csv();

fails with error

    E_CSC_USER_EXPRESSIONNOTCONSTANTFOLDABLE: Expression cannot be constant folded.
    Description:
    The expression cannot be evaluated at compile time.
    Resolution:
    Use a constant expression or a CONST parameter.

because the expression assigned to `@f` is now not a constant-foldable expression anymore (at the time of this writing).

#### New scalar FILE and PARTITION object functions are available to test for presence and other properties.

U-SQL added new intrinsic functions that allow to check for certain properties of the two intrinsic objects `FILE` and `PARTITION`. Note that these intrinsic functions are currently always evaluated at compile time (and thus considered constant-foldable).

They can be called as part of any scalar expression.

##### `FILE` object functions

The `FILE` intrinsic object supports the following functions:

    bool FILE.EXISTS(string path)

This function takes a constant-foldable string expression as its argument and checks if a file in the specified location exists at compile time. If the file exists and the user has access to it, it returns `true`. It returns `false`, if the file path does not exists, refers to a folder or the user has no access to it. 

The parameter value can be any supported file path URI. 

If the argument expression is not constant-foldable, the error `E_CSC_USER_EXPRESSIONNOTCONSTANTFOLDABLE` is raised.

If the path is empty (null or the zero-length string), the error `E_CSC_USER_EMPTYFILEPATH` is raised. If the path name contains invalid characters, the error `E_CSC_USER_INVALIDFILENAME` is raised.

    long? FILE.LENGTH(string path)

This function takes a constant-foldable string expression as its argument and returns the logical size in bytes of the file at the specified location at compile time. The function returns `null` if the file path does not exists, refers to a folder or the user has no access to it.

The parameter value can be any supported file path URI.

If the argument expression is not constant-foldable, the error `E_CSC_USER_EXPRESSIONNOTCONSTANTFOLDABLE` is raised.

If the path is empty (null or the zero-length string), the error `E_CSC_USER_EMPTYFILEPATH` is raised. If the path name contains invalid characters, the error `E_CSC_USER_INVALIDFILENAME` is raised.

    DateTime? FILE.CREATED(string path)

This function takes a constant-foldable string expression as its argument and returns the creation timestamp of the file at the specified location at compile time as a DateTime value (Kind is `Unspecified`). The function returns `null` if the file path does not exists, refers to a folder or the user has no access to it.

The parameter value can be any supported file path URI.

If the argument expression is not constant-foldable, the error `E_CSC_USER_EXPRESSIONNOTCONSTANTFOLDABLE` is raised.

If the path is empty (null or the zero-length string), the error `E_CSC_USER_EMPTYFILEPATH` is raised. If the path name contains invalid characters, the error `E_CSC_USER_INVALIDFILENAME` is raised.

    DateTime? FILE.MODIFIED(string path)

This function takes a constant-foldable string expression as its argument and returns the last modified timestamp of the file at the specified location at compile time as a DateTime value (Kind is `Unspecified`). The function returns `null` if the file path does not exists, refers to a folder or the user has no access to it.

The parameter value can be any supported file path URI.

If the argument expression is not constant-foldable, the error `E_CSC_USER_EXPRESSIONNOTCONSTANTFOLDABLE` is raised.

If the path is empty (null or the zero-length string), the error `E_CSC_USER_EMPTYFILEPATH` is raised. If the path name contains invalid characters, the error `E_CSC_USER_INVALIDFILENAME` is raised.

_Example:_

The following script shows how the `FILE` object intrinsic functions can be used in `DECLARE` statements, `IF` statements and `SELECT` expressions. The intrinsic will fail in the example in the `ELSE` clause if the file does not exist, because using a virtual column is not considered a constant-foldable expression. Note that if the file exists, the `ELSE` clause gets ignored during compilation.

    DECLARE @dir = "/Samples/Data/";
    DECLARE @filename = "SearchLog";
    DECLARE @extension = "tsv";
    DECLARE @filepath = @dir + @filename + "." + @extension;

    DECLARE CONST @f = FILE.EXISTS(@filepath) && FILE.LENGTH(@dir +@filename + '.' + @extension) < 16000 ? 'T' : 'F';

    @r1 =
        SELECT *
        FROM(VALUES(@f)) AS T(f);

    OUTPUT @r1
    TO "/output/docsamples/file-intrinsic1.csv"
    USING Outputters.Csv();

    IF (FILE.EXISTS(@filepath)) THEN

      @r2 =
          SELECT FILE.LENGTH(@filepath) AS length
               , FILE.CREATED(@filepath) AS created_datetime
               , FILE.MODIFIED(@filepath) AS modified_datetime
          FROM (VALUES (1)) AS T(dummy);

    ELSE // This will fail with Error E_CSC_USER_EXPRESSIONNOTCONSTANTFOLDABLE: Expression cannot be constant folded.

      @t =
         EXTRACT data string,
                 filename string
         FROM "/Samples/Data/{filename}.tsv"
         USING Extractors.Text(delimiter : '\b');

      @r2 = 
          SELECT FILE.LENGTH("/Samples/Data/" + filename + ".tsv") AS length
          FROM @t;

    END; // IF

    OUTPUT @r2
    TO "/output/docsamples/file-intrinsic2.csv"
    USING Outputters.Csv();

##### `PARTITION` object function

The `PARTITION` intrinsic object supports the following function:

    bool PARTITION.EXISTS(table_name, partition_value {, partition_value})

that returns `true` of a partition for the specified table exists and the user has access to it and `false` otherwise. It takes as argument the `table_name` as a U-SQL table [Identifier](https://msdn.microsoft.com/en-US/library/azure/mt490422.aspx) and the typed values that define the particular partition of the table. 

The function will be evaluated at compile-time (and is thus constant-foldable) and requires that the `partition_value` expressions are constant-foldable.

If the `partition_value` expressions are not constant-foldable, the error `E_CSC_USER_EXPRESSIONNOTCONSTANTFOLDABLE` is raised.

If the provided table does not exists or the user does not have access to it, the error `E_CSC_USER_DDLENTITYDOESNOTEXIST` is raised.

_Example:_

Let's assume we have created a partitioned table `PartTable` that is partitioned on the columns `PartId` and `market`:

    DROP TABLE IF EXISTS PartTable;

    CREATE TABLE PartTable
    (
        PartId int,
        market string,
        description string,
        price decimal,
        INDEX idx CLUSTERED(price)
        PARTITIONED BY (PartId, market)
        DISTRIBUTED BY RANGE(price)
    );

Then the following expression adds a partition for the table `PartTable` if it not already exists and then inserts two new rows into the specified partition.

    IF (!PARTITION.EXISTS(master.dbo.PartTable, 1, "en-us"))
    THEN
        ALTER TABLE PartTable ADD PARTITION (1, "en-us");
    END;

    INSERT INTO PartTable(description, price)
                PARTITION(1,"en-us")
    VALUES
       ("description 1", (decimal) 12.99),
       ("description 2", (decimal) 49.99);

	
#### U-SQL introduces set operations by name in addition to the existing positional processing

U-SQL adds a new `BY NAME` clause to the set expressions. The syntax is now:

    Set_Expression :=
	    Query_Expression Set_Operator [Set_Operator_Option] [By_Name]
	    Query_Expression.

    Set_Operator := 'INTERSECT' | 'UNION' | 'EXCEPT'.

    Set_Operator_Option :=
    	'DISTINCT'
    |	'ALL'.

    By_Name :=
    	'BY' 'NAME' ['ON' '(' (Identifier_List [',' '*'] | '*') ')'.

The newly introduced, optional `BY NAME` clause indicates that the set operation is matching up values not based on position but by name of the columns. 

If the `BY NAME` clause is not specified, the matching is done positionally.

If there is no `ON` clause, the counts of columns on the two sides must be the same and all columns on the left side must have matching columns with the same name on the right side. The schema of the result is the same as the schema of the left argument, i.e., the left argument determines the name and order of the output columns.

If there is an `ON` clause, it specifies the set of matching columns on both sides. There must be no duplicates in the list or an error is raised. All matching columns must be present in both rowset arguments or an error is raised. Moreover, the specified set of matching columns must be exactly the set of all columns in the two rowsets that have matching names. If there is an extra match that is not listed in the `ON` clause, an error is raised unless a `“*”` is specified in the list. The two rowsets may have other, non-matching columns, that are ignored by the set operator. The resulting schema is composed only of the matching columns in the order they are present in the left argument.

If the `ON` clause includes the `“*”` symbol (it may be specified as the last or the only member of the list), then extra name matches beyond those in the `ON` clause are allowed, and the result’s columns include all matching columns in the order they are present in the left argument.

_Example:_

Let’s assume that the following `@left` and `@right` rowset variables have been specified:

    @left = SELECT * FROM (VALUES(1, "Ursula" , (int?) null)
                        , (2, "Janine" , (int?) 19)
                        , (2, "Janine" , (int?) 19)
                        , (3, "Nils"   , (int?) 15)
                        , (3, "Nils"   , (int?) 15)) AS T(id, name, age);
    @right = SELECT * FROM (VALUES(1, "Ursula" , 15000)
                         , (2, "Janine" , 500)  
                         , (2, "Janine" , 2500)  
                         , (3, "Nils"   , 0)  
                         , (4, "Michael", 10000)) AS T(id, name, salary);

Then the following `INTERSECT`

    @i = SELECT id, name FROM @left INTERSECT BY NAME SELECT id, name FROM @right;

results in the following rowset `@i`

    | id   | name   |
    | ---- | ------ |
    | 1    | Ursula |
    | 2    | Janine |
    | 3    | Nils   |

The following `INTERSECT` expressions all result in the same result.

    @i = SELECT * FROM @left INTERSECT BY NAME ON (*) SELECT * FROM @right;
    @i = SELECT * FROM @left INTERSECT BY NAME ON (id, *) SELECT * FROM @right;
    @i = SELECT * FROM @left INTERSECT BY NAME ON (id, name) SELECT * FROM @right;

#### U-SQL introduces `OUTER UNION` to enable easier handling of semistructured data

U-SQL introduces a new `OUTER UNION` set expression that unions two rowsets that - unlike the normal `UNION` expression - have only partially overlapping schemas.

It's syntax is:

    Outer_Union_Expression :=
	    Query_Expression 'OUTER' 'UNION' ['ALL'] By_Name Query_Expression.

`OUTER UNION` requires the `BY NAME` clause and the `ON` list. As opposed to the other set expressions, the output schema of the `OUTER UNION` includes both the matching columns and the non-matching columns from both sides. This creates a situation where each row coming from one of the sides has "missing columns" that are present only on the other side. For such columns, default values are supplied for the "missing cells". The default values are `null` for nullable types and the .Net default value for the non-nullable types (e.g., `0` for `int`). 

As opposed to the other set expressions, `OUTER UNION` defaults to the `ALL` set option. The `DISTINCT` option is not supported.

_Example:_

The following script will union the two rowsets `@left` and `@right` with the partially overlapping schema on columns `A` and `K` while filling in `null` into the "missing cells" of column `C` and `0` as the default value for type `int` for column `B`.

    @left =
        SELECT *
        FROM (VALUES ( 1, "x", (int?) 50 ),
                     ( 1, "y", (int?) 60 )
             ) AS L(K, A, C);

    @right =
        SELECT *
        FROM (VALUES ( 5, "x", 1 ),
                     ( 6, "x", 2 ),
                     (10, "y", 3 )
             ) AS R(B, A, K);

    @res =
        SELECT * FROM @left
        OUTER UNION BY NAME ON (A, K)
        SELECT * FROM @right;

    OUTPUT @res TO "/output/docsamples/outerunion.csv" USING Outputters.Csv();

The result is:

    K | A   | C  | B  |
    - | --- | -- | -- |
    1 | "x" | 50 |  0 |
    1 | "x" |    |  5 |
    1 | "y" | 60 |  0 |
    2 | "x"	|    |  6 |
    3 | "y" |    | 10 |

The following alternative `OUTER UNION` expressions produce the same result:

    @res =
        SELECT * FROM @left
        OUTER UNION BY NAME ON (*)
        SELECT * FROM @right;
    @res =
        SELECT * FROM @left
        OUTER UNION BY NAME ON (K, *)
        SELECT * FROM @right;

#### New constant-foldable expressions in U-SQL

U-SQL now added the `Convert` class to the constant-foldable expressions. 
For example, `Convert.ToDateTime("2016-08-17")` is now constant-foldable.

#### U-SQL `USING` adds support of namespace aliasing. 

In a [previous refresh](https://github.com/Azure/AzureDataLake/blob/master/docs/Release_Notes/2016/2016_07_14/USQL_Release_Notes_2016_07_14.md#u-sql-now-supports-a-using-directive-that-allows-shortening-u-sql-custom-code-object-references), U-SQL added the `USING` clause that then only supported either 

    USING namespace.path;

or 

    USING alias = namespace.path.class;

Now it also supports 

    USING alias = namespace.path;	

#### Visual Studio Tool Provides Job Diff for Job Performance Debugging 
In some cases, similar jobs with similar U-SQL scripts can have very different performance. In the latest Azure Data Lake Tools for Visual Studio, you can use Job Diff to compare two jobs and find more insight for debugging. This is quite useful especially for recurring jobs. 

#### Visual Studio Tool Provides Throttling Warning Now
There are some ADLA account level networking bandwidth limitations, and your job may get throttled by these limitations.  In the latest Azure Data Lake Tools for Visual Studio, if 5% of your job execution time is impacted by throttling, "Throttling Time Check" issue will be found in the “Diagnostics” tab in "Job View". 

## PLEASE NOTE:

In order to get access to the new syntactic features and new tool capabilities on your local environment, you will need to [refresh your ADL Tools](http://aka.ms/adltoolsvs). Otherwise you will not be able to use them during local run and submission to the cluster will give you syntax warnings for the new language features.
