# U-SQL Release Notes 2016-04-11
--------------------------

## Breaking Changes
 
None.

## Major U-SQL Bug Fixes

#### MD5 Hash for WASB files is now correctly written

Previously, when writing a file to WASB locations, the MD5 hash was not set correctly. Tools that checked the MD5 hash, such as Azure Data Explorer or Polybase, failed. The MD5 hash is now correctly set.

#### CREATE TYPE is fixed

U-SQL allows the creation of table types to be used in U-SQL function and procedure signatures. In this release, some meta data catalog issues have been fixed so the type should be usable. 

Example: 
 
    CREATE TYPE IF NOT EXISTS MyTableType AS TABLE (id int, data string); 

    CREATE FUNCTION IF NOT EXISTS MyTVF(@t MyTableType) RETURNS @r 
    AS
    BEGIN
        @r = SELECT * FROM @t;
    END;
        
