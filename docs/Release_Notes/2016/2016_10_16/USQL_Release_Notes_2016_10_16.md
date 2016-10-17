# U-SQL Release Notes 2016-10-16
--------------------------
## Pending and Upcoming Deprecations

Please review your code and make sure you are cleaning your existing code to be ready for the following 
deprecations.

**Please note: Previously announced deprecation items are now deprecated and raise errors instead of warnings!**

Follow our [Azure Data Lake Blog](http://blogs.msdn.microsoft.com/azuredatalake) for more details and future annoucements of deprecation timelines.

#### `CREATE/DROP CREDENTIAL` DDL is being deprecated 
 
U-SQL currently requires a login secret that is being generated through a PowerShell command and a `CREDENTIAL` object to create an external data source (`CREATE DATA SOURCE`). This is getting simplified into a single secret credential that needs to be generated with a PowerShell command, so the `CREATE/DROP CREDENTIAL` DDL is no longer needed and will be removed in an upcoming release.

Please note that you can still use already created data sources and their credentials even after the depreciation. Only scripts that explicitly use the DDL (and the corresponding PowerShell scripts) will have to be updated.

## Breaking Changes

#### Previously announced deprecation items are now removed

Attempting to use any of the following items will result in a deprecation error instead of a warning:

1. [Deprecation of old `PARTITIONED BY` Syntax.](https://github.com/Azure/AzureDataLake/blob/master/docs/Release_Notes/2016/2016_08_01/USQL_Release_Notes_2016_08_01.md#start-of-deprecation-of-old-partitioned-by-syntax)

 
2. [DateTime file set pattern will require `HH` instead of `hh` for the hour pattern to align with 24h clock semantics](https://github.com/Azure/AzureDataLake/blob/master/docs/Release_Notes/2016/2016_08_01/USQL_Release_Notes_2016_08_01.md#datetime-file-set-pattern-will-require-hh-instead-of-hh-for-the-hour-pattern-to-align-with-24h-clock-semantics)

3. [Deprecation of `{col:*}` File Set pattern. Use `{col}` instead.](https://github.com/Azure/AzureDataLake/blob/master/docs/Release_Notes/2016/2016_07_14/USQL_Release_Notes_2016_07_14.md)

#### Introduction of a 4GB limit on .gz files

U-SQL's extraction framework automatically decompresses `.gz` files. Since the GZip format extraction is not parallelizable, a single vertex is processing a GZipped file. We have repeatedly seen vertices run out of memory due to this and thus are now limiting the size of a single `.gz` file to 4GB. Please split your files to fit into this limit and use the file set capabilities to scale out your extraction.

## Major U-SQL Bug Fixes, Performance and Scale Improvements

#### U-SQL quoted identifiers 

Under certain circumstances, U-SQL quoted identifiers were not working correctly. For example, you could not select from a table that contained an identifier that had to be quoted. With this refresh we are fixing many of the quoted identifier issues. The following 2 known issues are still **not** addressed and will be addressed in future refreshs:

1. Inserting into a table that contains columns with identifiers that have to be quoted
   `INSERT INTO Table SELECT *;` doesn't work if the table has a column name that has to be quoted. Until this is fixed, please provide the explicit column list as in `INSERT INTO Table([Quoted Column Name], normalColName) SELECT ...`.			
2. Inserting into column names that contain characters that need to be entitized in XML
   `INSERT INTO Table ([Column With Ampersand &]) ...` does not work correctly. Until this is fixed, please avoid using characters in column names that need to be entitized in XML (mainly `&`, `<`).


#### Input File Set scales orders of magnitudes better (requires opt-in)

Previously, U-SQL's file set pattern on `EXTRACT` expressions ran into compile time time-outs around 800 to 5000 files. 

U-SQL's file set pattern now scales to many more files and generates more efficient plans.

For example, a U-SQL script querying over 2500 files in our telemetry system previously took over 10 minutes to compile 
now compiles in 1 minute and the script now executes in 9 minutes instead of over 35 minutes using a lot less AUs.

We also have compiled scripts that access 30'000 files.

**Since we are still testing some of the limits of the feature, you are required to opt in. Please [contact us](mailto:usql@microsoft.com) if you want to explore this capability.**

## New U-SQL capabilities

#### The U-SQL sampling capabilities are now available per default

For more information see the [earlier announcement](https://github.com/Azure/AzureDataLake/blob/master/docs/Release_Notes/2016/2016_08_01/USQL_Release_Notes_2016_08_01.md#u-sql-now-offers-a-preview-of-sampling-capabilities-requires-a-simple-email-signup).

#### Catalogs can be shared among ADLA accounts as long as they share their primary ADLS storage 

One important ability of a data lake is to be able to share data across different analytics accounts. Starting in this release, U-SQL let's you use 4 part-names to access U-SQL catalog objects from a different Azure Data Lake Analytics account. 

The following restrictions apply:

1. The two Azure Data Lake Analytics accounts need to share the same Azure Data Lake Storage account as their primary storage accounts.
2. As in the primary catalog, database and catalog level access control needs to permit referring to the objects.
3. You cannot create meta data objects in the other ADLA account's meta data object. Thus you cannot invoke any DDL statement with a 4-part name, nor invoking a U-SQL procedure.

The following operations are supported.

###### Selecting from a table, external table, external data source or view from another account

Let's assume that you created the view `master.dbo.SearchlogView` (see the [U-SQL Hands-On-Lab](https://github.com/Azure/AzureDataLake/blob/master/docs/Hands_on_Labs/USQL_HOL.md) for its definition) in the account `mryskona`. You can run the following script from the account `mrys` that has the same default storage account (you can also submit it from `mryskona` for that matter):

    @res = SELECT * FROM mryskona.master.dbo.SearchlogView;
    OUTPUT @res TO @"\output\remote-view.csv" USING Outputters.Csv();

Note that currently only the short account name is supported. If you refer to a non-existing account or an account you don't have access to, an error is raised.

###### Inserting into a table in another account

Let's assume that there is a table `master.dbo.Searchlog1` in the account `mryskona` (see the [U-SQL Hands-On-Lab](https://github.com/Azure/AzureDataLake/blob/master/docs/Hands_on_Labs/USQL_HOL.md) for its definition). You can run the following script from the account `mrys`:

    INSERT INTO mryskona.master.dbo.SearchLog1
    VALUES ( 9999, DateTime.Now, "fr-fr", "plonger", (int?) null, "", "");

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

Then I can check for the existence of today's partition when running from account `mrys` that shares the default storage account with `mryskona` as follows:

    DECLARE @today = new DateTime(2016, 10, 16, 00,00,00,00,DateTimeKind.Utc);

    IF PARTITION.EXISTS(mryskona.master.dbo.PartTable, @today)
    THEN
      @result = SELECT * FROM mryskona.master.dbo.PartTable WHERE event_date == @today;
    ELSE
      @result = SELECT String.Format("Partition for event date {0} not found", @today) AS message FROM (VALUES (1)) AS T(d);
    END;

    OUTPUT @result TO "/output/todays_partition.csv" USING Outputters. Csv();

Please note that you **cannot** add a new partition in another account.

#### Built-in Extractors and Outputters support numeric and literal serialization and parsing of char datatypes

The `char` datatype in .Net and U-SQL may not be a frequently used data type, but it has some interesting behavior, since it can be generated from an integral number as in

    char c = 42;

and from a Unicode character as in

    char c = '\xFFFD';

Previously, U-SQL only supported outputting and extracting `char` values to and from integral numbers. Starting with this refresh, we added a new string-typed parameter called `charFormat` to the built-in extractors and outputters. The parameter supports the following values:

  - `uint16` or _`null`_ (default) - serializes the `char` value as an integral number (taking all other serialization options into account) 
 and parses the input as the  integral character code number to the corresponding character or errors if the input is not an integral character code (or mapable _`null`_ if extracting it as `char?`).
  - `string` - serializes the `char` value in its Unicode string representation (taking all other serialization options including encoding into account) and parses the input as the character codepoint using the specified encoding.

If an unsupported value is being passed, the error `E_CSC_USER_UNSUPPORTEDCHARFORMAT` is raised.

If the value cannot be extracted with the above rules, e.g., because it is not a number for the charFormat `uint16`, or the byte sequence is not representing a UTF-16 code point, appropriate conversion errors are raised.

If the value cannot be output with the above rules, e.g., because the character is not representable in the specified encoding, appropriate conversion errors are raised.

_Example:_

The following expressions show the different ways of outputting `char` values and their interaction with some of the other options:

    @data = 
      SELECT 42 AS n_int
           , '\xFFFD' AS c_u2
           , '数' AS c_ch
           , ',' AS c_c
           , '4' AS c_int
           , '\n' AS c_nl
           , (char?) null AS c_null
      FROM (VALUES(1)) AS T(dummy);

    OUTPUT @data TO "/output/builtinUDO/char_as_int1.csv" 
    USING Outputters.Csv();

This generates the following result where the characters are output using their numerical value:

    42,65533,25968,44,52,10,

    OUTPUT @data TO "/output/builtinUDO/char_as_int2.csv" 
    USING Outputters.Csv(charFormat:"uint16");

This generates the same result as above.

    OUTPUT @data TO "/output/builtinUDO/char_as_string.csv" 
    USING Outputters.Csv(charFormat:"string");

This generates a result where the characters are output as UTF-8 encoded strings:

    42,"�","数",",","4","
    ",

Now let's see how the new parameter interacts with some of the other parameters.

    OUTPUT @data TO "/output/builtinUDO/char_as_string_utf16.csv" 
    USING Outputters.Csv(charFormat:"string", encoding:Encoding.Unicode);

generates the same result but UTF-16 encoded.

    OUTPUT @data TO "/output/builtinUDO/char_as_int_null.csv" 
    USING Outputters.Csv(nullEscape:"NULL",escapeCharacter:'#');

generates:

    42,65533,25968,44,52,10,NULL

Finally, 

    OUTPUT @data TO "/output/builtinUDO/char_as_string_null.csv" 
    USING Outputters.Csv(charFormat:"string", nullEscape:"NULL",escapeCharacter:'#');

generates

    42,"�","数",",","4","#n",NULL

#### The built-in Outputters provide option to output column names as header

U-SQL adds a new parameter called `outputHeader` to the built-in outputters `Outputters.Tsv`, `Outputters.Csv` and `Outputters.Text` that outputs the column names of the rowset to be output as the first header row.

_Example:_

The following scripts outputs the column names as header row.

    @data =
        SELECT "Jane" AS [first name], "Doe" AS [name,last], 42 AS [数量]
        FROM(VALUES(1)) AS T(dummy);

    OUTPUT @data
    TO "/output/docsamples/output_header.csv"
    USING Outputters.Csv(outputHeader : true);

The header row is taking the options of the outputter into account. Thus, the above statement will quote the header values while the next `OUTPUT` statement is turning quoting off:
 
    OUTPUT @data
    TO "/output/docsamples/output_header_noquotes.csv"
    USING Outputters.Csv(outputHeader : true, quoting:false);

#### Scalar variables in DECLARE now allow inferring the variable's type

When declaring a scalar variable in U-SQL, you needed to explicitly mention its type, as in:

    DECLARE @s string = "This is a string";

If the expression is not producing the stated type, an error is raised.

Starting in this release, U-SQL additionally gives you the option of letting U-SQL infer the variable's type:

    DECLARE @s = "This is a string";
    DECLARE @m = new SqlMap<string, string> {{"This", "is a string in a map"}, {"That", "is also a string in a map"}};

## PLEASE NOTE:

In order to get access to the new syntactic features and new tool capabilities on your local environment, you will need to [refresh your ADL Tools](http://aka.ms/adltoolsvs). Otherwise you will not be able to use them during local run and submission to the cluster will give you syntax warnings for the new language features.
