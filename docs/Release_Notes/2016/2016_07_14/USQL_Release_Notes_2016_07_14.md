# U-SQL Release Notes 2016-07-14
--------------------------

## Breaking Changes
 
 #### Start of Deprecation of `{col:*}` File Set pattern.
 
 So far, one could write either `{col}` or `{col:*}` in a File Set pattern to wildcard part of a file path and 
 expose that part as a column with the name `col`.
`{col}` required that a predicate on `col` was specified in the script while `{col:*}` indicated that 
the predicate was not required.

After receiving some customer feedback and observing the customer issues, we decided that offering these two closely 
related options added too much complexity. Thus, the `{col:*}` pattern will be deprecated in the following steps in starting 
with this release:

1. _Current release_: U-SQL supports both `{col}` and `{col:*}` as synonyms without requiring the predicate. A warning is raised if the script uses the `{col:*}` pattern.

2. _A future release_ (probably before the GA release): U-SQL will remove `{col:*}` and will only support `{col}`.

So please start to replace your {col:*} with {col}. 

We will announce the future phase out on our blog/release notes.

As an aside: The `{*}` pattern is still supported for unnamed wildcards.

## Major U-SQL Bug Fixes

#### Space encoding in file paths are fixed

In previous releases, specifying paths with special characters such as spaces were causing the following problems:
- Files paths with spaces were not found in file set specifications. 
- Some file locations would work with URI escaped paths but others would not.

These issues have been fixed:
- File Set wildcards will match file paths with special characters in file paths such as spaces and will expose 
them in the virtual column unescaped, e.g. a virtual column pattern matching the file part "filename with spaces" 
will now will show "filename with spaces". 
- In order to specify spaces in file paths, the provided file path has to be specified with the standard 
URL encoding for spaces: `%20`. For example, if you want to `EXTRACT` from a file "filename with spaces.csv" you write
`FROM "filename%20with%20spaces.csv"`.

#### Data corruption due to non-determinism during writing into a U-SQL table is fixed

Under some conditions such as a plan that contains vertices that write to a table and any other location (including another vertex, table or file), 
and one of these vertices has to be rerun, there is a chance that the table meta data was not correctly updated. 
This would manifest itself in corrupted, non-readable tables. With this release, this condition has been addressed.

## New U-SQL capabilities

#### PRESORT on REDUCE is now supported to provide the ability to do custom ordered aggregators/reducers.

For an example see https://blogs.msdn.microsoft.com/azuredatalake/2016/06/27/how-do-i-combine-overlapping-ranges-using-u-sql-introducing-u-sql-reducer-udos/.
 

#### U-SQL now supports adding and removing columns on U-SQL tables

U-SQL now supports the ability to add columns to or remove columns from U-SQL tables with the following statement:

````
Alter_Table_AddDrop_Column_Statement :=
 	'ALTER' 'TABLE' Identifier 
	( 'ADD' 'COLUMN' Column_Definition_List 
	| 'DROP' 'COLUMN' Identifier_List  ).
````
Examples:
````
DROP TABLE IF EXISTS Logs;
CREATE TABLE Logs(date DateTime, 
                  eventType int, 
                  eventTime DateTime, 
                  INDEX Index_EventType CLUSTERED (eventType ASC)  PARTITIONED BY (date) DISTRIBUTED BY HASH(eventType) INTO 3);

// Add a column
ALTER TABLE Logs ADD COLUMN eventName string;

// add another column
ALTER TABLE Logs ADD COLUMN result string;

// drop a column and add another one
ALTER TABLE Logs DROP COLUMN result;
ALTER TABLE Logs ADD COLUMN clientId string;

// drop a column and add 3 more columns
ALTER TABLE Logs DROP COLUMN clientId;
ALTER TABLE Logs ADD COLUMN result string, clientId string, payload string;

// drop 2 columns
ALTER TABLE Logs DROP COLUMN clientId, result;
````

#### U-SQL now supports referencing custom assembly objects such as C# user-defined functions inside `DECLARE` statement.

Example:

    REFERENCE ASSEMBLY myassembly;
    
    DECLARE @myvar string = "string "+myns.myclass.mystringfn();
	
Note that these declarations are not constant-foldable.

#### U-SQL now supports a `USING` directive that allows shortening U-SQL custom code object references.

The new `USING` directive is similar to the C# `using` directive in that it allows to shorten function, 
type, extention methods, and UDO path names.

Syntax:

````
Using_Directive := 'USING' 	csharp_namespace 
                          | Alias '=' csharp_namespace_or_type. 
````

Examples: 
In this example we default C# namespace resolution to the specified namespace:
````
DECLARE @ input string = "somejsonfile.json";

REFERENCE ASSEMBLY [Newtonsoft.Json];
REFERENCE ASSEMBLY [Microsoft.Analytics.Samples.Formats];

USING Microsoft.Analytics.Samples.Formats.Json;

@data0 = 
    EXTRACT IPAddresses string
    FROM @input
    USING new JsonExtractor("Devices[*]");

...
````
In this example we use an alias to shorten the namespace path with an alias:
````
DECLARE @ input string = "somejsonfile.json";

REFERENCE ASSEMBLY [Newtonsoft.Json];
REFERENCE ASSEMBLY [Microsoft.Analytics.Samples.Formats];

USING json = Microsoft.Analytics.Samples.Formats.Json;

@data0 = 
    EXTRACT IPAddresses string
    FROM @input
    USING new json.JsonExtractor("Devices[*]");

...
````

In the following example, we alias the full type name:
````
DECLARE @ input string = "somejsonfile.json";

REFERENCE ASSEMBLY [Newtonsoft.Json];
REFERENCE ASSEMBLY [Microsoft.Analytics.Samples.Formats];

USING json = Microsoft.Analytics.Samples.Formats.Json.JsonExtractor;

@data0 = 
    EXTRACT IPAddresses string
    FROM @input
    USING new json("Devices[*]");

...
````

## PLEASE NOTE:
In order to get access to these new syntactic features on your local environment, you will need to refresh your ADL Tools. Otherwise 
you will not be able to use them during local run and submission to the cluster will give you syntax warnings.
