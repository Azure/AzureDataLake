Azure Data Lake Release Notes 2016/02/16

# BREAKING CHANGES
 
## UDO USING Expressions
 
Starting with this release, UDO USING expressions are bound only against the referenced assemblies in the current script/code object. 

For example, before in previous releases if you referenced an assembly outside a VIEW, FUNCTION or PROCEDURE body, they were visible inside. 
 
Now in this release they will not be visible by default.  To make them visible, please move the REFERENCE assembly statement into the body of a FUNCTION or PROCEDURE. In case of a VIEW, please rewrite the view as a table-valued function.
 
## UDTs no longer leak into the other scopes
 
Previously, User-defined types (UDTs) in the EXTRACT/PRODUCE clauses are bound against assemblies that were referenced in the main script only. Thus, referenced assemblies in the main script were leaked into code objects such as FUNCTIONs or PROCEDUREs (or VIEWs) and these code objects were not self-contained anymore 

1. Starting with this release, UDTs in EXTRACT/PRODUCE clauses are bound only against the referenced assemblies in the current script/code object.

2. For example, if you used a UDT in a VIEW, FUNCTION or PROCEDURE body, they were visible inside. Now they will not visible. To make them visible, please move the REFERENCE assembly statement into the body of a FUNCTION or PROCEDURE. In case of a VIEW, please rewrite the view as a table-valued function.
 
 
3.      As a consequence of changes 1 and 2, VIEW definitions cannot contain any user-defined functions (UDF), types (UDT), aggregators (UDAGG), nor operators (UDO). This is because the body of a VIEW can only contain a single query statement and no REFERENCE ASSEMBLY statements. Thus a VIEW's body is always resolved against system assemblies only. Please rewrite any view that needs to refer to a user-defined object (UDx) into a table-valued function.
 
4.      Before this release, FUNCTION and PROCEDURE bodies were not syntax checked during creation, but when being called. 
Starting with this release, syntax checks are being performed during the creation of the FUNCTION or PROCEDURE.
 
5.      SET statements on system-variables, e.g., SET @@baseURI, cannot contain variable references anymore (see also new capabilities below).


# NEW FEATURES
 
## DIRECT HASH PARTITIONING

You can now use DIRECT HASH partitioning with U-SQL Tables. This allows you to use a column that will directly indicate which partition number in which to partition to the data.
 
    <Example Script>
 
## U-SQL TABLE INSERTs

Previously the initial INSERT for a U-SQL table had to be in one job, and then append INSERT(s) had to be in a separate job. Now the initial INSERT and subsequent append INSERT(s) can be in the same job

For example, if we have a table T with two int columns. Then 
 
    INSERT INTO T VALUES (1, 2); // First insert into the table
    INSERT INTO T VALUES (3, 4); // Subsequent insert
 
Can be written in the same script now.
 
## Support for 2-part names 

2-part names are now allowed in DDL  statements for ASSEMBLY, CREDENTIAL, and DATA SOURCE objects, and 3-part names are now supported for TYPE DDL statements.
 
## Additional Window Functions

LAG/LEAD analytical functions are now supported.
 
## Functions can infer return types 

Functions now offer the ability to have the return types of the function inferred from the resulting expressions. 
 
Example: 
 
    CREATE FUNCTION InferredResult(@p1 int) 
        RETURN @res 
        AS BEGIN 
            @res = SELECT * FROM (VALUES(@p1, “string”)) AS v(i,s); 
        END; 
 
This Function will infer the result type as TABLE(i int, s string). 

Functions still allow you to specify the return types, in which case the result expressions will be type checked against the expected type.

## Using the SET statement
 
SET statements can be used anywhere in the script. They will apply to the full script and not just the part after the SET statement! As a consequence, SET statements cannot refer to other values produced by SET statements.  
 
 
## ALTER TABLE REBUILD

This release adds support for ALTER TABLE REBUILD. It is used to compact partitions and tables that have grown by multiple, incremental insertions into the same partitions in order to improve query performance over such tables.


