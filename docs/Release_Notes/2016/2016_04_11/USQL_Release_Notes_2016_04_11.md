
Azure Data Lake > U-SQL Release Notes 2016-04-11
--------------------------

# BREAKING CHANGES
 
No known breaking changes.

# NEW FEATURES
 
No new features. Several new language features are in the pipeline though.

# MAJOR BUG FIXES

## MD5 Hash for WASB files is fixed 

Previously, when writing a file to WASB locations, the MD5 hash has was not set correctly and tools that checked the MD5 hash, such as Azure Data Explorer or Polybase, failed. This issue now has been fixed. 

## CREATE TYPE is fixed 
U-SQL allows the creation of table types to be used in U-SQL function and procedure signatures. In this release, some meta data catalog issues have been fixed so the type should be usable. Example: 
 
CREATE TYPE MyTableType AS TABLE (id int, data string); 



 



