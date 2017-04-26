# U-SQL Release Notes 2017-04-24
--------------------------
## Pending and Upcoming Deprecations

Please review your code and make sure you are cleaning your existing code to be ready for the following 
deprecations of U-SQL preview features.

**Please note: Previously announced deprecation items are now deprecated and raise errors instead of warnings!**

Follow our [Azure Data Lake Blog](http://blogs.msdn.microsoft.com/azuredatalake) for more details and future annoucements of deprecation timelines.

#### `DROP CREDENTIAL` DDL will start to raise an error in an upcoming refresh
 
In case the migration of a credential object may have run into an issue, we still give customers the ability to use the explicit `DROP CREDENTIAL` command. In an upcoming refresh this syntax will be removed as well.

#### U-SQL jobs will introduce an upper limit for the number of table-backing files being read

U-SQL tables are backed by files. Each table partition is mapped to its own file, and each `INSERT` statement adds an additional file (unless a table is rebuild with `ALTER TABLE REBUILD`). 

If the file count of a table (or set of tables) grows beyond a certain limit and the query predicate cannot eliminate files (e.g., due to too many insertions), there is a large likely-hood that the compilation times out after 25 minutes. 

In previous releases, there was no limit on the number of table files read by a single job. In the current release we raise the following warning, if the number of table-backing files exceeds the limit of 3000 files per job:

>Warning: WrnExceededMaxTableFileReadThreshold

>Message: Script exceeds the maximum number of table or partition files allowed to be read. This message will be upgraded to an error message in next deployment. 

In the next release, we will turn this warning into an error per default. If your current job compiles but receives the warning today, we will provide a way to keep the warning in the next release.

Please note: This limit only applies to reading from table-backing files. Normal files don't have an explicit limit and have a much higher limit in practice since they use a different execution plan. The limit also only applies to files that are actually read and ignores the files in table partitions which are not used by the query.

In a future release, we will work on addressing this limit (no timeline yet). If you feel this is important to your scenario, please add your vote to the [feature request](https://feedback.azure.com/forums/327234-data-lake/suggestions/19050232-increase-number-of-u-sql-table-files-partitions-th).

#### Table-valued functions will disallow result variable names to conflict with parameter names

In a future refresh, table-valued functions will disallow a result variable from having the same name as any of the function's parameters. There is currently no warning in this refresh. There will be a warning in the next refresh and an error in the refresh after that.

## Breaking Changes
#### Previously announced deprecation items are now removed. Please check previous release notes for details

This refresh has no known breaking changes.

## Major U-SQL Bug Fixes, Performance and Scale Improvements

Besides many internal improvements and fixes to reported bugs, we would like to call out the following major bug fixes and improvements to performance and scalability of the language.

#### Improved data-size dependent selection for default numbers of `HASH DISTRIBUTION` buckets

In previous releases, the default bucket number for `HASH` distribution buckets was 25, which often was either too big (for smaller tables) or too small (for very large tables).

Starting with this refresh, U-SQL will now choose the default number for hash distribution buckets dependent on the size of the first data to be inserted into the table in the following way:

U-SQL has a short list of candidate bucket numbers `({ 2, 10, 20, 60, 120, 240, 480 })`. It chooses the default from this list based on the estimated data size for the first insert and the upper bound average distribution size limit of 2GB. It chooses the first (smallest) number from the list which produces the average distribution size smaller or equal to the 2GB upper bound.

(Note we announced this for the last refresh already, but it only got released in this one.)

#### `CREATE STATISTICS` Performance Improvements

When you create multiple statistics for a table, you will observe noticeable performance improvement during the statistics creation (4-5x in our internal benchmark). 

#### U-SQL increases the supported number of UNION and UNION ALL operators used in a single query expression

A much higher limit is now supported for a number of UNION or UNION ALL operators used in a continuous chain. While there is no specific numerical limit, the system can now handle thousands of chained UNION/UNION ALL operators before running out of resources. Previously, an internal limit was hit at about 200.

#### The R U-SQL Extension now provides the ability to return a data frame instead of a string

The R Extensions for U-SQL enable developers to perform massively parallel execution of R code for end to end data science scenarios covering: merging various data files, feature engineering (FE), partitioned data model building, and post deployment, massively parallel FE and scoring. You can install the extensions from the Azure ADLA Portal from the Sample Script blade. 

With the current release we added support to return a dataframe back into the U-SQL rowset from the R code instead of returning a string as before. More information can be found [here](https://microsoft.sharepoint.com/teams/adlcustomertraining/Shared%20Documents/Forms/AllItems.aspx?id=%2Fteams%2Fadlcustomertraining%2FShared%20Documents%2FMicrosoftVirtualAcademy%2FADL_MVA_VNEXT%2F06_USQL_Advanced_Programming%2FGet%20started%20with%20extending%20U-SQL%20with%20R%2Edocx&parent=%2Fteams%2Fadlcustomertraining%2FShared%20Documents%2FMicrosoftVirtualAcademy%2FADL_MVA_VNEXT%2F06_USQL_Advanced_Programming).

**NOTE: If you already have installed an earlier version of the extensions, you will have to reinstall the newest version from the Azure Portal.**

#### Improved error reporting in the U-SQL Python Extension

The Python Extensions for U-SQL enable developers to perform massively parallel execution of Python code from within U-SQL. You can install the extensions from the Azure ADLA Portal from the Sample Script blade. 

In previous versions, Python code error were returned as a generic error of the following form:

    Unhandled exception from user code: "Could not find file “
 
    ==== Caught exception System.IO.FileNotFoundException

Starting with this release, we provide the more detailed error information provided by the Python processor, such as the following error for a syntax error

    ScriptExecutionTrace: Error ScriptExecutionState: Running Message:     del df['aut
    ScriptExecutionTrace: Error ScriptExecutionState: Running Message:               ^
    ScriptExecutionTrace: Error ScriptExecutionState: Running Message: SyntaxError: EOL while scanning string literal

**NOTE: If you already have installed an earlier version of the extensions, you will have to reinstall the newest version from the Azure Portal.**


## U-SQL Preview Features

We currently have the following U-SQL features in preview. A feature in preview means that we are still finalizing the implementation of the feature, but are soliciting feedback and want to make it available ahead of a full release due to their value to the scenarios they address and the ability to learn more from our customers.

**Since we are still testing these features, you are required to opt in. Please [contact us](mailto:usql@microsoft.com) if you want to explore any of these capabilities and the opt-in is not provided in the description below.**

#### Input File Set scales orders of magnitudes better (opt-in statement is now provided)

Previously, U-SQL's file set pattern on `EXTRACT` expressions ran into compile time time-outs around 800 to 5000 files. 

U-SQL's file set pattern now scales to many more files and generates more efficient plans.

For example, a U-SQL script querying over 2500 files in our telemetry system previously took over 10 minutes to compile 
now compiles in 1 minute and the script now executes in 9 minutes instead of over 35 minutes using a lot less AUs.

We also have compiled scripts that access 30'000 files.

The preview feature can be turned on by adding the following statement to your script:

	SET @@FeaturePreviews = "FileSetV2Dot5:on";

#### A limited flexible-schema feature for U-SQL table-valued function parameters is now available for preview (requires opt-in)

This feature allows to write more generic U-SQL table-valued functions and procedures, where only part of the schema of a table parameter needs to be present.

## New U-SQL capabilities

#### U-SQL adds the notion of Packages

A U-SQL package allows to bundle commonly used constant declarations, assembly references and resource deployments in a single meta data object to make sharing and referencing easier. 

U-SQL packages can be nested, and can encapsulate references to use or export to the caller/importer of the package. 

Packages can  be parameterized (e.g. for context or versioning). 

The identifiers referenced inside a package definition will be resolved in the static context of the package definition, similarly to table-valued functions and procedures.

If conflicting assemblies or references are being referenced, imported or deployed, errors are raised once a package is getting imported in the main U-SQL script.

Since they are a meta data object they are subject to the database-level access control.

Note: no DDL is allowed inside a package (e.g. CREATE/ALTER/DROP .. etc).

U-SQL supports the following DDL statements for creating, deleting and using packages.

###### CREATE PACKAGE (U-SQL)

The `CREATE PACKAGE` statement creates a package to allow bundling of commonly used together U-SQL assemblies, variables and resources. 

A package declaration can consist of the using statement, reference statement and declare statements to set its own internal static name context, import from other packages, declare what gets exported by the package, and what resources the package will deploy to the runtime vertices. It also provides an IF statement that has the same semantics as the general U-SQL IF statement but only allows the statements supported inside a package definition.

The optional parameters can be used inside the package definition to help define variables, and to determine which alternative the `IF` statement will choose.

The package object will be created inside the current database and schema context.

    Create_Package_Statement :=
      'CREATE' 'PACKAGE' ['IF' 'NOT' 'EXISTS'] Identifier '(' [Parameter_List] ')'  
      ['AS']  
      'BEGIN'  
         Package_Statement_List  
      'END'.  

    Package_Statement_List := { Package_Statement }.

    Package_Statement :=
      Using_Statement 
    | Reference_System_Assembly_Statement 
    | Declare_Variable_Statement 
    | Import_Package_Statement 
    | Export_Package_Statement 
    | Deploy_Resource_Statement 
    | If_Package_Statement.

    Export_Package_Statement :=
      Export_User_Assembly_Statement | Export_System_Assembly_Statement | Export_Variable_Statement.

    If_Package_Statement := 
      'IF' Boolean_Expression 'THEN'
        Package_Statement 
      [ 'ELSEIF' Boolean_Expression 'THEN'
        Package_Statement ]
      [ 'ELSE' Boolean_Expression 'THEN'
        Package_Statement ]
      'END'.

###### DROP PACKAGE (U-SQL)

The `DROP PACKAGE` statement drops packages. As in the case with other meta data objects, a package gets dropped even if another package, table-valued function or procedure depends on it. 

    Drop_Package_Statement :=
    'DROP' 'PACKAGE' ['IF' 'EXISTS'] Identifier.

###### IMPORT PACKAGE (U-SQL)

The `IMPORT PACKAGE` statement will import all the assembly references, variable declarations and resource deployments exported by the specified package. The package identifier will be resolved in the static context of its invocation and
can refer to a package in the current account or a different Azure Data Lake Analytics account. The optional argument can provide parameters that can be used inside the package.

If conflicting assemblies or references are being referenced, imported or deployed, errors are raised once a package is getting imported in the main U-SQL script.

    Import_Package_Statement :=
      'IMPORT' 'PACKAGE' Global_Identifier ['(' [Argument_List] ')'] [Package_Alias].

    Package_Alias :=  
      'AS' Package_Name_Alias.

    Package_Name_Alias := 
      Quoted_or_Unquoted_Identifier.

The optional package alias must be used when referring to variables imported from the given package.

####### EXPORT ASSEMBLY and EXPORT SYSTEM ASSEMBLY (U-SQL)

The `EXPORT ASSEMBLY` and `EXPORT SYSTEM ASSEMBLY` statements specify inside a package definition which user or system assemblies respectively are being exported by the package. They also implicitly reference the assembly for the package context. 

The identifier will be resolved in the static context of the package definition, similarly to table-valued functions and procedures and can refer optionally to a user assembly in a different Azure Data Lake Analytics account.

    Export_User_Assembly_Statement :=
      'EXPORT' 'ASSEMBLY' Global_Assembly_Identifier.

    Export_System_Assembly_Statement :=
      'EXPORT' 'SYSTEM' 'ASSEMBLY' Assembly_Name.

    Assembly_Name := 
      Quoted_or_Unquoted_Identifier.

###### EXPORT VARIABLE (U-SQL)

The `EXPORT` statement specifies inside a package definition which variable is being exported by the package. It also implicitly declares the variable inside the package context. For more details about the semantics of U-SQL variables, see [here](https://msdn.microsoft.com/en-us/library/azure/mt621290.aspx).

    Export_Variable_Statement := 
      'EXPORT' ['CONST'] User_Variable_Name [Scalar_Type] '=' csharp_expression.

    User_Variable_Name := "@"+Unquoted_Identifier.
      
    Scalar_Type := Built_in_Type | User_defined_Type.

If a variable of a user-defined type gets exported, then the assembly defining the type should also be exported.

_Examples:_

1. The following example creates a package `XMLFiles` in an already existing database `db1`. The package exports a variable named `@xmlfile` with the value `/configfiles/config.xml` and deploys that file. The file does not need to exist during creation of the package, but it will have to exist at the time the import of the package occurs into the main script.

	    USE DATABASE db1;
        DROP PACKAGE IF EXISTS XMLFiles;
        CREATE PACKAGE XMLFiles()
	    BEGIN
          EXPORT @xmlfile = "/configfiles/config.xml";
          DEPLOY RESOURCE @xmlfile;
        END;

2. The next example is creating a parameterized package `XMLorJSON` in the same database that - based on the passed parameter - bundles the previously created custom assembly `Microsoft.Analytics.Samples.Formats` from the database `JSONBlog` with a different assembly (`NewtonSoft.Json`  from the database `JSONBlog` if the parameter is `json` or the system assembly `System.Xml` if the parameter is `xml`) and a `@format` variable, that is set to different values depending on the parameter. It also imports the previously defined package `XMLFiles` with the alias `xmlpack` and re-exports its `@xmlfile` variable if the parameter is set to `xml`.

        DROP PACKAGE IF EXISTS db1.dbo.XMLorJSON;

        CREATE PACKAGE db1.dbo.XMLorJSON(@requestedFormat string = "json")
        BEGIN
          // @xml and @json are just used inside the package and are not exported
          DECLARE @xml = "xml";
          DECLARE CONST @json = "json"; 

          // The following IMPORT statements will import variables and resource deployments from XMLFiles. 
          // Since there is no automatic re-export, it does not make sense to put an assembly export into
          // a package that only gets imported into a different package.
          // Note that no argument list is needed if no arguments are being passed.

          IF @requestedFormat == @xml THEN 
            IMPORT PACKAGE XMLFiles AS xmlpack;

            EXPORT ASSEMBLY JSONBlog.[Microsoft.Analytics.Samples.Formats];
            EXPORT SYSTEM ASSEMBLY [System.Xml]; 
            EXPORT @xmlfile = xmlpack.@xmlfile;
            EXPORT @format = "xml";

          ELSEIF @requestedFormat == @json THEN
            EXPORT ASSEMBLY JSONBlog.[Microsoft.Analytics.Samples.Formats];
            EXPORT ASSEMBLY [NewtonSoft.Json];
            EXPORT @format = "json";

          ELSE
            EXPORT @format = "invalid";
          END;
        END;

3. The following script uses the now the previously defined package `XMLorJSON` from database `db1` with the parameter `xml` and parses the XML file that was provided in the package's `@xmlfile` variable. Please note that the file deployment will occur as well, but is not really required for this script to work.
 

        IMPORT PACKAGE db1.dbo.XMLorJSON("xml") AS xmlpackage;

        USING xml = Microsoft.Analytics.Samples.Formats.Xml;

        // Currently the EXTRACT FROM and OUTPUT TO clauses do not support package variables.
        // Therefore we need to rename them.
        DECLARE @input = xmlpackage.@xmlfile;

        @configdata =
          EXTRACT option1 string,
                  option2 string,
                  option3 string
          FROM @input
          USING new xml.XmlDomExtractor("/config", 
                                        new SqlMap<string,string>{
                                            {"option1", "option1"},
                                            {"option2", "option2"}, 
                                            {"option3", "option3"}});

        @res =
          SELECT xmlpackage.@format AS format,
                 *
          FROM @configdata;

        OUTPUT @res
        TO "/output/packexample.csv"
        USING Outputters.Csv();

#### The column alias is no longer required for expressions that end with a property or field access

U-SQL so far required a column alias for all expressions that consist of more than a column name inside a `SELECT` clause.
With this refresh, the column alias is no longer required for `SELECT` clause expressions that end with a property or field access where the inferred name does not conflict with any other column name. The column name will be derived from the name of the property/field.

For example, 

    @t =
      SELECT *
      FROM (VALUES("1965")) AS T(c1);

    @r =
      SELECT c1.ToString().Length
      FROM @t;

    OUTPUT @r
    TO "/output/noalias.csv"
    USING Outputters.Csv(outputHeader : true);

will create a rowset with one column with the name `Length`.

## Azure Data Lake Tools for Visual Studio New Capabilities

The following are changes that have been added in the version 2.2.2600.1.

#### New Location of the Data Lake Menu

To align with other VisualStudio Tool extensions, the  Data Lake top level menu have been moved to **Tools > Data Lake** as shown in the following picture:
	
![New Menu Location](https://github.com/Azure/AzureDataLake/blob/master/docs/img/ReleaseNotes/2017-Apr-NewMenuLocation.gif) 

#### "Check for Updates" capability has been enabled

If you are using ADL Tools for VisualStudio 2012, 2013 and 2015, you can go to **Tools > Data Lake > Check for Updates** to see whether there is a newer version available.	

![Check for Updates](https://github.com/Azure/AzureDataLake/blob/master/docs/img/ReleaseNotes/2017-Apr-CheckForUpdates.gif) 

If you are using the ADL Tools in VisualStudio 2017, then **NEEDS TO BE ADDED**.

#### Exporting U-SQL database schemas and sample data and importing that information into your local run environment is now supported
	
You now can export a U-SQL database’s schema and sample data, and can also import the exported database to your (Local) database. Try it through **Server Explorer > ADLA account > database** and then right click and select the menu option **Export**.

![Exporting DB](https://github.com/Azure/AzureDataLake/blob/master/docs/img/ReleaseNotes/2017-Apr-ExportDB.gif) 

The Export option also gives you the ability to import the database with the sample data directly into the local run environment:

![Importing DB](https://github.com/Azure/AzureDataLake/blob/master/docs/img/ReleaseNotes/2017-Apr-ImportDB.png)

#### Local database credential management is now supported

You can now create credentials in your `(Local)` database using `LocalRunHelper.exe`. You can get `LocalRunHelper.exe` from the Data Lake installation folder or from http://www.Nuget.org:

- Data Lake installation folder: `C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\Extensions\Microsoft\ADL Tools\2.2.6000.0\LocalRunSDK`
- Nuget.org SDK: https://www.nuget.org/packages/Microsoft.Azure.DataLake.USQL.SDK/ 

You can find more information about the U-SQL Local Run SDK [here](https://docs.microsoft.com/en-us/azure/data-lake-analytics/data-lake-analytics-u-sql-sdk).

_Examples:_

1. Creating the credentials in the command line:

         >LocalRunHelper.exe credential -DataRoot d:\localrun\dataroot -Add master\TestCredential -User testdbUser -Password testdbPassword -Uri tcp://testdb.database.windows.net:1433
         >LocalRunHelper.exe credential -DataRoot d:\localrun\dataroot -Add master\TestCredential -User testdbUser -Password testdbPassword -DatabaseHost testdb.database.windows.net –Port 1433
         >LocalRunHelper.exe credential -DataRoot d:\localrun\dataroot -Add master\TestCredential -Uri tcp://testdb.database.windows.net:1433   
   In the last statement, the user and password will be asked interactively.

2. Enumerating the credentials (password will not be shown) in the command line:

         >LocalRunHelper.exe credential -DataRoot d:\localrun\dataroot -List master

    returns

         Name:'TestCredential'  \  User:'testdbUser'  \  Uri:'tcp://testdb.database.windows.net:1433'

3. Deleting a credential in the command line:

         >LocalRunHelper.exe credential -DataRoot d:\localrun\dataroot -Delete master\TestCredential

4. Referring to a local credential in a U-SQL script (basically the same as in the cluster):

        DROP TABLE IF EXISTS MySqlDbTestTable;
        DROP DATA SOURCE IF EXISTS MySqlDb;

        CREATE DATA SOURCE MySqlDb
        FROM SQLSERVER
        WITH (CREDENTIAL = TestCredential, PROVIDER_STRING = "Database=MyTestDB2;Timeout=60");

        CREATE EXTERNAL TABLE MySqlDbTestTable
        (oid int, cusid int?, otime DateTime?)
        FROM MySqlDb LOCATION "dbo.Orders";

        @out = SELECT * FROM MySqlDbTestTable;
    
        OUTPUT @out 
        TO "externaldata.csv"
        USING Outputters.Csv();

    Note that you will have to make sure that the local machine's IP address is allowed to access the Azure SQL instances.

## PLEASE NOTE:

In order to get access to the new syntactic features and new tool capabilities on your local environment, you will need to refresh your ADL Tools. You can download the latest version for VS 2013 and 2015 from [here](http://aka.ms/adltoolsvs) or use the Check for Updates menu item mentioned above. If you are using VisualStudio 2017, you currently have to wait for the next VisualStudio 2017 refresh that should occur about every 6 to 8 weeks.

Otherwise you will not be able to use the new features during local run and submission to the cluster will give you syntax warnings for the new language features (you can ignore them and still submit the job).
