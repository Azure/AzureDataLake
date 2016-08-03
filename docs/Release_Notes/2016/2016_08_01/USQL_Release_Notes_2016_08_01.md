# U-SQL Release Notes 2016-08-01
--------------------------
## Pending and Upcoming Deprecations

Please review your code and make sure you are cleaning your existing code to be ready for the following 
deprecations later this year (follow links for more details).

Follow our [Azure Data Lake Blog](http://blogs.msdn.microsoft.com/azuredatalake) for future annoucements of deprecation timelines.

#### Start of Deprecation of old `PARTITIONED BY` Syntax.
 
In order to adjust our syntax with other SQL products and to more clearly differentiate the addressable partitions from the internal data distributions in a table, we are changing the `PARTITIONED BY` syntax in the following way:

Old Syntax:

````
Partition_Specification :=
    'PARTITIONED' ['BY'] [Bucket_Specification] 
    Distribution_Scheme ['INTO' integer_or_long_literal]. 

Bucket_Specification :=
    'BUCKETS' '(' Identifier_List ')'.

Identifier_List :=
    Quoted_or_Unquoted_Identifier {',' Quoted_or_Unquoted_Identifier}.

Distribution_Scheme :=
    'RANGE' '(' Sort_Item_List ')'
|   'HASH' '(' Identifier_List ')' 
|   'DIRECT' 'HASH' '(' Identifier ')' 
|   'ROUND' 'ROBIN'.
````

New Syntax:
````
Partition_Specification :=
    [ 'PARTITIONED' ['BY'] '(' Identifier_List ')' ]
	Distribution_Specification.

Identifier_List :=
    Quoted_or_Unquoted_Identifier {',' Quoted_or_Unquoted_Identifier}.

	Distribution_Specification :=
    'DISTRIBUTED' ['BY'] Distribution_Scheme ['INTO' integer_or_long_literal]. 

Distribution_Scheme :=
    'RANGE' '(' Sort_Item_List ')'
|   'HASH' '(' Identifier_List ')' 
|   'DIRECT' 'HASH' '(' Identifier ')' 
|   'ROUND' 'ROBIN'.
````

_Examples:_

If you want to use HASH partitions to scale out your data processing, you previously would write:

````
CREATE TABLE T(col1 int, col2 string, partcol DateTime, 
               INDEX idx CLUSTERED (col1, col2)
			   PARTITIONED BY HASH (col1) INTO 5);
````

Now you write

````
CREATE TABLE T(col1 int, col2 string, partcol DateTime, 
               INDEX idx CLUSTERED (col1, col2)
			   DISTRIBUTED BY HASH (col1) INTO 5);
````
If you want to add addressable partitions to manage your data life cycle, you previously wrote

````
CREATE TABLE T(col1 int, col2 string, partcol DateTime, 
               INDEX idx CLUSTERED (col1, col2)
			   PARTITIONED BY BUCKET(partcol)
			   HASH (col1) INTO 5);
````

Now you write

````
CREATE TABLE T(col1 int, col2 string, partcol DateTime, 
               INDEX idx CLUSTERED (col1, col2)
			   PARTITIONED BY (partcol)
			   DISTRIBUTED BY HASH (col1) INTO 5);
````

Note that for now, both the old and new syntax are supported. 


#### DateTime file set pattern will require `HH` instead of `hh` for the hour pattern to align with 24h clock semantics

Currently U-SQL supports both forms with 24h semantics. In the future, support for `hh` in file set patterns will be dropped.

_Example:_ 

Let's assume the following file path exists in your ADLS account: `/data/2016/08/01/23/hourlydata.csv` cotaining the word `"test"`.

Please use the following pattern to include the above file in your extract statement:

````
@data = EXTRACT data string, date DateTime
        FROM "/data/{date:yyyy}/{date:MM}/{date:dd}/{date:HH}/hourlydata.csv"
		USING Extractors.Csv();

#### [Deprecation of `{col:*}` File Set pattern](https://github.com/Azure/AzureDataLake/blob/master/docs/Release_Notes/2016/2016_07_14/USQL_Release_Notes_2016_07_14.md)

## Breaking Changes

None.

Note that the release of folder/file/database level access control may impact existing jobs if the access permissions are set incorrectly. If your jobs start failing due to inaccessible files or folders, please check that you have correct permissions. See [this blog post](https://blogs.msdn.microsoft.com/azuredatalake/2016/07/31/introducing-file-and-folder-acls-for-azure-data-lake-store/) for more details.

## Major U-SQL Bug Fixes

#### Record-Boundary Alignment Bug is fixed

In previous releases, if you uploaded files into Azure Data Lake Storage that were larger than 250MB and were record-oriented (e.g., CSV files), you had to upload them as row-oriented files via Powershell or the ADL Tools in VisualStudio.
If you used binary uploads or tools (such as standard webhdfs APIs) that were not aware of this requirement or your row delimiters were not CR and/or LF, you would get error messages during `EXTRACT` ranging from invalid encodings, to wrong column counts.

These issues have been fixed:
- Any row-oriented (CSV-like) file of any size can now be uploaded, assuming that the size of a row is not larger than 4MB.
- Any Unicode (UTF-8, UTF-16 or UTF-32) row delimiter can now be used.

Technically, the row splitter in the UDO model for both built-in and custom extractors is now able to peek into the first 4MB of the next data split to find the end of the row and will start after the first split.

#### Extractor input bytestream level properties provide correct information

As part of fixing the Record-Boundary Alignment issue, the input properties provide the correct information:

`input.Length` is the length of the input segment of the file that is being processed by the extractor (without the 4MB look into the next segment).
`input.Start` is the byte offset of the start of the segment in relationship to the file. It is 0 for the first segment.
`input.BaseStream.Position` is the local position where the reader on the input baseStream is in relation to the segment.

#### Improved error messages

Several error messages received improvements, such as:

1. Better error reporting when duplicate table names are used in `CROSS APPLY` expressions. Example of such invalid naming: 

       @q4 = SELECT i FROM @a CROSS APPLY EXPLODE(a1) AS T(i) 
	                       CROSS APPLY EXPLODE(a2) AS T(i);

2. Error for the unsupported "TOP" syntax now guides users to use one of the two supported constructs for restricting the row count, ORDER BY FETCH clause or SAMPLE ANY clause.
	
3. Added more details to the resolution steps of the error message for when there is an unexpected number of columns in the input stream (E_EXTRACT_UNEXPECTED_NUMBER_COLUMNS)

## New U-SQL capabilities

#### U-SQL now offers a preview of sampling capabilites (requires a simple email signup)
 
U-SQL is providing a preview of several sampling expressions, such as

1. Inline operator for simple cases that provides random and probabilistic sampling: 
````
@data0 = SELECT * FROM @data SAMPLE ANY(5);
````
Samples the `@data` rowset by selecting any number of rows randomly (`5` in the example above).

````
@data1 = SELECT * FROM @data SAMPLE UNIFORM (0.5);
````

Samples the `@data` rowset using a random uniform sampling with the provided probability (50% in the example above).

2. Complex expression provides the ability to add weight information and provides more complex samplers:

````
@data2 =  SAMPLE @data ON name, deviceName UNIVERSE (0.1) WITH WEIGHT AS x; 
````
Samples the `@data` rowset such that the `name` and `deviceName` columns values are in some randomly chosen fraction 
of the overall value space (10% in the above example) and adds a weight column called `x` to the resulting rowset. 
U-SQL uses cryptographically strong hash functions to pick a random portion. 
 
````
@data3 = SAMPLE @data ON name DISTINCT (0.1, 2); 
````

Samples the `@data` rowset such such at least 2 rows (in the example) per distinct value in the column `name` 
are included, and additional rows are added with the indicated probability (10% in the example above).

If you are interested in trying the sampling operators out before it gets enabled, please contact us at the usql@microsoft.com email address.

#### U-SQL supports a compile-time IF statement

U-SQL now offers an IF statement which will evaluate its expression at compile time (the Boolean expression has to be compile-time constant foldable, such as the value passed as a parameter).

_Example:_

````
IF @debugstate == 1 THEN 
 // insert U-SQL statement list
ELSEIF @debugstate == 2 THEN 
 // ELSEIFs can be repeated and are optional
 // insert other U-SQL statement list
ELSE 
 // ELSE is optional
 //insert the final U-SQL statement list
END;
````

#### U-SQL built-in extractors now support skipping header rows (`skipFirstNRows` parameter)

U-SQL's built-in row-oriented extractors such as `Csv()` now support the `skipFirstNRows` that allows to specify the number of rows to be skipped.
Note that the rows being skipped do not need to conform to the column schema in either type or count of columns.
_Example:_

Let's assume that the file `input.csv` looks like:

````
col1,col2
secondrow data
1,2
3,4
````
Then the following script
````
@data = EXTRACT c1 int, c2 int FROM "input.csv" USING Extractors.Csv(skipFirstNRows:2);

OUTPUT @data TO "output.csv" USING Outputters.Csv();
````

creates the file `output.csv` with the content

````
1,2
3,4
````
Note you can only skip rows that are in the first segment of a file. If you try to skip more rows, an error will be raised.

#### U-SQL now offers database level access control

U-SQL now offers the ability to set access permissions on databases in addition to access permissions to the catalog as a whole.

The creator of the ADLA account will be the owner of the catalog and the `master` database (that cannot be deleted).

Enumeration access provides the right to see the objects and their definition inside a database, but not run queries against the data in tables. Note that views and TVFs that access files will have to be secured by setting ACLs on the files.
Read access provides the right to query the data in the database's tables.
Write access provides the right to create objects in the database and insert data into its tables.

The `master` database per default is both read and write accessible to any authorized security principal (user or security group) of the given ADLA account.

All other databases per default are owned by the creator of the database who has to give other users and security groups explicit permissions.

Access permissions on databases currently can only be set through the Azure Portal.

Note that access permissions on databases created before the release have been propagated as part of the release. So if you had access to the database `myDB` before the update, you should still have the same access after the update. However, all newly added users will need to be granted explicit permissions on all previously existing databases (including `master`
## PLEASE NOTE:either by an explicit grant or by adding the user to an authorized security group.

In order to get access to the new syntactic features and new tool capabilities on your local environment, you will need to [refresh your ADL Tools](http://aka.ms/adltoolsvs). Otherwise you will not be able to use them during local run and submission to the cluster will give you syntax warnings for the new language features.
