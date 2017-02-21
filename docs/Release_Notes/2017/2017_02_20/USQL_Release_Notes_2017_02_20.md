# U-SQL Release Notes 2017-02-20
--------------------------
## Pending and Upcoming Deprecations

Please review your code and make sure you are cleaning your existing code to be ready for the following 
deprecations.

**Please note: Previously announced deprecation items are now deprecated and raise errors instead of warnings!**

Follow our [Azure Data Lake Blog](http://blogs.msdn.microsoft.com/azuredatalake) for more details and future annoucements of deprecation timelines.

#### `CREATE/ALTER CREDENTIAL` DDL has been deprecated and now raises an error message
 
For more details please see [this announcement](https://blogs.msdn.microsoft.com/azuredatalake/2017/01/24/u-sql-deprecation-update-migration-of-data-source-credentials-and-removal-of-create-credential-alter-credential-and-drop-credential/).

Please note that you can still use already created data sources and their credentials, which have been automatically migrated, even after the depreciation. Only scripts that explicitly use the DDL (and the corresponding PowerShell scripts) will have to be updated.

#### `DROP CREDENTIAL` DDL will start to raise an error in the next refresh
 
In case the migration of a credential object may have run into an issue, we still give customers the ability to use the explicit `DROP CREDENTIAL` command for one more refresh cycle. With the next refresh, this syntax will be removed as well.

#### `PARTITION BY BUCKET` Syntax has been removed

As part of the [previous `PARTITION` syntax](https://github.com/Azure/AzureDataLake/blob/master/docs/Release_Notes/2016/2016_08_01/USQL_Release_Notes_2016_08_01.md#start-of-deprecation-of-old-partitioned-by-syntax), the deprecated `PARTITION BY BUCKET` Syntax has been removed in this refresh. More [details here](https://blogs.msdn.microsoft.com/azuredatalake/2017/01/23/u-sql-deprecation-notice-partition-by-bucket-will-be-removed/).

## Breaking Changes
#### Previously announced deprecation items are now removed

Attempting to use any of the following items will result in a deprecation error instead of a warning:

1. [`PARTITIONED BY BUCKET` Syntax.](https://blogs.msdn.microsoft.com/azuredatalake/2017/01/23/u-sql-deprecation-notice-partition-by-bucket-will-be-removed/)

2. [`CREATE/ALTER CREDENTIAL` DDL](https://blogs.msdn.microsoft.com/azuredatalake/2017/01/24/u-sql-deprecation-update-migration-of-data-source-credentials-and-removal-of-create-credential-alter-credential-and-drop-credential/)

#### `DISTINCT` Aggregates are no longer allowed in an `OVER` expression

DISTNCT aggregates are no longer allowed in an [`OVER` expression](https://msdn.microsoft.com/en-us/library/azure/mt490608.aspx) (e.g., `SELECT COUNT(DISTINCT x) OVER()`). 
In most cases this did not work previously anyway and resulted in an optimizer error during preparation phase. 
If you were using it and it did work, please [contact us](mailto:usql@microsoft.com) for a workaround.

## Major U-SQL Bug Fixes, Performance and Scale Improvements

Besides many internal improvements and fixes to reported bugs, we would like to call out the following major bug fixes and improvements to performance and scalability of the language.

#### Major performance improvement when running jobs near the ADLS read/write throttling limits

To provide fair access to the available ADLS read and write bandwidth, each account gets an upper limit on its available bandwidth. If a U-SQL job looks like it may exceed one of the bandwidth limits, the system will throttle the job execution (you can see if such throttling occurred in the Visualstudio's ADL Tool's Job Diagnostic tab). 

In this refresh, we release an improved throttling algorithm that improves the vertex execution times at production throttling limits by 100%. 

#### All Unicode whitespace characters are now recognized as whitespace by the U-SQL parser

In previous releases, U-SQL only recognized U+0020 (space), U+0009 (tab), U+000A (linefeed), U+000D (carriage return) as a valid whitespace characters and would reject scripts that contained other Unicode whitespace characters. With this refresh, U-SQL will accept all [Unicode whitespace characters](https://en.wikipedia.org/wiki/Whitespace_character).

#### Improved data-size dependent selection for default numbers of `HASH DISTRIBUTION` buckets

In previous releases, the default bucket number for `HASH` distribution buckets was 25, which often was either too big (for smaller tables) or too small (for very large tables).

Starting with this refresh, U-SQL will now choose the default number for hash distribution buckets dependent on the size of the first data to be inserted into the table in the following way:

U-SQL has a short list of candidate bucket numbers `({ 2, 10, 20, 60, 120, 240, 480 })`. It chooses the default from this list based on the estimated data size for the first insert and the upper bound average distribution size limit of 2GB. It chooses the first (smallest) number from the list which produces the average distribution size smaller or equal to the 2GB upper bound.

#### Support for `USE` statements inside U-SQL code object bodies coming from an external account

Previously, a script that called a U-SQL table-valued function or procedure from an external account would fail to compile if the function or procedure contained a `USE` statement. This issue should now be fixed.
	
#### Error reporting for expressions with unbalanced parentheses is greatly improved

Previously, error messages for unbalanced parentheses were often reported at the end of the script instead of the actual error location. The location indication should now be greatly improved.

#### U-SQL's assembly object aliasing (`USING` statement) now works again in `EXTRACT` expressions

This fixes an earlier regression in the support of the `USING` statement.

#### The C# `checked` expression is not a constant-foldable expression

In previous releases, we allowed `checked` in a constant-foldable expression and turned it into an unchecked expression. Starting in this release, if a `checked` expression appears in an expression, that expression is not constant-folded.

#### Two previously mentioned issues with quoted identifiers got fixed.

The following two issues mentioned in the [October 2016 release notes](https://github.com/Azure/AzureDataLake/blob/master/docs/Release_Notes/2016/2016_10_16/USQL_Release_Notes_2016_10_16.md#u-sql-quoted-identifiers) have been fixed since:

1. Inserting into a table that contains columns with identifiers that have to be quoted
   `INSERT INTO Table SELECT *;` does work if the table has a column name that has to be quoted. 
			
2. Inserting into column names that contain characters that need to be entitized in XML
   `INSERT INTO Table ([Column With Ampersand &]) ...` does work now.

## U-SQL Preview Features

We currently have the following U-SQL features in preview. A feature in preview means that we are still finalizing the implementation of the feature, but are soliciting feedback and want to make it available ahead of a full release due to their value to the scenarios they address and the ability to learn more from our customers.

**Since we are still testing these features, you are required to opt in. Please [contact us](mailto:usql@microsoft.com) if you want to explore any of these capabilities and the opt-in is not provided in the description below.**

#### Input File Set scales orders of magnitudes better (requires opt-in)

Previously, U-SQL's file set pattern on `EXTRACT` expressions ran into compile time time-outs around 800 to 5000 files. 

U-SQL's file set pattern now scales to many more files and generates more efficient plans.

For example, a U-SQL script querying over 2500 files in our telemetry system previously took over 10 minutes to compile 
now compiles in 1 minute and the script now executes in 9 minutes instead of over 35 minutes using a lot less AUs.

We also have compiled scripts that access 30'000 files.

#### A limited flexible-schema feature for U-SQL table-valued function parameters is now available for preview (requires opt-in)

This feature allows to write more generic U-SQL table-valued functions and procedures, where only part of the schema of a table parameter needs to be present.

## New U-SQL capabilities

#### Catalogs can be shared among ADLA accounts even across different primary ADLS accounts  

One important ability of a data lake is to be able to share data across different analytics accounts. [Earlier](https://github.com/Azure/AzureDataLake/blob/master/docs/Release_Notes/2016/2016_10_16/USQL_Release_Notes_2016_10_16.md#catalogs-can-be-shared-among-adla-accounts-as-long-as-they-share-their-primary-adls-storage), U-SQL started to let you use 4 part-names to access U-SQL catalog objects from a different Azure Data Lake Analytics account if they shared the same primary Azure Data Lake Store account. 

With this refresh, you are allowed to access U-SQL catalog objects also if the two ADLA accounts have different primary Azure Data Lake Store accounts.

The following conditions and restrictions apply:

1. As in the primary catalog, database and catalog level access control needs to permit referring to the objects.
2. The ADLS and ADLA accounts need to be in the same Azure region.
3. You cannot create meta data objects in the other ADLA account's meta data object. Thus you cannot invoke any DDL statement with a 4-part name, nor invoking a U-SQL procedure.
3. You cannot insert into a table of a different account.

The same operations as before are supported now with different primary ADLS accounts:

###### Selecting from a table, external table, external data source or view from another account

Let's assume that you created the view `master.dbo.SearchlogView` (see the [U-SQL Hands-On-Lab](https://github.com/Azure/AzureDataLake/blob/master/docs/Hands_on_Labs/USQL_HOL.md) for its definition) in the account `mryskona`. You can run the following script from the account `mrys`  (you can also submit it from `mryskona` for that matter):

    @res = SELECT * FROM mryskona.master.dbo.SearchlogView;
    OUTPUT @res TO @"\output\remote-view.csv" USING Outputters.Csv();

Note that currently only the short account name is supported. If you refer to a non-existing account or an account you don't have access to, an error is raised.

###### Invoking a table-valued function from another account

You can call the table-valued function `mryskona.master.dbo.RegionalSearchlog` (see the [U-SQL Hands-On-Lab](https://github.com/Azure/AzureDataLake/blob/master/docs/Hands_on_Labs/USQL_HOL.md) for its definition) from the account `mrys`:

    OUTPUT mryskona.master.dbo.RegionalSearchlog(DEFAULT) TO @"\output\regionalsearch.csv" USING Outputters.Csv();

###### Referencing an assembly from another account

Let's assume that you have [registered the `SqlSpatial`](https://blogs.msdn.microsoft.com/azuredatalake/2016/08/26/how-to-register-u-sql-assemblies-in-your-u-sql-catalog/) assembly in the `mryskona` account in the `SqlSpatial` database. You can reference it from another account such as `mrys` as follows:

    REFERENCE SYSTEM ASSEMBLY [System.Xml];
    REFERENCE ASSEMBLY mryskona.SQLSpatial.SqlSpatial;

    USING Geometry = Microsoft.SqlServer.Types.SqlGeometry;
    USING Geography = Microsoft.SqlServer.Types.SqlGeography;
    USING SqlChars = System.Data.SqlTypes.SqlChars;

    @spatial =
        SELECT * FROM (VALUES 
                       ( Geometry.Point(1.0,1.0,0).ToString()),    
                       ( Geometry.STGeomFromText(new SqlChars("LINESTRING (100 100, 20 180, 180 180)"), 0).ToString()) 
                      ) AS T(geom);

    OUTPUT @spatial
    TO "/output/spatial.csv"
    USING Outputters.Csv();

###### Checking for a partition in a table of another account

Let's assume we have created a partitioned table `PartTable` that is partitioned on the column `event_date` in schema `master.dbo` in account `mryskona`:

    DROP TABLE IF EXISTS PartTable;

    CREATE TABLE PartTable
    (
        PartId int,
        event_date DateTime,
        market string,
        description string,
        price decimal,
        INDEX idx CLUSTERED(market, PartId)
        PARTITIONED BY (event_date)
        DISTRIBUTED BY RANGE(market)
    );

Then I can check for the existence of today's partition when running from account `mrys` as follows:

    DECLARE @today = new DateTime(2016, 10, 16, 00,00,00,00,DateTimeKind.Utc);

    IF PARTITION.EXISTS(mryskona.master.dbo.PartTable, @today)
    THEN
      @result = SELECT * FROM mryskona.master.dbo.PartTable WHERE event_date == @today;
    ELSE
      @result = SELECT String.Format("Partition for event date {0} not found", @today) AS message FROM (VALUES (1)) AS T(d);
    END;

    OUTPUT @result TO "/output/todays_partition.csv" USING Outputters. Csv();

Please note that you **cannot** add a new partition in another account.

#### U-SQL added `PIVOT`/`UNPIVOT` support

U-SQL added the `PIVOT` and `UNPIVOT` rowset expressions. Both are similar to their T-SQL equivalents, but `PIVOT` additionally requires that the columns names are specified with column aliases in the `IN` clause.

The U-SQL Syntax looks like:

    Rowset_Expression :=
           '(' Query_Expression ')'
    |      Function_Call
    |      External_Rowset_Expression
    |      Pivot_Expression
    |      Unpivot_Expression. 

    Pivot_Expression := 
           Aliased_Rowset 'PIVOT' '(' 
             Aggregate_Function_Call 'FOR' Column_Identifier 'IN' '(' 
               expression Column_Alias {','  expression Column_Alias}
             ')'
           ')'.

    Unpivot_Expression :=
           Aliased_Rowset 'UNPIVOT' '('
             Column_Identifier 'FOR' Column_Identifier 'IN' '(' 
                Column_Identifier {',' Column_Identifier }
             ')'
           ')'.

##### U-SQL `PIVOT` Example

    @data = SELECT * FROM (VALUES
       ("Beatles", "George Harrison", 1943, "acoustic guitar", 1),
       ("Beatles", "John Lennon",     1940, "acoustic guitar", 2),
       ("Beatles", "Paul McCartney",  1942, "bass guitar", 5),
       ("Beatles", "Ringo Starr",     1940, "drums", 3),   
       ("Rolling Stones", "Charlie Watts",	1941, "drums", 1), 
       ("Rolling Stones", "Keith Richards", 1943, "acoustic guitar", 5), 
       ("Rolling Stones", "Mick Jagger",    1943, "vocals", 7), 
       ("Rolling Stones", "Ronnie Wood",    1947, "bass guitar", 6),      
       ("Creedence Clearwater Revival", "Doug Clifford", 1945, "drums", 0),
       ("Creedence Clearwater Revival", "John Fogerty",  1945, "lead guitar", 4),  
       ("Creedence Clearwater Revival", "Stu Cook",      1945, "bass guitar", 0),
       ("Creedence Clearwater Revival", "Tom Fogerty",   1941, "rhythm guitar", 2),
       ("Eagles", "Don Henley",     1947, "drums", 4),
       ("Eagles", "Glenn Frey",     1948, "acoustic guitar", 3), 
       ("Eagles", "Joe Walsh",      1947, "lead guitar", 4), 
       ("Eagles", "Timothy Schmit", 1947, "bass guitar", 2), 
       ("Pink Floyd", "David Gilmour",  1946, "lead guitar", 8),
       ("Pink Floyd", "Nick Mason",     1944, "drums", 4),    
       ("Pink Floyd", "Richard Wright", 1943, "keyboards", 3),
       ("Pink Floyd", "Roger Watters",  1943, "bass guitar", 3),
       ("Pink Floyd", "Syd Barrett",    1946, "rhythm guitar", 0)     
    ) AS RockBands(Band, Musician, YearOfBirth, Instrument, Children);
    
    @result1 =
        SELECT * 
        FROM @data
          PIVOT (ANY_VALUE(Children) FOR YearOfBirth IN (
               1940 AS y1940,
               1941 AS y1941,
               1942 AS y1942,
               1943 AS y1943,
               1944 AS y1944,
               1945 AS y1945,
               1946 AS y1946,
               1947 AS y1947,
               1948 AS y1948))
          AS PivotedTable;
    
    @result2 =
        SELECT * FROM @data
          PIVOT (ANY_VALUE(Instrument) FOR Band IN (
               "Beatles" AS Beatles,
               "Rolling Stones" AS [Rolling Stones],
               "Creedence Clearwater Revival" AS [CCR],
               "Eagles" AS [Eagles],
               "Pink Floyd" AS [Pink Floyd]))
          AS PivotedTable1
          PIVOT (ANY_VALUE(Children) FOR YearOfBirth IN (
               1940 AS y0,
               1941 AS y1,
               1942 AS y2,
               1943 AS y3,
               1944 AS y4,
               1945 AS y5,
               1946 AS y6,
               1947 AS y7,
               1948 AS y8))
          AS PivotedTable2;

    OUTPUT @result1 TO @"/output/documentation/pivot/Pivoted1.tsv"
    USING Outputters.Tsv(outputHeader:true, nullEscape:"#null#");

    OUTPUT @result2 TO @"/output/documentation/pivot/Pivoted2.tsv"
    USING Outputters.Tsv(outputHeader:true, nullEscape:"#null#");

##### U-SQL `UNPIVOT` Example

    @data =
      EXTRACT
        Musician string,
        [Beatles] string,
        [Rolling Stones] string,
        [Creedence Clearwater Revival] string,
        [Eagles] string,
        [Pink Floyd] string,
        [1940] int?,
        [1941] int?,
        [1942] int?,
        [1943] int?,
        [1944] int?,
        [1945] int?,
        [1946] int?,
        [1947] int?,     
        [1948] int?
      FROM  @"/output/documentation/pivot/Pivoted2.tsv"
      USING Extractors.Tsv(skipFirstNRows:1, nullEscape:"#null#");

    @result = 
      SELECT * 
      FROM @data
        UNPIVOT (Instrument FOR Band IN (
           [Beatles],
           [Rolling Stones],
           [Creedence Clearwater Revival],
           [Eagles],
           [Pink Floyd]
        )) AS UnpivotedTable1
        UNPIVOT (Children FOR YearOfBirth IN (
           [1940],
           [1941],
           [1942],
           [1943],
           [1944],
           [1945],
           [1946],
           [1947],
           [1948]  
        )) AS UnpivotedTable2;

    OUTPUT @result TO @"/output/documentation/unpivot/Unpivoted.tsv"
    ORDER BY Band, Musician ASC
    USING Outputters.Tsv(outputHeader:true, nullEscape:"#null#");

#### U-SQL's `VALUES` row/rowset constructor supports 1 million constant values

U-SQL's row/rowset constructor `VALUES` increased its limit to support up to 1 million constant values. The limit is based on the total count based on the `number of rows*number of columns`.

#### U-SQL's `CROSS`/`OUTER APPLY` adds support for `VALUES` expression and C# expressions of types `IEnumerable<T>`, `KeyValuePair<K,V>`, `IEnumerable<Tuple>` 

Previously, U-SQL's `CROSS`/`OUTER APPLY` required that the right-hand side expression was either an applier or an `EXPLODE` expression on a `SqlArray<T>` or `SqlMap<K,V>` instance.

Starting with this refresh, U-SQL adds support for a large number of new right-hand side expressions.

The new syntax for the `Apply_Expression` is:

    Apply_Expression :=                                                                                      
        Rowset_Source Apply_Operator Explode_Expression  
    |   Rowset_Source Apply_Operator Applier_Expression
    |   Rowset_Source Apply_Operator Table_Value_Constructor_Expression Derived_Table_Alias.

    Explode_Expression :=                                                                                    
       'EXPLODE' '(' (sqlmap_expression | sqlarray_expression | expression ) ')'  
        Derived_Table_Alias_With_Opt_Types.  

    Applier_Expression :=                                                                                    
        ['USING'] applier_type_expression Derived_Table_Alias_With_Types
        [Readonly_Clause] [Required_Clause].

Where `expression` is a C# expression returning a value of either type `IEnumerable<T>`, `IEnumerable<KeyValuePair<K,V>>`,  or `IEnumerable<Tuple>`. The expression normally refers to at least one of the columns from the `Rowset_Source`.

Also note that we added the optional keyword `USING` to the applier expression, both to align it with the syntax for other UDOs and to give parity with the `EXPLODE` and `VALUES` clause. We recommend that you use that keyword, since in the future we may start to make it mandatory.

##### U-SQL's `CROSS`/`OUTER APPLY` with `VALUES`

The `VALUES` clause allows to apply each row of the constructed rowset to be correlated to each row in the rowset source.

Example:

    @bands = 
      SELECT * 
      FROM (VALUES ("Beatles", "George Harrison, John Lennon, Paul McCartney, Ringo Starr"), 
                   ("Creedence Clearwater Revival", "Doug Clifford, John Fogerty, Stu Cook, Tom Fogerty"), 
                   ("Eagles", "Don Henley, Glenn Frey, Joe Walsh, Timothy Schmit"), 
                   ("Pink Floyd", "David Gilmour, Nick Mason, Richard Wright, Roger Watters, Syd Barrett"), 
                   ("Rolling Stones", "Charlie Watts, Keith Richards, Mick Jagger, Ronnie Wood")) AS Bands(name, members);
    
    @ca_val1 = SELECT * FROM @bands CROSS APPLY VALUES (1) AS T(x);
    @ca_val2 = SELECT * FROM @bands CROSS APPLY VALUES (1),(name.Length) AS T(x);
    @ca_val3 = SELECT * FROM @bands CROSS APPLY VALUES (1,name.Length,3) AS T(x,y,z);

    OUTPUT @ca_val1 TO "/output/documentation/crossapply_value1.csv" USING Outputters.Csv();
    OUTPUT @ca_val2 TO "/output/documentation/crossapply_value2.csv" USING Outputters.Csv();
    OUTPUT @ca_val3 TO "/output/documentation/crossapply_value3.csv" USING Outputters.Csv();

##### U-SQL's `CROSS`/`OUTER APPLY` `EXPLODE` with C# expressions of type `IEnumerable<T>`

As a generalization of the `EXPLODE` with SqlArray typed expressions, we now also support expressions that return any array or `IEnumerable<T>`, where `T` is a supported U-SQL type (U-SQL built-in type or U-SQL UDT).

Example:

    @bands = 
      SELECT * 
      FROM (VALUES ("Beatles", "George Harrison, John Lennon, Paul McCartney, Ringo Starr"), 
                   ("Creedence Clearwater Revival", "Doug Clifford, John Fogerty, Stu Cook, Tom Fogerty"), 
                   ("Eagles", "Don Henley, Glenn Frey, Joe Walsh, Timothy Schmit"), 
                   ("Pink Floyd", "David Gilmour, Nick Mason, Richard Wright, Roger Watters, Syd Barrett"), 
                   ("Rolling Stones", "Charlie Watts, Keith Richards, Mick Jagger, Ronnie Wood")) AS Bands(name, members);

    @ca_ie_T1 = 
      SELECT name, member 
      FROM @bands CROSS APPLY EXPLODE (members.Split(',')) AS T(member);
    @ca_ie_T2 = 
      SELECT name, member 
      FROM @bands CROSS APPLY EXPLODE (from m in members.Split(',') select m) AS T(member);

    OUTPUT @ca_ie_T1 TO "/output/documentation/crossapply_IE_T1.csv" USING Outputters.Csv();
    OUTPUT @ca_ie_T2 TO "/output/documentation/crossapply_IE_T2.csv" USING Outputters.Csv();

##### U-SQL's `CROSS`/`OUTER APPLY` with C# expressions of type `IEnumerable<KeyValuePair<K,V>>`

As a generalization of the `EXPLODE` with SqlMap typed expressions, we now also support expressions that return any `IEnumerable<KeyValuePair<K,V>>`, where `K` and `V` are supported U-SQL types (U-SQL built-in type or U-SQL UDT).

Examples:

    @bands = 
      SELECT * 
      FROM (VALUES ("Beatles", "George Harrison, John Lennon, Paul McCartney, Ringo Starr"), 
                   ("Creedence Clearwater Revival", "Doug Clifford, John Fogerty, Stu Cook, Tom Fogerty"), 
                   ("Eagles", "Don Henley, Glenn Frey, Joe Walsh, Timothy Schmit"), 
                   ("Pink Floyd", "David Gilmour, Nick Mason, Richard Wright, Roger Watters, Syd Barrett"), 
                   ("Rolling Stones", "Charlie Watts, Keith Richards, Mick Jagger, Ronnie Wood")) AS Bands(name, members);

    @ca_ie_kvp = 
      SELECT T.* 
      FROM @bands CROSS APPLY EXPLODE (from m in members.Split(',') select new KeyValuePair<string,string>(name,m)) AS T(k,v);

    OUTPUT @ca_ie_kvp TO "/output/documentation/crossapply_ie_kvp.csv" USING Outputters.Csv();

##### U-SQL's `CROSS`/`OUTER APPLY` with C# expressions of type `IEnumerable<Tuple>`

As a further generalization, we now also support `EXPLODE` expressions that return any `IEnumerable<Tuple<T1, T2, ...>`, where `T1`, `T2` etc. are supported U-SQL types (U-SQL built-in type or U-SQL UDT).

Examples:

    @bands = 
      SELECT * 
      FROM (VALUES ("Beatles", "George Harrison, John Lennon, Paul McCartney, Ringo Starr"), 
                   ("Creedence Clearwater Revival", "Doug Clifford, John Fogerty, Stu Cook, Tom Fogerty"), 
                   ("Eagles", "Don Henley, Glenn Frey, Joe Walsh, Timothy Schmit"), 
                   ("Pink Floyd", "David Gilmour, Nick Mason, Richard Wright, Roger Watters, Syd Barrett"), 
                   ("Rolling Stones", "Charlie Watts, Keith Richards, Mick Jagger, Ronnie Wood")) AS Bands(name, members);

    @ca_tuple = 
      SELECT name, m1, m2, m3, m4 
      FROM @bands CROSS APPLY EXPLODE (new [] {Tuple.Create(members.Split(',')[0],
                                                            members.Split(',')[1],
                                                            members.Split(',')[2],
                                                            members.Split(',')[3] )}) AS T(m1,m2,m3,m4);

    OUTPUT @ca_tuple TO "/output/documentation/crossapply_tuple.csv" USING Outputters.Csv();

#### U-SQL added file modification check in the compiler for catalog managed files

U-SQL may have failed a job at runtime if a file has been changed in the account's `/catalog` directory without going through U-SQL statements. In this release, we have added compile-time checks for the file integrity. If a file has been tampered with (e.g., overwritten or removed at the file system level), the following error is raised:

TBD

#### U-SQL Table types from a different database may now be referenced using 3-part naming

Just like any other database schema-level objects, table types can now be referenced using 3-part naming in function or procedure signatures.

#### U-SQL user-defined types (UDTs) can now be used in U-SQL variables 

Starting in this release, any [U-SQL UDT with the UDT annotations](https://docs.microsoft.com/en-us/azure/data-lake-analytics/data-lake-analytics-u-sql-programmability-guide) can know be used in as the inferred or explicitly specified data type of a U-SQL variable DECLARE statement.

For example, let's assume that the U-SQL assembly `MyAssembly` contains a UDT `MyNamespace.MyUDT`. Then the following two statements are now supported:

    REFERENCE ASSEMBLY MyAssembly;
    DECLARE @myvar1 MyNamespace.MyUDT = new MyNamespace.MyUDT(/*insert initialization values*/);  
    DECLARE @myvar2 = new MyNamespace.MyUDT(/*insert initialization values*/);  

## Azure Data Lake Tools for Visual Studio New Capabilities

#### Data View now is available in Data Lake Tools for Visual Studio. 

Using this feature, you can find easily the input and output tables/files for a particular job. Switch to the Data tab in the Job View window to enjoy this feature.
	
![Data View](https://github.com/Azure/AzureDataLake/blob/master/docs/img/ReleaseNotes/DataViewTab.png) 

#### Improved failed vertex debug experience for code behind .cs file. 

If a job with a code behind .cs file failed, you can now click the Download button on the top of the Job View to download both the failed vertex environment and the user-defined code behind to your local machine for debugging and troubleshooting. You can find more info [here](http://go.microsoft.com/fwlink/?LinkId=820718). 
	
#### The Azure Data Lake U-SQL SDK is now available at [Nuget.org](http://nuget.org)
	
You can install the U-SQL SDK with the following nuget command:

    nuget install Microsoft.Azure.DataLake.USQL.SDK
	
After installing the SDK, you can now compile and run your U-SQL script locally either from the command line or via programming interfaces. 

#### Expiration information for files now is exposed in Store Explorer

#### Better representation of Metadata Operations in Job View. 

The different metadata operations are now shown in a cleaner way.

#### Old versions for the Azure Data Lake Tools for Visual Studio are now archived.

They can be found at [https://github.com/Azure/AzureDataLake/releases](https://github.com/Azure/AzureDataLake/releases).


## PLEASE NOTE:

In order to get access to the new syntactic features and new tool capabilities on your local environment, you will need to [refresh your ADL Tools](http://aka.ms/adltoolsvs). Otherwise you will not be able to use them during local run and submission to the cluster will give you syntax warnings for the new language features.
