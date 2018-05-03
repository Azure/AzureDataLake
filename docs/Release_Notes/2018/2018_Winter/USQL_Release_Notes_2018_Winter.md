# U-SQL Winter 2017/2018 Release Notes 2018-04-30
--------------------------
## Table of Content
1. [Pending and Upcoming Deprecations and Breaking Changes](#pending-and-upcoming-deprecations-and-breaking-changes)

    1.1 [U-SQL jobs will introduce an upper limit for the number of table-backing files being read](#u-sql-jobs-will-introduce-an-upper-limit-for-the-number-of-table-backing-files-being-read)

    1.2 [Built-in extractors will change mapping of empty fields from zero-length string to null with quoting enabled](#built-in-extractors-will-change-mapping-of-empty-fields-from-zero-length-string-to-null-with-quoting-enabled)

    1.3 [Disallow `DECLARE EXTERNAL` inside packages, procedures and functions](#disallow-declare-external-inside-packages-procedures-and-functions)

    1.4 [Removal of undocumented use of `CLUSTER` | `CLUSTERED BY` in `CREATE INDEX` and `CREATE TABLE`](#removal-of-undocumented-use-of-cluster--clustered-by-in-create-index-and-create-table)

    1.5 [Disallow U-SQL identifiers in C# delegate bodies in scripts](#disallow-u-sql-identifiers-in-c-delegate-bodies-in-scripts)

    1.6 [Strengthening of Read after DML check](#strengthening-of-read-after-dml-check)

2. [Breaking Changes](#breaking-changes)

    2.1 [The following previously announced deprecation items are now removed.](#the-following-previously-announced-deprecation-items-are-now-removed)

    2.2 [API changes for the cognitive extension libraries](#api-changes-for-the-cognitive-extension-libraries)

3. [Major U-SQL Bug Fixes, Performance and Scale Improvements](#major-u-sql-bug-fixes-performance-and-scale-improvements)

    3.1 [Input File Set scales orders of magnitudes better](#input-file-set-scales-orders-of-magnitudes-better-finally-released)

    3.2 [U-SQL Python, R and Cognitive Extensions bug fixes and performance and scale improvements](#u-sql-python-r-and-cognitive-extensions-bug-fixes-and-performance-and-scale-improvements)

4. [U-SQL Preview Features](#u-sql-preview-features)

    4.1 [Input File Set uses less resources when operating on many small files (Public Preview)](#input-file-set-uses-less-resources-when-operating-on-many-small-files-is-in-public-preview)

    4.2 [Built-in Parquet Extractor and Outputter (Public Preview)](#built-in-parquet-extractor-and-outputter-is-in-public-preview)

    4.3 [Automatic GZip compression on `OUTPUT` statement  (Public Preview)](#automatic-gzip-compression-on-output-statement-is-in-public-preview)

    4.4 [Built-in ORC Extractor and Outputter (Private Preview)](#built-in-orc-extractor-and-outputter-is-in-private-preview)

    4.5 [Data-driven Output Partitioning with `OUTPUT` fileset is in Private Preview](#data-driven-output-partitioning-with-output-fileset-is-in-private-preview) 

    4.6 [A limited flexible-schema feature for U-SQL table-valued function parameters is now available for private preview](#a-limited-flexible-schema-feature-for-u-sql-table-valued-function-parameters-is-now-available-for-preview-requires-opt-in)

5. [New U-SQL capabilities](#new-u-sql-capabilities)

    5.1 [U-SQL adds job information system variable `@@JOBINFO`](#u-sql-adds-job-information-system-variable-jobinfo)

    5.2 [U-SQL adds support for computed file property columns on `EXTRACT`](#u-sql-adds-support-for-computed-file-property-columns-on-extract)

    5.3 [DiagnosticStream support in .Net User-code](#diagnosticstream-support-in-net-user-code)

    5.4 [Built-in Text/Csv/Tsv Extractors and Outputters support ANSI/Windows 8-bit codepage encodings](#built-in-textcsvtsv-extractors-and-outputters-support-ansiwindows-8-bit-codepage-encodings)

    5.5 [U-SQL supports ANSI SQL `CASE` expression](#u-sql-supports-ansi-sql-case-expression)

    5.6 [U-SQL adds C# `Func<>`-typed variables in `DECLARE` statements (named lambdas)](#u-sql-adds-c-func-typed-variables-in-declare-statements-named-lambdas)

    5.7 [U-SQL adds temporary, script-bound meta data objects with `DECLARE` statements](#u-sql-adds-temporary-script-bound-meta-data-objects-with-declare-statements)

    5.8 [The `ORDER BY FETCH` clause can be used with all query expressions](#the-order-by-fetch-clause-can-be-used-with-all-query-expressions)

    5.9 [The `EXTRACT`, `REDUCE` and `COMBINE` expressions now support a `SORTED BY` assertion](#the-extract-reduce-and-combine-expressions-now-support-a-sorted-by-assertion)

    5.10 [The `REQUIRED` clause for UDO invocations now allows `NONE`](#the-required-clause-for-udo-invocations-now-allows-none)

    5.11 [The `EXTRACT` expressions now support the `REQUIRED` clause to support column pruning in user-defined extractors](#the-extract-expressions-now-support-the-required-clause-to-support-column-pruning-in-user-defined-extractors)

    5.12 [U-SQL adds compile-time user errors and warnings](#u-sql-adds-compile-time-user-errors-and-warnings)

    5.13 [U-SQL User-defined Operators can now request more memory and CPUs with annotations](#u-sql-user-defined-operators-can-now-request-more-memory-and-cpus-with-annotations)

    5.14 [U-SQL Cognitive Library additions](#u-sql-cognitive-library-additions)

6. [Azure Data Lake Tools for Visual Studio New Capabilities](#azure-data-lake-tools-for-visual-studio-new-capabilities)

    6.1 [ADL Tools for VisualStudio provides an improved Analytics Unit modeler to help improve a job's performance and cost](#adl-tools-for-visualstudio-provides-an-improved-analytics-unit-modeler-to-help-improve-a-jobs-performance-and-cost)

    6.2 [Job Submission's simple interface now makes it easier to change the allocated AUs](#job-submissions-simple-interface-now-makes-it-easier-to-change-the-allocated-aus)

    6.3 [Improved visualization of the job execution graph inside a vertex](#improved-visualization-of-the-job-execution-graph-inside-a-vertex)

    6.4 [The job stage graph and job execution graph now indicates if the stage contains user-defined operators and what language they have been authored in](#the-job-stage-graph-and-job-execution-graph-now-indicates-if-the-stage-contains-user-defined-operators-and-what-language-they-have-been-authored-in)

    6.5 [Separate tab for enumerating all input and output data](#separate-tab-for-enumerating-all-input-and-output-data)

    6.6 [Job View includes a link to the diagnostic file's folder](#job-view-includes-a-link-to-the-diagnostic-files-folder)

7. [Azure Portal Updates](#azure-portal-updates)

    7.1 [The portal provides an improved Analytics Unit modeler to help improve a job's performance and cost](#the-portal-provides-an-improved-analytics-unit-modeler-to-help-improve-a-jobs-performance-and-cost)


----------

Most of the examples below are available in a sample Visual Studio solution on the [U-SQL GitHub site](https://github.com/Azure/usql/tree/master/Examples/Winter-2017-2018-ReleaseNotes).

--------------------------
## Pending and Upcoming Deprecations and Breaking Changes

Please review your code and make sure you are cleaning your existing code to be ready for the following 
deprecations of U-SQL preview features.

**Please note: Previously announced deprecation items are now deprecated and raise errors instead of warnings!**

Follow our [Azure Data Lake Blog](http://blogs.msdn.microsoft.com/azuredatalake) for more details and future announcements of deprecation timelines.

#### U-SQL jobs will introduce an upper limit for the number of table-backing files being read

U-SQL tables are backed by files. Each table partition is mapped to its own file, and each `INSERT` statement adds an additional file (unless a table is rebuilt with `ALTER TABLE REBUILD`). 

If the file count of a table (or set of tables) grows beyond a certain limit and the query predicate cannot eliminate files (e.g., due to too many insertions), there is a large likely-hood that the compilation times out after 25 minutes. 

In previous releases, there was no limit on the number of table files read by a single job. In the current release, we raise the following warning if the number of table-backing files exceeds the limit of 3000 files per job:

>Warning: WrnExceededMaxTableFileReadThreshold

>Message: Script exceeds the maximum number of table or partition files allowed to be read. 

This message will be upgraded to an error message in a future refresh. You can turn on the error message today with the following setting:

    SET @@InternalDebug = "EnforceMaxTableFileReadThreshold:on";

If your current job compiles but receives the warning today, you will continue to be able to run your job in the future, after we will turn this into an error, with the following setting:

    SET @@InternalDebug = "EnforceMaxTableFileReadThreshold:off";

Please note: This limit only applies to reading from table-backing files. Normal files don't have an explicit limit and have a much higher limit in practice since they use a different execution plan. The limit also only applies to files that are actually read and ignores the files in table partitions which are not used by the query.

In a future release, we will work on addressing this limit (no timeline yet). If you feel this is important to your scenario, please add your vote to the [feature request](https://feedback.azure.com/forums/327234-data-lake/suggestions/19050232-increase-number-of-u-sql-table-files-partitions-th).


#### Built-in extractors will change mapping of empty fields from zero-length string to null with quoting enabled

In a future refresh, we will change the handling of empty cells in CSV-like files by the built-in extractors and align them with the documentation.

Currently, a CSV file `a.csv` with the content

    1,,"b"

and the following statement

    @a = EXTRACT c1 int, c2 string, c3 string FROM "a.csv" USING Extractors.Csv();

would produce a rowset with the values

	| c1 | c2 |  c3 |
    |----|----|-----|
    |  1 | "" | "b" |

instead of the rowset with the values

    | c1 |   c2 |  c3 |
    |----|------|-----|
    |  1 | null | "b" |

We now give you an option to enable this change so you can test your existing scripts and see if you are going to be impacted. 

If you want to test your script with the new behavior, please add the following statement to your script to turn on the correct `null` value extraction:

    SET @@InternalDebug = "EmptyStringAsNullWhenQuoting:on"; 


At some point in the future (with detailed warnings), we will change the default behavior of that switch and always map the empty field to null. **Until then, we strongly recommend that you develop new scripts with the new behavior explicitly turned on!**

#### Disallow `DECLARE EXTERNAL` inside packages, procedures and functions

Currently `DECLARE EXTERNAL` and `EXPORT EXTERNAL` (in packages only) are syntactically allowed inside U-SQL packages, procedures and functions, even though they do not really make sense. Currently the following warning is raised:

>Warning: W_CSC_USER_EXTERNALNOTINMAINSCRIPT

>Message: Found EXTERNAL in procedure/package/function.

In a future release this warning will be turned into an error.

#### Removal of undocumented use of `CLUSTER` | `CLUSTERED BY` in `CREATE INDEX` and `CREATE TABLE`

The implementation allows the undocumented use of `CLUSTER BY` and `CLUSTERED BY` as synonyms for `DISTRIBUTE BY` and `DISTRIBUTED BY` in the `CREATE INDEX` and `CREATE TABLE` statements. 

A future release will remove these synonyms to enable us to introduce other capabilities in this space. If you use this undocumented syntax, please replace it with the documented keywords. A future refresh will produce a warning and then an error.

#### Disallow U-SQL identifiers in C# delegate bodies in scripts

Since C# delegate bodies (C# blocks enclosed in `{}`) are normal C# from a binding perspective, U-SQL identifiers should be passed in as parameters to the delegate instead. We currently allow U-SQL identifiers in C# delegate bodies, but may remove them in the future.

#### Strengthening of Read after DML check

U-SQL script execution currently does not allow reading from a table that has been inserted into previously or concurrently in the same script (reading before-hand is fine).

The next release will strengthen this check, in particular it will also take table actions across code objects such as table-valued functions into account.

In most cases such reads would have already been failing with a system error `STORE_FILE_NOT_FOUND`.

This system error is now turned into a compile time error calling out that read after insert is not allowed.

In the following few rare cases however, compilation could have passed successfully, although not necessarily in a consistent manner (some invocations in some cases could still produce the error above):

- Read from the table is optimized away: There could be several reasons for this such as partition elimination, file set elimination etc. The outcome of compilation (successful versus failure with above mentioned system error) cannot be guaranteed and is dependent on external factors such as which files were present in the store. 

- Truncate the table/partition between `INSERT` and read operations:
In this case truncation would have to operate on all table distribution groups on which the `INSERT` was performed. This results in loss of all the inserted data anyways.

Going forward both cases will result in the aforementioned compilation error.

The suggested mitigations are:

- Split the job into two jobs that perform `INSERT` and `SELECT` operations separately

- Write the `SELECT` as a union of the old state of the table and the newly added rows, thus not reading from the table after the DML has occurred.

## Breaking Changes

#### The following previously announced deprecation items are now removed

Please check previous release notes for previously deprecated items and follow the links for details on the items below.

The recent refresh raises errors for the following deprecated items:

  1. [Disallowing user variables that start with `@@`](https://github.com/Azure/AzureDataLake/blob/master/docs/Release_Notes/2017/2017_Summer/USQL_Release_Notes_2017_Summer.md#disallowing-user-variables-that-start-with-)
 
    U-SQL reserves variables starting with `@@` for system variables, and and raises the following error if a user tries to define a variable starting with `@@`: 

    > Error: E_CSC_USER_SYNTAXERROR

    > Description: Invalid syntax found in the script.

    > Message: syntax error. Expected one of: DOTNET FUNCTION JVM PROCEDURE PYTHON TYPE VIEW ASSEMBLY CONST EXTERNAL variable at token '@@invalid_var'.

    Use a single `@` for your custom variables, e.g., `@valid_var`.

 2. [Table-valued functions disallow result variable names that conflict with parameter names](https://github.com/Azure/AzureDataLake/blob/master/docs/Release_Notes/2017/2017_Summer/USQL_Release_Notes_2017_Summer.md#table-valued-functions-will-disallow-result-variable-names-to-conflict-with-parameter-names)

    Table-valued functions disallow a result variable from having the same name as any of the function's parameters. The following error is raised:

    > Error: E_CSC_USER_ROWSETVARIABLEALREADYDECLARED

    > Description: The same variable cannot be assigned both a rowset and a scalar value.

    > Message: Rowset variable {0} is already declared as a scalar variable..

    Please change the names to disambiguate your return and parameter names.

#### API changes for the cognitive extension libraries

Azure Data Lake Analytics provides a set of libraries for running Python and R code and use some of the cognitive processing capabilities on images and text that can be installed as U-SQL extensions via the Azure Data Lake Analytics Portal. 

These assemblies are currently considered to be in a preview release stage. Therefore, more changes may occur in future refreshes, such as moving the text assemblies to use the same extractor/applier model as the image assemblies.


The recent refresh introduced the following breaking changes:

##### The `Cognition.Vision.OcrExtractor` Processor has been replaced with an Extractor and Applier

This UDO has been replaced by an equivalent extractor and applier. For more details on the replacements see [below](#ocr-extraction-is-now-available-as-an-extractor-and-an-applier).

##### The `Cognition.Text.KeyPhraseExtractor` Processor has been renamed to `Cognition.Text.KeyPhraseProcessor` and its signature has changed

This UDO has been renamed to `Cognition.Text.KeyPhraseProcessor` and instead of returning the key phrases as a ; separated string, the `keyphrases` column is now a `SqlArray<string>`. For more details on the changed UDO and the additional extractor see [below](#ocr-extraction-is-now-available-as-an-extractor-and-an-applier).

**NOTE: If you already have installed an earlier version of the extensions, you will have to reinstall the newest version from the Azure Portal.**

## Major U-SQL Bug Fixes, Performance and Scale Improvements

Besides many internal improvements and fixes to reported bugs, we would like to call out the following major bug fixes and improvements to performance and scalability of the language.

#### Input File Set scales orders of magnitudes better (Finally released!)

Previously, U-SQL's file set pattern on `EXTRACT` expressions ran into compile time time-outs around 800 to 5000 files. 

U-SQL's file set pattern now scales to many more files and generates more efficient plans.

For example, a U-SQL script querying over 2500 files in our telemetry system previously took over 10 minutes to compile 
now compiles in 1 minute and the script now executes in 9 minutes instead of over 35 minutes using a lot less AUs.

We  have compiled scripts that access 30'000 files and have seen customers successfully compile scripts with over 150'000 files.

This means that you do not need to specify any of the following preview features anymore (although if you use them, they will still work):

	SET @@FeaturePreviews = "FileSetV2Dot5:on";
	SET @@FeaturePreviews = "AsyncCompilerStoreAccess:on";

If you want to turn these improvements off, for example to test the impact of the improvement by measuring the before state or because you think you may have run into a bug related to the improvement, you can turn them off. For example:

	SET @@FeaturePreviews = "FileSetV2Dot5:off";

#### U-SQL Python, R and Cognitive Extensions bug fixes and performance and scale improvements

##### The extension UDOs can use up to 5GB of memory

The R, Python and Cognitive Extension UDOs have been updated to provide access to up to 5GB of memory (up from 500MB) starting in version 1.0.6. If you are using an older version of the extensions, please visit the Azure Portal to update to the latest version.

##### U-SQL Python Extension provides data frame indexing by column name

The Python extension now allows access to data frame columns by name regardless of the width of the data frame. Before this, indexing by column name only worked for data frames with 10 columns and access to wider data frames was restricted to indices.

##### U-SQL R Extension handles nullable types

The U-SQL R extension now supports nullable types.


## U-SQL Preview Features

We currently have the following U-SQL features in public or private preview. A feature in preview means that we are still finalizing the implementation of the feature, but are soliciting feedback and want to make it available ahead of a full release due to their value to the scenarios they address and the ability to learn more from our customers.

A _private preview_ feature means that you have to contact us for access since it often involves extra steps (like specifying a special runtime), is only working in a limited context (e.g., it does not work with all other capabilities), or may be removed/not shipped due to customer feedback.

**Since we are still testing these features, you are required to opt in. Please [contact us](mailto:usql@microsoft.com) if you want to explore any of the private preview capabilities.**

A _public preview_ feature means that we provide you with the opt-in setting in the release notes and the feature is normally accessible in the default experience after you opt-in. Compared to a released feature, it may still change its APIs or details in behavior, but we plan on releasing the feature and will provide normal support if you encounter any issues.

#### Input File Set uses less resources when operating on many small files (is in Public Preview)

While we do not recommend that you operate on many small files due to the inherent overheads of having to enumerate a lot of file system meta data and the overhead of running extractor code on small files, we have added a performance and scale improvement that helps improve performance, scale and costs of jobs that need to process many smaller files. 

You can turn the improvement on with the following preview option:

    SET @@FeaturePreviews = "InputFileGrouping:on";

For example, if you process image files, you may want to operate on a large set of image files in the mega byte range. In the following example, I am operating on a set of JPEG images in the KB to MB range and load them into a U-SQL table for later processing. Since the images are larger than the U-SQL rowsize, I will create a thumbnail during the extraction using the sample code available on the [U-SQL Github site](https://github.com/Azure/usql/tree/master/Examples/ImageApp) using the following script:

    // SET @@FeaturePreviews = "InputFileGrouping:on"; // uncomment this line to turn improvement on

    DROP TABLE IF EXISTS T;

    REFERENCE ASSEMBLY Images;

    @image_features =
      EXTRACT thumbnail byte[], 
              name string, format string
      FROM @"/Samples/Data/Images/{name}.{format}"
      USING new Images.ImageFeatureExtractor(scaleWidth:500, scaleHeight:300);

    CREATE TABLE T(INDEX idx CLUSTERED(filename) DISTRIBUTED BY HASH (filename)) AS
      SELECT name+"."+format AS filename, thumbnail 
      FROM @image_features
      WHERE format IN ("JPEG", "jpeg", "jpg", "JPG");

If I run the script without the new improvement, I will get one vertex per file:

![Job Graph - No File Grouping](https://github.com/Azure/AzureDataLake/blob/master/docs/img/ReleaseNotes/winter2018/2018-winter-filesets-nogrouping-jobgraph.jpg) 

and each vertex will spend more time in creating the vertex than actually doing the thumbnail creation:

![Vertex Execution View - No File Grouping](https://github.com/Azure/AzureDataLake/blob/master/docs/img/ReleaseNotes/winter2018/2018-winter-filesets-nogrouping-vertexexecution.jpg) 

However, if I turn the feature preview on, the job will pack several files into a vertex, thus reducing the complexity of the stage to two vertices:

![Job Graph - With File Grouping](https://github.com/Azure/AzureDataLake/blob/master/docs/img/ReleaseNotes/winter2018/2018-winter-filesets-withgrouping-jobgraph.jpg) 

and improving the cost of each vertex execution by doing more work in a vertex:

![Vertex Execution View - With File Grouping](https://github.com/Azure/AzureDataLake/blob/master/docs/img/ReleaseNotes/winter2018/2018-winter-filesets-withgrouping-vertexexecution.jpg) 

This also reduces the cost of the job. In the first case, the job cost about 50 seconds of execution time when submitting it with 5 AUs (or 26 seconds execution time with 15 AUs), while the improvement reduces the cost to about 35 seconds of execution time with 2 AUs.


#### Built-in Parquet Extractor and Outputter is in Public Preview

Parquet is one of the major open source structured data formats used when processing data at scale. It provides a columnar storage layout, designed for efficient reads and aggregations. 

It enables more efficient data exchange between different analytics engines (that know how query Parquet files) than CSV. 

U-SQL offers the ability to read from and write to Parquet files with a built-in extractor and outputter. Currently version 2.3.1 of Parquet is supported.

In order to enable the preview feature, add the following statement to your script:

    SET @@FeaturePreviews = "EnableParquetUdos:on";

##### `Extractors.Parquet()`

The `Parquet()` extractor converts Parquet files into rowsets. It will read the Parquet files meta data to identify the column compression schemes, and produces a rowset based on the `EXTRACT` expression’s schema. 

If the column type specified in the `EXTRACT` expression is not nullable and the Parquet file contains a `nullable` type, an error is raised during Execution.

The following table provides the type mapping between the Parquet storage type and their equivalent U-SQL types:

| Parquet Types on read | Compatible U-SQL Types | Parquet Types on output |
|-----------------------|------------------------|-------------------------|
| `BOOLEAN` | `bool(?)` |	`BOOLEAN` |
| `INT32/(U)INT_32`| `(u)int(?)` | `INT32/(U)INT_32` |
| `INT32/(U)INT_8` | `(s)byte(?)`	| `INT32/(U)INT_8` |
| `INT32/(U)INT_16` | `(u)short(?)` | `INT32/(U)INT_16` |
| `INT64/(U)INT_64` | `(u)long(?)` | `INT64/(U)INT_64` |
| `FLOAT` | `float(?)` |	`FLOAT` |
| `DOUBLE` | `double(?)` |	`DOUBLE` |
| `INT32/DATE` | `DateTime(?)` |	`INT32/DATE` [4] |
| `INT64/TIMESTAMP_MILLIS` | `DateTime(?)` | `INT64/TIMESTAMP_MILLIS` [4] |
| `INT64/TIMESTAMP_MICROS` | `DateTime(?)` | `INT64/TIMESTAMP_MICROS` [4] |
| `INT96` [3] | `DateTime(?)` | - |
| `INT32/DECIMAL` | `decimal(?)` | `INT32/DECIMAL` [6] |
| `INT64/DECIMAL`| `decimal(?)` | `INT64/DECIMAL` [6] |
| `FIXED_LEN_BYTE_ARRAY/DECIMAL`| `decimal(?)` | `FIXED_LEN_BYTE_ARRAY/DECIMAL` [6] |
| `BYTE_ARRAY/UTF8` | `string` [5] | `BYTE_ARRAY/UTF8` |
| `BYTE_ARRAY/UTF8` | `byte[]` | `BYTE_ARRAY` |
| `BYTE_ARRAY` | `string` | `BYTE_ARRAY/UTF8` |
| `BYTE_ARRAY` | `byte[]` | `BYTE_ARRAY` |
| `FIXED_LEN_BYTE_ARRAY`| `byte[]`, `string` | - |
| `INT32/TIME_MILLIS` [2] | `integer(?)` | - | 
| `INT64/TIME_MICROS` [2] | `long(?)` | - |
| `JSON` [2] | `string`, `byte[]` | - |
| `BSON` [2] | `byte[]`  | - |
| `MAP` [1] | - | - |
| `MAP_KEY_VALUE` [1] | - | - | 
| `LIST` [1] | - | - |
| `ENUM` [1] | - | - |
| `INTERVAL` [1] | - | - |
| - | `char(?)` [1] | - |	 
| - |	`Guid(?)` [1] | - |
| - | `SQL.MAP<K,V>` [1] | - |
| - | `SQL.ARRAY<T>` [1] | - |

Notes:

[1] Not supported. This may be supported in a future refresh.

[2] Not explicitly supported, but can be extracted.

[3] Truncated from nanoseconds to ticks (100 nanoseconds), deprecated.

[4] Truncated from ticks to microseconds/milliseconds/days.

[5] `BYTE_ARRAY/UTF8` is used to write `string` typed values. Any `BYTE_ARRAY` can be extracted to a `string` typed column, but if the byte sequence is not a valid UTF-8 encoded string, a runtime error is raised.

[6] Outputted with specified precision/scale, rescaled (and rounded) if needed.

If the Parquet decimal column values do not fit into the U-SQL types precision or scale, the extractor will attempt to re-scale the decimal values, if possible, or raises an error during execution otherwise.

For example, the value 
`987'654'321'123'456.789'123'456'789'150'000'000'00` (precision: 38, scale: 23) 
will be normalized (with rounding) to
`987'654'321'123'456.789'123'456'789'2` (precision: 28, scale: 13).
But the value
`987'654'321'123'456'789'123'456'789'15.0'000'000'00` (precision: 38, scale: 9)
cannot be re-scaled, since the resulting scale would have to be negative. 

_Parameters_

None.

_Limitations_

- The Parquet extractor does not support user-defined types nor the built-in complex types. 
- The Parquet extractor does not support `guid` nor `char` data types.

_Best Practice Guidance_

Currently, a Parquet file is not read in parallel by the Parquet extractor. That means that very large Parquet files may take a long time to read. Thus, we recommend that you operate on a set of Parquet files in the 250MB to 2GB range and use the `EXTRACT` file set feature to read a set of files (see Example 2).

_Examples_

1. This script reads all the numeric columns from the Parquet file that is generated by the script in [`Outputters.Parquet()`](#outputtersparquet) Example 1 and writes it into a CSV file.

        SET @@FeaturePreviews = "EnableParquetUdos:on";

        @N = 
          EXTRACT
            c_tinyint byte?,
            c_smallint short?,
            c_int int?,
            c_bigint long?,
            c_float float?,
            c_double double?,
            c_decimal_int decimal?,
            c_decimal_w_params decimal?
          FROM "/output/releasenotes/winter2017-18/parquet/typedvalues.parquet"
          USING Extractors.Parquet();

        OUTPUT @N 
        TO "/output/releasenotes/winter2017-18/parquet/typed_numeric_values.csv"
        USING Outputters.Csv(outputHeader:true);

2. This script reads all the columns from the Parquet files that were generated by the script in [`Outputters.Parquet()`](#outputtersparquet) Example 2 and writes its distinct rows into a CSV file.

        SET @@FeaturePreviews = "EnableParquetUdos:on";

        @data_req = 
          EXTRACT vehicle_id int
          , entry_id long
          , event_date DateTime
          , latitude float
          , longitude float
          , speed int
          , direction string
          , trip_id int?
          , vid int 
          , date DateTime 
          FROM "/output/releasenotes/winter2017-18/parquet/vehicles_{*}.parquet"
          USING Extractors.Parquet();

        @data_req =
          SELECT DISTINCT *
          FROM @data_req;

        OUTPUT @data_req
        TO "/output/releasenotes/winter2017-18/parquet/vehicles_all.csv"
        USING Outputters.Csv(outputHeader:true);

##### `Outputters.Parquet()`

The `Parquet()` outputter converts a rowset into a Parquet file. It provides a set of parameters to specify column compression schemes and several other parameters to transform the rowset into the file. 

_Parameters_

|Parameter Name | Parameter Type | Default |
|---------------|----------------|---------|
| `rowGroupSize` | `int` | `250` | 

The parameter `rowGroupSize` specifies the maximum size of the Parquet file's row group in Mb. The valid range is [1..1024], 256 by default. If an invalid value is specified, an error is raised.

|Parameter Name | Parameter Type | Default |
|---------------|----------------|---------|
| `rowGroupRows` | `int` | `10485760` | 

The parameter `rowGroupRows` specifies the maximum size of the Parquet file's row group in Rows. The valid range is [1..100M], 10M (10485760) by default. If an invalid value is specified, an error is raised.

|Parameter Name | Parameter Type | Default |
|---------------|----------------|---------|
| `columnOptions` | `string` | null | 

The parameter `columnOptions` specifies a precision, scale and compression for the specific columns. 
It is represented in a comma separated list of column options with the following syntax:

    ColOptions := ColOption [',' ColOption].
    ColOption := index ([':'decimalPrecision]['.'decimalScale])|[':'DateTimePrecision]['~'Compression].

where

`index` specifies the the column ordinal as a non-negative integer in the rowset to be output (starting at 0 for the first column in the rowset).

`decimalPrecision` represents the decimal precision in the range [1..28] for the U-SQL decimal typed column and will determine the Parquet data type that is used to represent it. If the value is in the range [1..9] it maps to the Parquet type `INT32`, if it is in the range [10..18] it will map to `INT64`, and if it is in the range [19..28] it will map to `FIXED_LEN_BYTE_ARRAY`. The default is `28`.

`decimalScale` represents the decimal scale in the range [1..DecimalPrecision] for the U-SQL decimal typed column used when writing it to Parquet. The default is `8` (**The current preview implementation uses the default 0. That will change in the future**).
 
`DateTimePrecision` represents the DateTime precision for the U-SQL DateTime typed column used when writing it to Parquet. The supported values of the precision are:

    DateTimePrecision := 'micro' | 'milli' | 'days'.

The default is `micro`.

`Compression` specifies the compression used to compress the specified column. The supported compression formats are:

    Compression := 'uncompressed' | 'snappy' | 'brotli' | 'gzip'.

The default is `'uncompressed'`.

If an option is specified on a column that is not of an applicable type or is specified with an invalid value, an error is raised.

**Note:** A future version will provide additional parameters, some of which may replace the `columnOptions` parameter.

_Limitations_

- The Parquet outputter does not support user-defined types nor the built-in complex types. Please convert them to `byte[]` or string if possible before outputting.
- The Parquet outputter does not support `guid` nor `char` data types. Please convert them to `string` before outputting.

_Best Practice Guidance_

Currently, a Parquet file is not written in parallel by the Parquet outputter. That means that large Parquet files may take a long time to write. Thus, we recommend that the `OUTPUT` statement generates a set of files using the `OUTPUT` file set feature. See Example 3 for the current limited support for generating multiple files. The [OUTPUT file set feature](#data-driven-output-partitioning-with-output-fileset-is-in-private-preview) in private preview can partition the rowset into many Parquet files based on column values in the rowset.

_Examples_

1. This script generates a Parquet file with default parameters of a rowset containing values of the different built-in scalar types. See [`Extractors.Parquet()`](#extractorsparquet) Example 1 for reading the file.

        SET @@FeaturePreviews = "EnableParquetUdos:on";

        @I =
          SELECT (bool?) true AS c_boolean,
                 (byte?) 1 AS c_tinyint,
                 (short?) null AS c_smallint,
                 (int?) 2147483647 AS c_int,
                 (long?) 9223372036854775807 AS c_bigint,
                 (float?) 123.345 AS c_float,
                 (double?) 234.567 AS c_double,
                 (decimal?) 346 AS c_decimal_int,
                 (decimal?) 345.67800 AS c_decimal_w_params,
                 DateTime.Parse("2015-05-10T12:15:35.1230000Z") AS c_timestamp,
                 "ala ma kota" AS c_string,
                 new byte[]{42, 43, 57} AS c_binary
          FROM (VALUES(1)) AS T(x);

        OUTPUT @I 
        TO "/output/typedvalues.parquet"
        USING Outputters.Parquet();

    Note that the current default scale for decimals is 0. If you want to set the scale of the `decimal_w_params` column to `8`, use the following `OUTPUT` statement instead:

        OUTPUT @I 
        TO "/output/releasenotes/winter2017-18/parquet/typedvalues2.parquet"
        USING Outputters.Parquet(columnOptions:"8.8");

2. This script converts the vehicle sample data that is available as part of the ADLA sample data set into Parquet files. It generates as many Parquet files as there are processing vertices feeding into the `OUTPUT` statement. In order to generate enough data to create several files, a `CROSS APPLY` makes 1000 copies of each extracted row. Note that the resulting 16 file names will look like: `vehicles_0000000.parquet` etc., and each file will be about 850kB in size in this case.

        SET @@FeaturePreviews = "EnableParquetUdos:on";

        DECLARE @ADL_DIR string = "/Samples/Data/AmbulanceData/";
        DECLARE @ADL_FILESET_REQVID string = @ADL_DIR + "vehicle{vid}_{date:MM}{date:dd}{date:yyyy}.{*}";

        @data_req = 
          EXTRACT vehicle_id int
          , entry_id long
          , event_date DateTime
          , latitude float
          , longitude float
          , speed int
          , direction string
          , trip_id int?
          , vid int // virtual file set column
          , date DateTime // virtual file set column
          FROM @ADL_FILESET_REQVID
          USING Extractors.Csv();

        // Now explode the data to generate enough data to produce more than one file
        @alldata_req =
          SELECT d.*
          FROM @data_req AS d CROSS APPLY EXPLODE(Enumerable.Range(1,1000)) AS M(factor);

        OUTPUT @alldata_req
        TO "/output/releasenotes/winter2017-18/parquet/vehicles_{*}.parquet"
        USING Outputters.Parquet();

3. This script writes some sample data into an uncompressed Parquet file, one Parquet file where only the `c_string` column is compressed with the `snappy` compression and one Parquet file where additionally the `c_bigint` column is compressed with the `brotli` compression. All three files set the scale at 8 for the `c_decimal` column.

        SET @@FeaturePreviews = "EnableParquetUdos:on";

        @I=
          SELECT *
          FROM (VALUES
                ((long?) 9223372036854775807,
                 (decimal?) 345.67800,
                 "This is a long string, so long that hopefully the compression will make a difference when writing the column. Long strings benefit from compression."),
                ((long?) 9223372036854775807,
                 (decimal?) 1234567.89,
                 "This is another long string, so long that hopefully the compression will make a difference when writing the column. Long strings benefit from compression.")       
               ) AS T(c_bigint, c_decimal, c_string);

        // write uncompressed
        OUTPUT @I 
        TO "/output/releasenotes/winter2017-18/parquet/uncompressed.parquet"
        USING Outputters.Parquet(columnOptions:"1.8");

        // write compressed string column
        OUTPUT @I 
        TO "/output/releasenotes/winter2017-18/parquet/compressed_string.snappy.parquet"
        USING Outputters.Parquet(columnOptions:"1.8,2~snappy");

        // compress string with snappy and bigint value with brotli
        OUTPUT @I 
        TO "/output/releasenotes/winter2017-18/parquet/compressed.parquet"
        USING Outputters.Parquet(columnOptions:"1.8,2~snappy,0~brotli");

This feature addresses the following user-voice feedback: [Support Parquet in Azure Data Lake](https://feedback.azure.com/forums/327234-data-lake/suggestions/11698695-support-parquet-in-azure-data-lake)

#### Automatic GZip compression on `OUTPUT` statement is in Public Preview

U-SQL automatically decompresses GZip files with the `.gz` file extension in the `EXTRACT` expression. We now added support for automatic compression of files with GZip if the `OUTPUT` statement targets files with a `.gz` file extension. 

This feature is currently in preview and can be turned on with the following statement in your script:

	SET @@FeaturePreviews = " GZipOutput:on";

If the preview is not turned on, you still will receive the error message `E_CSC_USER_FILEFORMATNOTSUPPORTED`.

_Example:_

    SET @@FeaturePreviews = "GZipOutput:on";

    @data =
      SELECT *
      FROM (VALUES (1,2),(3,4)) AS T(i, j);

    OUTPUT @data
    TO "/output/data.csv.gz"
    USING Outputters.Csv(outputHeader:true);

#### Built-in ORC Extractor and Outputter is in Private Preview

We have started a private preview of a built-in ORC extractor and outputter. Please [contact us](mailto:usql@microsoft.com) if you want to try it out and provide us with your feedback.

This feature addresses the following user-voice feedback: [Read from and write to Optimized Row Columnar (ORC) format](https://feedback.azure.com/forums/327234-data-lake/suggestions/10701201-read-from-and-write-to-optimized-row-columnar-orc)

#### Data-driven Output Partitioning with `OUTPUT` fileset is in Private Preview

We have started a private preview of the data-driven output partitioning with filesets. It will give you the ability to use column values to create parts of a file path and partition the remaining data into different files based on these values. 

This feature addresses the following user-voice feedback: [Support 'dynamic' output file names in ADLA](https://feedback.azure.com/forums/327234-data-lake/suggestions/10550388-support-dynamic-output-file-names-in-adla)

Please [contact us](mailto:usql@microsoft.com) if you want to try it out and provide us with your feedback.

#### A limited flexible-schema feature for U-SQL table-valued function parameters is now available for preview (requires opt-in)

This feature allows to write more generic U-SQL table-valued functions and procedures, where only part of the schema of a table parameter needs to be present.

Please [contact us](mailto:usql@microsoft.com) if you want to try it out and provide us with your feedback.

## New U-SQL capabilities

#### U-SQL adds job information system variable `@@JOBINFO`

Often you want to use job related information such as who submitted the job inside your U-SQL script. In the current refresh U-SQL adds a structured system variable `@@JOBINFO` that provides a variety of job related information. The following parts are supported:

| Job Info Property | Return Type | Description |
|-------------------|-------------|------------------|
| `@@JobInfo.Id`      | `Guid?`       | The job's id typed as a nullable Guid. |
| `@@JobInfo.Name`    | `string`      | The job's name as set by the job submitter. |
| `@@JobInfo.Priority` | `int?`       | The job's priority as set by the job submitter. |
| `@@JobInfo.Allocation` | `int?`     | The job's analytic unit (AU) allocation as set by the job submitter. |
| `@@JobInfo.Account.Name` | `string`       | The name of the Azure Data Lake Analytics account against which the job got submitted. |
| `@@JobInfo.Submit.Alias` | `string`       | The alias of the job submitter. |
| `@@JobInfo.Submit.Date` | `DateTime?` | The timestamp when the job was submitted (in UTC-7 timezone). |

_Limitations and Notes:_ 

1. As of time of publishing this release note, `@@JobInfo.Submit.Date` only returns `null`.
2. In a future release, we may rename some of the property names to align with the Azure SDK naming. The current properties will continue to be available.
3. The `@@JobInfo` properties are currently _not_ available in the local run environment. They will be added in a future release.

_Examples:_

1. The following script collects the job information when the script gets executed and outputs the result into a CSV file with header information. Note that since both `@@JobInfo.Name` and `@@JobInfo.Account.Name` result in the same column name `Name`, we have to disambiguate the columns with an explicit alias.

        @data =
          SELECT @@JobInfo.Id, 
                 @@JobInfo.Name AS JobName, 
                 @@JobInfo.Priority, 
                 @@JobInfo.Allocation, 
                 @@JobInfo.Account.Name AS AccountName, 
                 @@JobInfo.Submit.Alias, 
                 @@JobInfo.Submit.Date 
          FROM(VALUES(1)) AS T(x);

        OUTPUT @data
        TO "/output/releasenotes/winter2018/jobinfo.csv"
        USING Outputters.Csv(outputHeader : true);

2. The following script mimics the use of a local user folder based on who submitted the script:

        DECLARE CONST @output = 
          "/output/releasenotes/winter2018/" + @@JobInfo.Submit.Alias.Substring(0, @@JobInfo.Submit.Alias.IndexOf('@')) + "/result.csv";

        @data = 
          SELECT data 
          FROM (VALUES ("This data is inserted into a folder based on the alias who submitted the job")) AS T(data);

        OUTPUT @data
        TO @output
        USING Outputters.Csv();

#### U-SQL adds support for computed file property columns on `EXTRACT`

At times you would like to get information about the files that you process, such as their full URI path or information about their size, creation or modification dates. And sometimes you want to use that information to filter the set of files that you want to process.

Starting with this refresh, U-SQL adds support for computed file property columns on `EXTRACT` expressions. It provides the following capabilities:

1. Provide built-in file property functions that expose specific file properties.
2. Provide a way to assign the properties to "virtual" columns in the `EXTRACT`'s schema. This is done using a "calculated column" syntax.
3. Allow constant-foldable comparisons on these "virtual" columns in subsequent query predicates to limit the files being processed by the `EXTRACT` expression.

U-SQL provides the following file property functions in the `EXTRACT` expression's schema:

| File Property Function | Return type | Description |
|------------------------|-------------|-------------|
| `FILE.URI()`           | `string` | Returns the file's absolute URI or the full local path if used in a local run. |
| `FILE.MODIFIED()` | `DateTime?` | Returns the file's last modification time stamp in UTC-0 timezone. |
| `FILE.CREATED()` | `DateTime?` | Returns the file's creation time stamp in UTC-0 timezone. |
| `FILE.LENGTH()` | `long?` | Returns the file's size in bytes. | 

The `EXTRACT` expression's schema definition needs to contain at least one non virtual column. 

_Note:_ These functions are modeled after the file intrinsic functions, but are taking implicit context from the `EXTRACT` expression's file set.

_Limitations:_

1. Only `FILE.URI()` is currently supported in local run. The other intrinsics will be added in the next refresh of the tooling.

_Examples:_

1. The following script selects all columns of the `vehicle` files in the U-SQL sample data set as well as their file properties.

        @data = 
          EXTRACT 
            vehicle_id int
          , entry_id long
          , event_date DateTime
          , latitude float
          , longitude float
          , speed int
          , direction string
          , trip_id int?
          , uri = FILE.URI()
          , modified_date = FILE.MODIFIED()
          , created_date = FILE.CREATED()
          , file_sz = FILE.LENGTH()
          FROM "/Samples/Data/AmbulanceData/vehicle{*}"
          USING Extractors.Csv();

        OUTPUT @data
        TO "/output/releasenotes/winter2017-18/fileprops.csv"
        USING Outputters.Csv(outputHeader : true);

2. The following script selects all columns of the `vehicle` files in the U-SQL sample data set as well as the properties of all the files in the file set for files that are larger than 1590000 bytes.

        @data = 
          EXTRACT 
            vehicle_id int
          , entry_id long
          , event_date DateTime
          , latitude float
          , longitude float
          , speed int
          , direction string
          , trip_id int?
          , uri = FILE.URI()
          , modified_date = FILE.MODIFIED()
          , created_date = FILE.CREATED()
          , file_sz = FILE.LENGTH()
          FROM "/Samples/Data/AmbulanceData/vehicle{*}"
          USING Extractors.Csv();

        @res =
          SELECT *
          FROM @data
          WHERE file_sz > 1590000;

        OUTPUT @res
        TO "/output/releasenotes/winter2017-18/fileprops-filtered.csv"
        USING Outputters.Csv(outputHeader : true);

#### DiagnosticStream support in .Net User-code

The U-SQL .NET extension model added a new object called `Microsoft.Analytics.Diagnostics.DiagnosticStream` that allows to record (limited) diagnostic information into a file in the job folder. 

The .NET user code can be written using the `DiagnosticStream` object and the actual generation of the diagnostic file can be controlled by setting the system variable `@@Diagnostics" in the U-SQL script using the user code. Its default setting is turned off. The following script statement will turn it on:

    SET @@Diagnostics = true;

_Generated file:_

Each job will receive its own file in the job folder in ADLS at

`/system/jobservice/jobs/Usql/{yyyy}/{MM}/{dd}/{HH}/{mm}/{job-guid}/diagnosticstreams/diagnostic.xml`

The VisualStudio ADL Tool has a link to the folder containing the diagnostic stream's folder.

The file contains the following information (`...` indicates repetition) as an XML fragment in UTF-8 encoding (meaning it can have no XML declaration and can have more than one top-level XML element):

    <Vertex name="vertexname" v="vertexversion" guid="vertex-guid-withoutbraces">
      <l>entitized-user-provided-content</l> ...
    </Vertex> ...

Note that each Vertex element appears in its own line and there are no CR/LF or additional whitespaces in its content (they are added in the example above for better visualization only).

The diagnosis has the following limits:

1.	Each `entitized-user-provided-content` cannot be larger than 100kb of UTF-16 encoded, pre-entitized data (e.g., the user writes `A\rB` which will be entitized to `A&xD;B`. This will be counted as 3 bytes).
2.	The overall diagnosis file size limit is 5GB. This includes all the XML tags and attribute information.

In the case that the diagnostic information exceeds the limits above, writing the diagnosis information should not fail. Therefore, the following information will be added as an indication that the data got truncated:

1.	Each Line that is being written that ends up being too long will be represented by the following element:

        <l truncated="true">first-100kb-of-user-provided-content-in-entitizedform<l>

2.	If the file gets over 5GB in size, one special Vertex element will be appended at the end of the file of the following form:

        <Vertex truncated="true"/>

Defaulted information:

We may extend this format in the future. Therefore, there is an implied version attribute on each of the Vertex elements that is implied to be version="1.0".  Future versions will update the minor version, if backwards-compatible changes are done to the XML format; and they will update the major version, if the changes are not backwards-compatible.

Timing of writing information to the file:

In order to strike a balance between seeing the progress too late and writing too often and writing invalid XML fragments, the vertex information is written at the end of a vertex execution and reports both successful and failed vertices.

Each time the content of the file becomes visible, the file is considered to be sealed.

Handling multiple vertex executions and failed vertices:

ADLA executes a vertex possibly more than once for reasons such as

1.	A vertex failed and the system decides that a retry may succeed.
2.	The system may decide to duplicate an execution of a vertex and pick the fasted to succeed.

Regardless of the reason, each vertex execution gets its own version number. 

A vertex may also fail due to system error or user error.

The following list indicates which vertex version’s data is being written into the diagnostic file:

1.	A vertex succeeds once: The vertex data is written with the vertex’ version number.
2.	A vertex succeeds several times (e.g., they were submitted concurrently and both succeeded): The information for both vertices is written. The vertices have different version numbers. The highest version number is the one that is the one taken as the input for the next stage.
3.	A retried vertex after a failed vertex: Both the retried and the failed vertex information is written. The highest version information is the one considered as the input for the next stage. The element representing the failed vertex gets an additional attribute `failed="true"`.
4.	A vertex fails due to user-error: The last failed vertex is written with the content. The element representing the failed vertex gets an additional attribute `failed="true"`.
5.	A vertex fails due to system-error: The last failed vertex is written with the content, as long as the error is catch-able (e.g., a store read error). The element representing the failed vertex gets an additional attribute `failed="true"`.
6.	A vertex fails due to a system crash: No diagnostics is written. This condition should be reported as a system issue.
7.	A vertex is discarded by the job manager before it finishes: No diagnostics is written.

**Note: 4. and 5. are currently not recording diagnostics. The above specification will be implemented in a future refresh**

Diagnostic File Expiration:

Since the file is written into the job folder, its expiration is subject to the job data expiration. The user can either increase the expiration for the job, or copy the file into a separate location.

_Methods on the `DiagnosticStream` class_

    void DiagnosticStream.WriteLine(string user_provided_content)

Takes the `user_provided_content` and writes up to the first 100kb of the provided data (size determined based on UTF-16 encoding) into the diagnose stream into an `l` element after entitizing CR and LF using XML character code entities and entitizing < and & with the named XML character entities. If the data is more than 100kb an additional attribute `truncated="true"` will be added to the `l` element. Note that the data in the file will be UTF-8 encoded.

Example:

    DiagnosticStream.WriteLine("This is an example with an &");

Adds the following line to the vertex’ element content:

    <l>This is an example with an &amp;</l>

_System Variable controls Diagnostic Output_ 

Often diagnostics may only be used under certain conditions (debug executions etc) and otherwise can lead to negative performance impact, even if the user wraps it with an if statement. 

Therefore, U-SQL provides a system variable `@@Diagnostics` to control if the DiagnosticStream code gets invoked or not.

The two acceptable values are `true` and `false`. The default is `false`, which means that the DiagnosticStream code does not get invoked. The following script statement will turn it on:

    SET @@Diagnostics = true;

The system variable can only be set once per script invocation. If a script contains more than one statement setting it, a compile time error is raised.


_Example:_

Given the file `/Samples/Data/SearchLogWithHeaderandWronglines.tsv` with the following content:

    id	date	market	search_string	time	found_uris	visited_uris
    399266	2/15/2012 11:53:16 AM	en-us	how to make nachos	73	www.nachos.com;www.wikipedia.com	NULL
    382045	2/15/2012 11:53:18 AM	en-gb	best ski resorts	614	skiresorts.com;ski-europe.com;www.travelersdigest.com/ski_resorts.htm	ski-europe.com;www.travelersdigest.com/ski_resorts.htm
    382045	2/16/2012 11:53:20 AM	en-gb	broken leg	74	mayoclinic.com/health;webmd.com/a-to-z-guides;mybrokenleg.com;wikipedia.com/Bone_fracture	mayoclinic.com/health;webmd.com/a-to-z-guides;mybrokenleg.com;wikipedia.com/Bone_fracture
    106479	2/16/2012 11:53:50 AM	en-ca	south park episodes	24	southparkstudios.com;wikipedia.org/wiki/Sout_Park;imdb.com/title/tt0121955;simon.com/mall	southparkstudios.com
    906441	2/16/2012 11:54:01 AM	en-us	cosmos	1213	cosmos.com;wikipedia.org/wiki/Cosmos:_A_Personal_Voyage;hulu.com/cosmos	NULL
    351530	2/16/2012 11:54:01 AM	en-fr	microsoft	241	microsoft.com;wikipedia.org/wiki/Microsoft;xbox.com	NULL
    640806	2/16/2012 11:54:02 AM	en-us	wireless headphones	502	www.amazon.com;reviews.cnet.com/wireless-headphones;store.apple.com	www.amazon.com;store.apple.com
    304305	2/16/2012 11:54:03 AM	en-us	dominos pizza	60	dominos.com;wikipedia.org/wiki/Domino's_Pizza;facebook.com/dominos	dominos.com
    460748	2/16/2012 11:54:04 AM	en-us	yelp	1270	yelp.com;apple.com/us/app/yelp;wikipedia.org/wiki/Yelp,_Inc.;facebook.com/yelp	yelp.com
    354841	2/16/2012 11:59:01 AM	en-us	how to run	610	running.about.com;ehow.com;go.com	running.about.com;ehow.com
    
    354068	2/16/2012 12:00:33 PM	en-mx	what is sql	422	wikipedia.org/wiki/SQL;sqlcourse.com/intro.html;wikipedia.org/wiki/Microsoft_SQL	wikipedia.org/wiki/SQL
    674364	2/16/2012 12:00:55 PM	en-us	mexican food redmond	283	eltoreador.com;yelp.com/c/redmond-wa/mexican;agaverest.com	NULL
    347413	2/16/2012 12:11:55 PM	en-gr	microsoft	305	microsoft.com;wikipedia.org/wiki/Microsoft;xbox.com	NULL
    848434	2/16/2012 12:12:35 PM	en-ch	facebook	10	facebook.com;facebook.com/login;wikipedia.org/wiki/Facebook	facebook.com
    604846	2/16/2012 12:13:55 PM	en-us	wikipedia	612	wikipedia.org;en.wikipedia.org;en.wikipedia.org/wiki/Wikipedia	wikipedia.org
    840614	2/16/2012 12:13:56 PM	en-us	xbox	1220	xbox.com;en.wikipedia.org/wiki/Xbox;xbox.com/xbox360	xbox.com/xbox360
    656666	2/16/2012 12:15:55 PM	en-us	hotmail	691	hotmail.com;login.live.com;msn.com;en.wikipedia.org/wiki/Hotmail	NULL
    951513	2/16/2012 12:17:00 PM	en-us	pokemon	63	pokemon.com;pokemon.com/us;serebii.net	pokemon.com
    350350	2/16/2012 12:18:17 PM	en-us	wolfram	30	wolframalpha.com;wolfram.com;mathworld.wolfram.com;en.wikipedia.org/wiki/Stephen_Wolfram	NULL
    corrupted id	2/16/2012 12:18:17 PM	en-us	wolfram	30	wolframalpha.com;wolfram.com;mathworld.wolfram.com;en.wikipedia.org/wiki/Stephen_Wolfram	NULL
    641615	2/16/2012 12:19:55 PM	en-us	kahn	119	khanacademy.org;en.wikipedia.org/wiki/Khan_(title);answers.com/topic/genghis-khan;en.wikipedia.org/wiki/Khan_(name)	khanacademy.org
    321065	2/16/2012 12:20:03 PM	en-us	clothes	732	gap.com;overstock.com;forever21.com;footballfanatics.com/college_washington_state_cougars	footballfanatics.com/college_washington_state_cougars
    651777	2/16/2012 12:20:33 PM	en-us	food recipes	183	allrecipes.com;foodnetwork.com;simplyrecipes.com	foodnetwork.com
    666352	2/16/2012 12:21:03 PM	en-us	weight loss	630	en.wikipedia.org/wiki/Weight_loss;webmd.com/diet;exercise.about.com	webmd.com/diet

and the U-SQL script

    SET @@FeaturePreviews = "DIAGNOSTICS:ON";
    @data =
      EXTRACT [id] int,
              [date] DateTime,
              [market] string,
              [searchstring] string,
              [time] int,
              [found_urls] string,
              [visited_urls] string
      FROM "/Samples/Data/SearchLogWithHeaderandWronglines.tsv"
      USING new USQLApplication1.MyExtractor();

    OUTPUT @data
    TO "/output/searchlog.tsv"
    USING Outputters.Tsv(outputHeader:true, quoting: false);
 
with the following custom extractor (in code-behind):

    using Microsoft.Analytics.Interfaces;
    using Microsoft.Analytics.Diagnostics;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    namespace USQLApplication1
    {        
        [SqlUserDefinedExtractor]
        public class MyExtractor : IExtractor
        {
            public override IEnumerable<IRow> Extract(IUnstructuredReader input, IUpdatableRow outputrow)
            {
                ulong rowId = 0;
                char column_delimiter = '\t';
                string lastLine = string.Empty;
                string line;
                foreach (var stream in input.Split(new byte[] { 0x0d, 0x0a }))
                {
                    var reader = new StreamReader(stream, encoding: Encoding.UTF8);
                    line = reader.ReadToEnd();

                    if (string.IsNullOrEmpty(line))
                    {
                        DiagnosticStream.WriteLine(String.Format("Skip empty line at line {0}.", rowId));
                    }
                    else if (line.StartsWith("id\t"))
                    {
                        DiagnosticStream.WriteLine(String.Format("Skip header line at line {0}.", rowId));
                    }
                    else
                    {
                        try
                        {
                            var tokens = line.Split(column_delimiter);
                            outputrow.Set<int>("id", int.Parse(tokens[0]));
                            outputrow.Set<DateTime>("date", DateTime.Parse(tokens[1]));
                            outputrow.Set<string>("market", tokens[2]);
                            outputrow.Set<string>("searchstring", tokens[3]);
                            outputrow.Set<int>("time", int.Parse(tokens[4]));
                            outputrow.Set<string>("found_urls", tokens[5]);
                            outputrow.Set<string>("visited_urls", tokens[6]);
                        }
                        catch
                        {
                            DiagnosticStream.WriteLine("============\t <afds />====\n==============\xc4");
                            DiagnosticStream.WriteLine(String.Format("Last  line {0}: {1}", rowId - 1, lastLine));
                            DiagnosticStream.WriteLine(String.Format("Error line {0}: {1}", rowId, line));
                            DiagnosticStream.WriteLine("==============================");
                        }

                        yield return outputrow.AsReadOnly();
                    }

                    lastLine = line;
                    ++rowId;
                }
            }
        }
    }

will produce the following file in the job folder at `/system/jobservice/jobs/Usql/yyyy/MM/dd/HH/mm/jobguid/diagnosticstreams/diagnosticstream.xml` in your account (where the `yyyy/MM/dd/HH/mm/jobguid` path identifies the job's time and GUID):

    <Vertex name="SV1_Extract[0][0]" v="0" guid="{DD9A2150-594D-45F2-8851-56DE40C2732A}" truncated="false"><l>Skip header line at line 0.</l><l>Skip empty line at line 11.</l><l>============	 &lt;afds /&gt;====&#x0a;==============Ä</l><l>Last  line 20: 350350	2/16/2012 12:18:17 PM	en-us	wolfram	30	wolframalpha.com;wolfram.com;mathworld.wolfram.com;en.wikipedia.org/wiki/Stephen_Wolfram	NULL</l><l>Error line 21: corrupted id	2/16/2012 12:18:17 PM	en-us	wolfram	30	wolframalpha.com;wolfram.com;mathworld.wolfram.com;en.wikipedia.org/wiki/Stephen_Wolfram	NULL</l><l>==============================</l></Vertex>

#### Built-in Text/Csv/Tsv Extractors and Outputters support ANSI/Windows 8-bit codepage encodings

U-SQL's built-in text format extractors and Outputters for CSV-like data added support to both extract from and output to files that are encoded with any of the following additional 8-bit codepages (besides the currently supported Unicode and ASCII encodings):

| Encoding number | Encoding name | Description |
|-----------------|---------------|-------------|
| 1250 | Windows-1250 | Central European (Windows) |
| 1251 | Windows-1251 | Cyrillic (Windows) |
| 1252 | Windows-1252 | Western European (Windows) |
| 1253 | Windows-1253 | Greek (Windows) |
| 1254 | Windows-1254 | Turkish (Windows) |
| 1255 | Windows-1255 | Hebrew (Windows) |
| 1256 | Windows-1256 | Arabic (Windows) |
| 1257 | Windows-1257 | Baltic (Windows) |
| 1258 | Windows-1258 | Vietnamese (Windows) |

In order to specify them, an encoding object has to be created using the C# `System.Text.Encoding.GetEncoding` expression. It takes either one of the above encoding numbers or the encoding name (with upper- or lower-case `W`) as argument. In the case of the outputters, if the rowset contains a value that is not able to be represented by the specified encoding, then the character will be replaced by `?` (hex 3F).

_Examples_

1. The following script creates a file in Windows-1253 encoding

        DECLARE @encoding = System.Text.Encoding.GetEncoding(1253);
        DECLARE @output = "/output/releasenotes/winter2018/encoding/res_1253.csv";

        OUTPUT
        (
            SELECT "Some sample text with Α α Β β θ £ ¥ Ä ä æ ç ö" AS c
            FROM (VALUES(1)) AS T(x)
        )
        TO @output
        USING Outputters.Csv(encoding:@encoding);

2. The following script reads the above file in Windows-1253 encoding and will write it out in Windows-1252

        @data = 
          EXTRACT text string 
          FROM "/output/releasenotes/winter2018/encoding/res_1253.csv" 
          USING Extractors.Csv(encoding:System.Text.Encoding.GetEncoding("Windows-1253"));
    
        OUTPUT @data
        TO "/output/releasenotes/winter2018/encoding/extracted_1253-1252.csv" 
        USING Outputters.Csv(encoding:System.Text.Encoding.GetEncoding(1252));

#### U-SQL supports ANSI SQL `CASE` expression

While U-SQL supports the C# ternary comparison expression `cond? true_expr : false_expr` that can be used to build up case expressions, a lot of customers requested to have the SQL's `CASE` expression supported as well. 

U-SQL now supports the following `CASE` expression in the context of U-SQL's scalar expression language:

_Syntax_

    CASE_Expression := Simple_CASE_Expression | Searched_CASE_Expression.

    Simple_CASE_Expression :=
      'CASE' input_expression 
      'WHEN' when_expression 'THEN' result_expression
      { 'WHEN' when_expression 'THEN' result_expression }
      [ 'ELSE' result_expression ]
      'END'.

    Searched_CASE_Expression :=
      'CASE' 
      'WHEN' boolean_expression 'THEN' result_expression
      { 'WHEN' boolean_expression 'THEN' result_expression }
      [ 'ELSE' result_expression ]
      'END'.
     
_Semantics_

The `CASE` expression evaluates a list of conditions and returns one of multiple possible result expressions. 
It has two formats: 

1. The simple `CASE` expression compares an expression to a set of at least one simple expression to determine the result. 
2. The searched `CASE` expression evaluates a set of at least one Boolean expressions to determine the result. 

Both formats support an `ELSE` clause that is optional if the result type is nullable and required if the result type is not nullable. 

The result of the `CASE` expression is the first match in lexical order or null if no match can be found.

The `CASE` expression can be used in any U-SQL expression or clause that allows a valid scalar expression, but cannot be used inside C# code blocks.

* `input_expression`

    The `input_expression` in the `Simple_CASE_Expression` specifies the U-SQL scalar expression that will be compared against the values specified in the `WHEN` clauses.

* `when_expression`

    The `when_expression` in the `Simple_CASE_Expression` specifies the U-SQL scalar expression that will be compared against the value specified by the `input_expression`. The first `when_expression` in lexical order that evaluates to true will trigger the execution of the `result_expression` in the associated `THEN` clause. All `when_expression`s in a `CASE` expression at the same nesting level have to be of the same result type and be of the same type as the `input_expression` or a compilation error is raised.

* `boolean_expression`

    The `boolean_expression` in the `Searched_CASE_Expression`'s `WHEN` clause specifies a condition as a U-SQL Boolean expression. If the expression is not resulting in a Boolean data type, a compile time error is raised. The first  `WHEN` clause in lexical order for which the `boolean_expression`  evaluates to true will trigger the execution of the `result_expression` in the associated `THEN` clause. 

* `result_expression`

    The `result_expression` specifies the expression that will be the result of the `CASE` expression if the associated condition evaluates to true. All `result_expression`s in a `CASE` expression at the same nesting level have to be of the same result type or a compilation error is raised.

_Examples_

1. Translate some of the US state abbreviations into their long forms using a simple `CASE` expression:

        @input = 
          SELECT state_abbreviation 
          FROM (VALUES ("WA"), ("VA"), ("CA"), ("IA"), ("AK"), ("HI"), ("OR"), ("OK"), ("AZ"), ("DC")) AS s(state_abbreviation);

        @result =
          SELECT state_abbreviation,
                 CASE state_abbreviation
                 WHEN "AK" THEN "Alaska"
                 WHEN "AZ" THEN "Arizona"
                 WHEN "CA" THEN "California"
                 WHEN "HI" THEN "Hawaii"
                 WHEN "IA" THEN "Iowa"
                 WHEN "ID" THEN "Idaho"
                 WHEN "OK" THEN "Oklahoma"
                 WHEN "OR" THEN "Oregon"
                 WHEN "WA" THEN "Washington"
                 WHEN "VA" THEN "Virginia"
                 END AS state
        FROM @input;

        OUTPUT @result
        TO "/output/releasenotes/winter2018/simplecase.csv"
        USING Outputters.Csv(outputHeader : true);

2. Check if a temperature value falls into a certain range and provide a string classification. Note that in case of overlapping ranges, the first lexical match will be chosen:

        @input =
          SELECT temp_K
          FROM( VALUES(43),(300),(285),(1000)) AS t(temp_K);

        @result = 
          SELECT temp_K,
                 CASE
                 WHEN(double) temp_K BETWEEN 0.0 AND 273.15 THEN "freezing"
                 WHEN(double) temp_K BETWEEN 273.15 AND 285.0 THEN "cold"
                 WHEN(double) temp_K BETWEEN 284.0 AND 310.0 THEN "warm"
                 ELSE "too hot"
                 END AS temperature_class
          FROM @input;

        OUTPUT @result
        TO "/output/releasenotes/winter2018/searchcase.csv"
        USING Outputters.Csv(outputHeader : true);

#### U-SQL adds C# `Func<>`-typed variables in `DECLARE` statements (named lambdas)

U-SQL variables are not variables in the traditional sense but in the sense of assigning names to expressions like in functional languages. However, the U-SQL variables for scalar expressions did not allow parameterization of the expressions until now. Instead it was necessary to create an assembly to provide simple functional abstractions. 

U-SQL has now added C# function variables that provide parameterization of C# expressions and a more light-weight approach of adding functional abstractions to your U-SQL script. Also note that these function variables (or "named lambdas") can be shared with others via the use of U-SQL packages.

_Syntax_

    Scalar_Variable_Assignment :=
       `DECLARE` User_Variable [Variable_Type] = csharp_expression.

    Variable_Type := Built_in_Type | dotnet_type.

_Semantics_

* `User_Variable` specifies the name of the variable. User variables start with a single `@`-sign.

* `Variable_Type` is optional and specifies the variable's type. It can be any valid U-SQL built-in type or .Net type, including function types (`Func<>`) and user defined types. If it is not specified, the type is inferred from the expression.

* `csharp_expression` is any valid C# expression. If the variable type is a function type, it can be a lambda expression or a lambda expression containing a C# block. If the expression's result type is not consistent with the specified variable type, a compile time error is raised. 

_Limitations_

1. A variable function inside a U-SQL package can currently not refer to another variable.

_Examples_

1. The following script uses the function variable to solve the Tweet Analysis sample:

        DECLARE @get_mentions Func<string,SqlArray<string>> = 
                (tweet) => new SqlArray<string>(
                               tweet.Split(new char[]{' ', ',', '.', ':', '!', ';', '"', '“'}).Where(x => x.StartsWith("@"))
                           );
        @t = 
          EXTRACT date string
                , time string
                , author string
                , tweet string
          FROM "/Samples/Data/Tweets/MikeDoesBigDataTweets.csv"
          USING Extractors.Csv();

        // Extract mentions
        @m = SELECT @get_mentions(tweet) AS mentions FROM @t;

        @m = 
          SELECT m.Substring(1) AS m
               , "mention" AS category
          FROM @m CROSS APPLY EXPLODE(mentions) AS t(m)
          WHERE m != "@";

        // Count mentions 
        @res = 
          SELECT m.ToLowerInvariant() AS mention
               , COUNT( * ) AS mentioncount
          FROM @m
          GROUP BY m.ToLowerInvariant();

        OUTPUT @res
        TO "/output/releasenotes/winter2017-18/MyTwitterAnalysis.csv"
        ORDER BY mentioncount DESC
        USING Outputters.Csv(outputHeader:true);

2. The following scripts show how the function variable can consist of more complex C# expressions, can invoke each other and can be part of U-SQL packages. In particular, the example shows how to package the TryParse expression into a single function.

    The first script defines the package with the function variables `@EnumerateToFloor` and `@TryParseDateTime`:

        DROP PACKAGE IF EXISTS MyFunctions;

        CREATE PACKAGE MyFunctions () AS
        BEGIN
          EXPORT @EnumerateToFloor Func<int,double,IEnumerable<int>> = 
                 (x, y) => Enumerable.Range(x, (int) Math.Floor(y));

          EXPORT @TryParseDateTime Func<string, DateTime?> = 
                 (d) => {
                          DateTime dt; 
                          var b = DateTime.TryParse(d, out dt); 
                          return b ? (DateTime?) dt : (DateTime?) null;
                        };
        END;

    Then the following script imports the package and uses the two functions:

        IMPORT PACKAGE MyFunctions() AS myfns;

        @data1 = 
          SELECT * 
          FROM (VALUES(1, (float)456, (float)5.2)) AS T(id, revenueamount, BillingFrequency);

        @res1 = 
          SELECT *
          FROM @data1 CROSS APPLY EXPLODE (myfns.@EnumerateToFloor(1, BillingFrequency)) AS q(Quartile);

        OUTPUT @res1
        TO "/output/releasenotes/winter2017-18/quartile.csv"
        USING Outputters.Csv(outputHeader : true);

        @data2 =
          SELECT myfns.@TryParseDateTime(c_date_str) AS c_date
          FROM (VALUES
                ("2018-01-01T01:02:03"),
                ("12/13/2017"),
                ("invalid")
               ) AS T(c_date_str);

        OUTPUT @data2
        TO "/output/releasenotes/winter2017-18/tryparse.csv"
        USING Outputters.Csv(outputHeader : true);

#### U-SQL adds temporary, script-bound meta data objects with `DECLARE` statements

U-SQL provides an extensive meta data catalog that allows you to create, manage and share database objects such as tables, functions, views, code assemblies etc. The objects that are created in the meta data catalog have a life span beyond the script where they have been created and need to be explicitly dropped.

In some circumstances, this long term creation is an overhead that is not needed. For example, you may just want to refactor some code into a table-valued function for re-use in your script without wanting to register it and share it with others. To help in this situation, U-SQL adds the ability to create temporary, script-bound meta data objects.

Instead of using a `CREATE` DDL statement to persist the objects, you use a `DECLARE` DDL statement (similar to declaring a variable), but you identify the object type. As in `DECLARE` statements for variables, you use a variable name for these temporary objects, e.g., `@myfunction`.

The following script-bound objects can be created with a `DECLARE` statement. Additional objects may be added in future releases.

_Syntax_

    DECLARE_Script_Bound_Object :=
      DECLARE_Script_Bound_Type
    | DECLARE_Script_Bound_Function
    | DECLARE_Script_Bound_Procedure
    | DECLARE_Script_Bound_View.

    DECLARE_Script_Bound_Type := 
      'DECLARE' 'TYPE' Local_DDL_Variable 'AS' Anonymous_Table_Type.

    DECLARE_Script_Bound_Function :=
      'DECLARE' 'FUNCTION' Local_DDL_Variable TVF_Signature  
      ['AS'] 'BEGIN'  
        TVF_Statement_List   
      'END'.

    DECLARE_Script_Bound_Procedure :=
      'DECLARE' 'PROCEDURE' Local_DDL_Variable '(' [Parameter_List] ')'  
      ['AS']  
      'BEGIN'  
        Proc_Statement_List  
      'END'.

    DECLARE_Script_Bound_View := 
      'DECLARE' 'VIEW' Local_DDL_Variable 'AS' Query_Expression.

    Local_DDL_Variable := User_Variable.

_Semantics_

The `DECLARE_Script_Bound_Object` statements will create the specified object type in the local context of the script. The objects are syntactically visible from their point of specification and will live for the duration of the script only. The provided name has to be non-conflicting with any other script-bound object names or variable names or an error is raised.

For the object specific semantics, please refer to the documentation of the equivalent `CREATE` statements.

The objects cannot be dropped nor altered, nor do they appear in the catalog views.

You refer to the script-bound objects in the same context as their catalog-bound equivalents using their variable names.

_Example_

The following statement uses a locally declared TVF to wrap the EXTRACT expression on the sample search log data and invokes it:

    DECLARE FUNCTION @sl() RETURNS @searchlog AS
    BEGIN
      @searchlog =
        EXTRACT UserId int,
                Start DateTime,
                Region string,
                Query string,
                Duration int?,
                Urls string,
                ClickedUrls string
        FROM "/Samples/Data/SearchLog.tsv"
        USING Extractors.Tsv();
    END;

    OUTPUT @sl()
    TO "/output/releasenotes/winter2017-18/extract-order-by.csv"
    USING Outputters.Csv(outputHeader:true);

#### The `ORDER BY FETCH` clause can be used with all query expressions 

The `ORDER BY FETCH` clause can now be used on any query expression and not just `SELECT`. For the detailed syntax [see the documentation](https://msdn.microsoft.com/en-us/azure/data-lake-analytics/u-sql/query-statements-and-expressions-u-sql#qry_exp_fetch).

_Examples_

1. The following statement extracts from the search log sample data and takes the 10 rows with the longest `Duration`:

        @searchlog =
          EXTRACT UserId          int,
                  Start           DateTime,
                  Region          string,
                  Query           string,
                  Duration        int?,
                  Urls            string,
                  ClickedUrls     string
          FROM "/Samples/Data/SearchLog.tsv"
          USING Extractors.Tsv()
          ORDER BY Duration DESC FETCH 10;

        OUTPUT @searchlog
        TO "/output/releasenotes/winter2017-18/extract-order-by.csv"
        USING Outputters.Csv(outputHeader:true);

2. The following statement uses a locally declared TVF to wrap the EXTRACT expression in Example 1 and only returns the 5 rows with the longest `Duration`:

        DECLARE FUNCTION @sl() RETURNS @searchlog AS
        BEGIN
          @searchlog =
            EXTRACT UserId int,
                    Start DateTime,
                    Region string,
                    Query string,
                    Duration int?,
                    Urls string,
                    ClickedUrls string
            FROM "/Samples/Data/SearchLog.tsv"
            USING Extractors.Tsv();
        END;

        @searchlog = @sl() ORDER BY Duration DESC FETCH 5;

        OUTPUT @searchlog
        TO "/output/releasenotes/winter2017-18/extract-order-by.csv"
        USING Outputters.Csv(outputHeader:true);

#### The `EXTRACT`, `REDUCE` and `COMBINE` expressions now support a `SORTED BY` assertion

U-SQL's query optimizer can take advantage of the knowledge if the output of a user-defined extractor, reducer, or combiner is sorted. In order to assert that the result of such a UDO is sorted, the `EXTRACT`, `REDUCE` and `COMBINE` expressions now support an optional `SORTED BY` assertion for user-defined operators. 

_Syntax_

    Extract_Expression :=                                                                                    
      'EXTRACT' Column_Definition_List
      Extract_From_Clause
      [SortedBy_Clause]
      USING_Clause.

    Combine_Expression :=                                                                                    
      'COMBINE' Combine_Input                            
      'WITH' Combine_Input Join_On_Clause
      Produce_Clause                                 
      [SortedBy_Clause]
      [Readonly_Clause]  
      [Required_Clause]  
      Using_Clause.

    Reduce_Expression :=                                                                                      
      'REDUCE' Input_Rowset   
      ['PRESORT'Identifier_List] 
      ('ALL' | 'ON' Identifier_List) 
      Produce_Clause  
      [SortedBy_Clause]
      [Readonly_Clause]  
      [Required_Clause]
      USING_Clause.

    SortedBy_Clause := 'SORTED' 'BY' Identifier_List.

_Semantics_

* `SortedBy_Clause` The optional `SORTED BY` clause asserts that the rows are ordered by the specified columns. This information allows the optimizer to potentially improve the performance of your script since it knows that the data is ordered. However, if the expression returns data that is not ordered according to the assertion, a runtime error is raised. If it is specified on a built-in Extractor, it will raise a compile time error.

See the U-SQL documentation for the semantics of the previously available clauses.

_Examples_

1. The following example will work because the input file is ordered according to the specified columns.

    Given the `searchlog.tsv` input file, and the following simple custom extractor:

        using Microsoft.Analytics.Interfaces;
        using Microsoft.Analytics.Types.Sql;
        using System;
        using System.Collections.Generic;
        using System.IO;
        using System.Linq;
        using System.Text;

        namespace Winter2018ReleaseNotes
        {
            public static class UpdatableRowExtensions
            {
                public static void SetColumnIfRequested<T>(this IUpdatableRow source, string colName, Func<T> expr)
                {
                    var colIdx = source.Schema.IndexOf(colName);
                    if (colIdx != -1)
                    { source.Set<T>(colIdx, expr()); }
                }
            }

            public class SLExtractor : IExtractor {
                public override IEnumerable<IRow> Extract(IUnstructuredReader input, IUpdatableRow output)
                {
                    foreach (Stream current in input.Split())
                    {
                        using (StreamReader streamReader = new StreamReader(current, Encoding.UTF8 ))
                        {
                            string[] array = streamReader.ReadToEnd().Split(new string[]{"\t"}, StringSplitOptions.None);
 
                            output.SetColumnIfRequested("UserId", () => Int32.Parse(array[0]));
                            output.SetColumnIfRequested("Start", () => DateTime.Parse(array[1]));
                            output.SetColumnIfRequested("Region", () => (array[2]));
                            output.SetColumnIfRequested("Query", () => (array[3]));
                            output.SetColumnIfRequested("Duration", () => Int32.Parse(array[4]));
                            output.SetColumnIfRequested("Urls", () => (array[5]));
                            output.SetColumnIfRequested("ClickedUrls", () => (array[6]));
                        }
                        yield return output.AsReadOnly();
                    }
                }
            }
        }

    the following script will succeed (although no optimization will occur due to its simplicity):

        @searchlog =
          EXTRACT UserId          int,
                  Start           DateTime,
                  Region          string,
                  Query           string,
                  Duration        int?,
                  Urls            string,
                  ClickedUrls     string
          FROM "/Samples/Data/SearchLog.tsv"
          SORTED BY Start
          USING new Winter2018ReleaseNotes.SLExtractor();

        OUTPUT @searchlog
        TO "/output/releasenotes/winter2017-18/extract-sorted-by.csv" 
        USING Outputters.Csv(outputHeader:true);

2. The following example will fail because the input file is not ordered according to the specified columns.
   
    Given the same input file and custom extractor, the following script will fail with the runtime error `E_RUNTIME_USER_ROWORDERING_CORRUPTED`:

        @searchlog =
          EXTRACT UserId          int,
                  Start           DateTime,
                  Region          string,
                  Query           string,
                  Duration        int?,
                  Urls            string,
                  ClickedUrls     string
          FROM "/Samples/Data/SearchLog.tsv"
          SORTED BY UserId
          USING new Winter2018ReleaseNotes.SLExtractor();

        OUTPUT @searchlog
        TO "/output/releasenotes/winter2017-18/extract-sorted-by.csv" 
        USING Outputters.Csv(outputHeader:true);

#### The `REQUIRED` clause for UDO invocations now allows `NONE`

The `REQUIRED` clause specifies which columns are required and cannot be pruned by the optimizer. It specifies that either all columns are required on input for the UDO (if specified with `*`), no columns are required (if specified with `NONE`) or the specified columns are required. If a specified column is followed by a list of columns in parenthesis, then the input column is only required if the columns in that list are referenced from the output. The default is that all columns are required. 

It is recommended that if you want to allow the optimizer to pass column pruning up through the UDO invocation, to specify the minimal set of columns that are required. Passing the column pruning up higher in the script will provide faster script execution since less data needs to be passed, or - if the column pruning reaches the extractor - read.

_Syntax_

    Required_Clause :=                                                                                  
      'REQUIRED' None_Star_Or_Required_Column_List.

    Star_Or_Required_Column_List :=  
      '*' | `NONE` | Required_Column_List.

#### The `EXTRACT` expressions now support the `REQUIRED` clause to support column pruning in user-defined extractors

U-SQL supports column pruning in built-in extractors such as `Extractors.Csv` and `Extractors.Parquet`. Example of column pruning are the case where the `EXTRACT`'s expressions schema may specify more than the columns needed in the script, e.g., because the extractor needs them for identifying the position in the data such as the CSV extractor, but the final script only uses a few of the specified columns. In that case the query compilation will only generate code that will extract the required columns and skip the other columns.

In the case of user-defined extractors, U-SQL does not know the internal implementation of the extractor. Thus per default, it assumes that all the columns need to be extracted. However, column pruning may be interesting for user-defined extractors as well. If a user-defined extractor is written in a way that it does not need all columns to be extracted, then the `REQUIRED` clause can be used to specify which columns cannot be pruned and the optimizer can then provide column pruning.

_Syntax_

    Extract_Expression :=                                                                                    
      'EXTRACT' Column_Definition_List
      Extract_From_Clause
      [SortedBy_Clause]
      [Required_Clause]
      USING_Clause.

See [above](#the-required-clause-for-udo-invocations-now-allows-none) for the semantics of the `Required_Clause`.

_Example_

The following sample extractor will only process the columns `Start`,`Region`,`Query`,`Duration`,`Urls`,`ClickedUrls` of the search log file if they are specified in the `EXTRACT` schema and are not pruned by the query optimizer, while the column `UserId` is always processed and raises an error if it is missing. It writes information about whether the column is used into the diagnostic stream info for better observing the behavior.

    using Microsoft.Analytics.Interfaces;
    using Microsoft.Analytics.Types.Sql;
    using Microsoft.Analytics.Diagnostics;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    namespace RequiredOnExtract
    {
        public static class UpdatableRowExtensions
        {
            public static void SetColumnIfRequested<T>(this IUpdatableRow source, string colName, Func<T> expr)
            {
                var colIdx = source.Schema.IndexOf(colName);
                if (colIdx != -1)
                {
                    DiagnosticStream.WriteLine(String.Format("Column <{0}> is requested.", colName));
                    source.Set<T>(colIdx, expr());
                }
            }
        }
    
        public class SLExtractor : IExtractor
        {
            public override IEnumerable<IRow> Extract(IUnstructuredReader input, IUpdatableRow output)
            {
                foreach (Stream current in input.Split())
                {
                    using (StreamReader streamReader = new StreamReader(current, Encoding.UTF8))
                    {
                        string[] array = streamReader.ReadToEnd().Split(new string[] { "\t" }, StringSplitOptions.None);

                        output.Set<int>("UserId", Int32.Parse(array[0]));
                        output.SetColumnIfRequested("Start", () => DateTime.Parse(array[1]));
                        output.SetColumnIfRequested("Region", () => (array[2]));
                        output.SetColumnIfRequested("Query", () => (array[3]));
                        output.SetColumnIfRequested("Duration", () => Int32.Parse(array[4]));
                        output.SetColumnIfRequested("Urls", () => (array[5]));
                        output.SetColumnIfRequested("ClickedUrls", () => (array[6]));
                    }
                    yield return output.AsReadOnly();
                }
            }
        }
    }

If the above extractor is invoked with the following script, then only the columns will actually be executed and the pruned columns will not be requested from the extractor, even though they have been requested:

    SET @@Diagnostics = true;

    @searchlog =
      EXTRACT UserId          int,
              Start           DateTime,
              Region          string,
              Query           string
      FROM "/Samples/Data/SearchLog.tsv"
      REQUIRED UserId
      USING new RequiredOnExtract.SLExtractor();

    @res =
      SELECT UserId,
             Start
      FROM @searchlog;

    OUTPUT @res
    TO "/output/releasenotes/winter2017-18/extract-required.csv"
    USING Outputters.Csv(outputHeader:true);

The resulting diagnostics.xml file will look something like this (in pretty print format and shortened):

    <Vertex name="SV1_Extract[0][0]" v="0" guid="BD3344B0-223A-43CF-B6BD-E342BC5A239E">
      <l>Column &lt;Start&gt; is requested.</l>
      <l>Column &lt;Start&gt; is requested.</l>
      <l>Column &lt;Start&gt; is requested.</l>
      <l>Column &lt;Start&gt; is requested.</l>
      <l>Column &lt;Start&gt; is requested.</l>
      <l>Column &lt;Start&gt; is requested.</l>
      <l>Column &lt;Start&gt; is requested.</l>
      <l>Column &lt;Start&gt; is requested.</l>
      ...
    </Vertex>

#### U-SQL adds compile-time user errors and warnings

U-SQL allows the user to raise compile-time errors and warnings with the new `RAISE` statement that is added to the `Statement` [grammar rule](https://msdn.microsoft.com/en-us/azure/data-lake-analytics/u-sql/u-sql-scripts).

_Syntax_

    RAISE_Statement :=
      'RAISE' ('ERROR' | 'WARNING') Static_String_Expression.

_Semantics_

* `RAISE ERROR`

    This raises a compile time error `E_CSC_USER_RAISEERROR` of the form:

        DESCRIPTION: The user script raised a user defined error.
        MESSAGE: RAISE ERROR: _the provided error message_

* `RAISE WARNING`

    This raises a compile time warning `W_CSC_USER_RAISEWARNING` of the form:

        DESCRIPTION: The user script raised a user defined warning.
        MESSAGE: RAISE WARNING: _the provided warning message_

* `Static_String_Expression`

    The static string expression that provides the error/warning message. If the expression evaluates to `null`, an error is raised.

_Limitations_

1. Warnings are currently not passed through to the front-end APIs or tooling. This will be fixed in a future refresh.

_Example_

The following script will raise an error if a file does not exists or if it contains some data and it will raise a warning if the output file already exists.

    DECLARE @semfile = @"/output/releasenotes/winter2018/raise-error-warning_sem.txt";
    DECLARE @outfile = @"/output/releasenotes/winter2018/raise-error-warning_out.csv";

    DECLARE @error = "Semaphore file <" + @semfile +"> does not exists or is locking the job. Job aborted.";
    DECLARE @warning = "Output file <{0}> already exists and will be overwritten.";

    IF (!FILE.EXISTS(@semfile) || FILE.LENGTH(@semfile) > 0) THEN
       RAISE ERROR @error;
    ELSEIF (FILE.EXISTS(@outfile)) THEN
       RAISE WARNING String.Format(@warning, @outfile);
    END;

    @data = SELECT "This is some data" AS data FROM (VALUES(1)) AS T(x);

    OUTPUT @data
    TO @outfile
    USING Outputters.Csv();

    OUTPUT (SELECT x FROM (VALUES(1)) AS T(x) WHERE false) 
    TO @semfile
    USING Outputters.Csv();

#### U-SQL User-defined Operators can now request more memory and CPUs with annotations

Azure Data Lake Analytics (ADLA) executes U-SQL scripts by partitioning the script into stages where each stage executes a certain amount of processing. Each stage can be scaled out based on input data partitioning into vertices that all execute the same processing over a different partition of the data. Each of these vertices gets a container to run the code that does the processing. 

Today, the container is limited to 6GB of memory overall and 2 virtual CPU cores. Since the system tries to have several operations executed in a vertex, U-SQL in ADLA limits the amount of memory for each user-defined operator (e.g., an extractor) to 500MB. 

While this limit works for many cases, sometimes you want to use a different amount of memory for the following reasons:

1. You may want more memory because your processing needs to allocate more complex data structures (think of a machine learning model or an XML or JSON document model).
2. You want to allocate more operators into a vertex since they don't need 500MB each which will improve the performance of the job. This can be achieved by requesting less memory. 

In order to help with these scenarios, U-SQL's UDO model is adding an annotation that allow you to request a different amount of memory with the following .NET annotation:

    [SqlUserDefinedMemory(Max=n)]

where `n` is a value of type `long` that indicates the maximal amount of memory that the UDO requests in bytes. It's default is 500MB. If the requested memory exceeds the container's 6GB limit, a larger container will be requested, or an error is raised if such a container cannot be provided. Note that a larger container will incur additional costs. 

If you request more memory, operations that previously were inside the same vertex may be split across several vertices, thus increasing the job's execution time. If you request less memory, operations that previously were in different vertices may be combined into the same vertex, thus improving the job's overall execution time.

The UDO will be allowed to allocate at least `n` bytes, although the system may allow around 300MB more than specified, since it keeps 300MB of memory as allocation buffer for a variety of memory needs in the vertex. 

_Examples:_

The following user-defined extractor is requesting 2GB of memory in its annotation and will attempt to allocate bytes memory in increments of `incr` bytes. If it catches an error while it tries to allocate the memory, it will stop and report the error message in the rowset.

    using Microsoft.Analytics.Interfaces;
    using Microsoft.Analytics.Types.Sql;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    namespace UDO_Annotations
    {

        internal static class MyLimits
        {
            public const long MaxUdoMemory = 2L * 1024 * 1024 * 1024;
        }

        [SqlUserDefinedExtractor]
        [SqlUserDefinedMemory(Max=MyLimits.MaxUdoMemory)]
        public class MyExtractor : IExtractor
        {
            private long max_allocation_size;
            private long increment;
            private int no_buff;
            private byte[][] alloc_mem;

            public MyExtractor(long max_alloc_sz = 10*1024*1024, long incr = 1024*1024){
                max_allocation_size = max_alloc_sz;
                increment = incr;
                no_buff = (int)Math.Ceiling((decimal) max_alloc_sz/(decimal)incr);
                alloc_mem = new byte[no_buff][];
            }

            public override IEnumerable<IRow> Extract(IUnstructuredReader input, IUpdatableRow outputrow)
            {
                outputrow.Set<long>("GC_TotalMem_Start", GC.GetTotalMemory(true));
                outputrow.Set<long>("MaxUDOMemory", MyLimits.MaxUdoMemory);
    
                var buff_idx = 0;
                var failed = false;
                var gc_mem = GC.GetTotalMemory(true);
                try
                {
                    while (buff_idx < no_buff) {
                        alloc_mem[buff_idx] = new byte[increment];
                        alloc_mem[buff_idx][0] = 1; // to avoid it being optimized away
                        buff_idx++;
                        gc_mem = GC.GetTotalMemory(true);
                    }
                }
                catch (Exception e)
                {
                    failed = true;
                    outputrow.Set<string>("error", e.Message);
                }
                outputrow.Set<long>("GC_TotalMem_End", gc_mem);
                outputrow.Set<bool>("failed", failed);
                outputrow.Set<long>("alloc_sz", buff_idx*increment);

                yield return outputrow.AsReadOnly();
            }
        }
    }

The following script applies this extractor each of the files in the specified file set and tries to allocate 3GB each, thus causing an out of memory exception after allocating the 2GB plus some of the above mentioned buffer on every invocation:

    @data =
    EXTRACT
        filename string,
        GC_TotalMem_Start long,
        MaxUDOMemory long,
        GC_TotalMem_End long,
        alloc_sz long,
        failed bool,
        error string
    FROM "/Samples/Data/{filename}"
    USING new UDO_Annotations.MyExtractor(max_alloc_sz : 3L * 1024 * 1024 * 1024, incr: 1024*1024);

    OUTPUT @data
    TO "/output/releasenotes/winter2017-18/udo_annotation_maxmem.csv"
    USING Outputters.Csv(outputHeader : true);

The resulting file may look something like:

| filename | GC_TotalMem_Start | MaxUDOMemory | GC_TotalMem_End | alloc_sz | failed | error |
|----------|-------------------|--------------|-----------------|----------|--------|-------|
| AdsLog.tsv | 86008 | 2147483648 | 2451713256 | 2451570688 | True | Exception of type 'System.OutOfMemoryException' was thrown. |
| SearchLog.tsv | 86008 | 2147483648 | 2451713352 | 2451570688 | True | Exception of type 'System.OutOfMemoryException' was thrown. |

#### U-SQL Cognitive Library additions

##### OCR Extraction is now available as an Extractor and an Applier

In previous versions of the U-SQL cognitive libraries, optical character recognition (OCR) "extraction" was only provided as a U-SQL processor. The processor worked well on data that is already in a rowset, e.g., operating on images that are stored in a table. However, it only operated on data that fit into a row, thus limiting the image to the row size limit of at most 4MB. It also only provided a single string and not the location of individually identified strings.

In the latest release of the cognitive libraries, the older `Cognition.Vision.OcrExtractor` processor has now been replaced by the following OCR extractor and OCR applier. The extractor allows to perform the OCR processing directly on files, thus avoiding the file size limit. As a trade-off, the scale is limited by the scale of operating on file sets (up to 10000s of files) compared to millions of rows with the applier. The applier and extractor also allow identifying several text fragments in an image with their bounding box.
 
1.	`Cognition.Vision.OcrExtractor` Extractor

    For each JPEG file it gets applied to, this U-SQL extractor returns one row per detected string in the file (column `Text` of type `string`) with additional information about the detected string's bounding box (columns `RectX`, `RectY`, `Width`, `Height` all of type `float`).

    All columns have to be specified in the EXTRACT clause.

    It provides the following argument to specify the string's column name with its default:

        Cognition.Vision.OcrExtractor(string txtCol = "Text")

    _Example_

    The following statement creates a rowset of all detected strings and their bounding boxes from all JPEG files with the file extension `.JPG` in the specified directory. It uses a different column name for the number of faces in the image column.

        REFERENCE ASSEMBLY ImageCommon;
        REFERENCE ASSEMBLY ImageOcr;

        @ocrs_from_extractor =
          EXTRACT FileName string, 
                  RectX float,
                  RectY float,
                  Width float,
                  Height float,
                  Text string
          FROM @"/usqlext/samples/cognition/{FileName}.jpg"
          USING new Cognition.Vision.OcrExtractor(txtCol : "Text");

        OUTPUT @ocrs_from_extractor 
        TO "/output/releasenotes/winter2017-18/ocr_extractor.csv"
        USING Outputters.Csv(outputHeader : true);


2.  `Cognition.Vision.OcrApplier` Applier

    For each JPEG image provided as a byte array in the column with the default name `ImgData`, it returns one row per detected string in the file with additional information about the detected string's bounding box (all with the same column names and types as the extractor).

    It provides the following arguments to specify the relevant input and output column names with their defaults:

        public OcrApplier(
            string imgCol = "ImgData", 
            string txtCol = "Text") 

    _Example_

    The following scripts creates a rowset of all detected strings and their bounding boxes from an input rowset called @imgs that contains the JPEG images in the column `image`:

        REFERENCE ASSEMBLY ImageCommon;
        REFERENCE ASSEMBLY ImageOcr;

        @imgs =
          EXTRACT FileName string,
                  image byte[]
          FROM @"/usqlext/samples/cognition/{FileName}.jpg"
          USING new Cognition.Vision.ImageExtractor();

        @ocrs_from_applier =
          SELECT FileName,
                 ocr.*
          FROM @imgs CROSS APPLY USING
                     new Cognition.Vision.OcrApplier(imgCol : "image") 
                     AS ocr(RectX float, RectY float, Width float, Height float, Text string);

        OUTPUT @ocrs_from_applier 
        TO "/output/releasenotes/winter2017-18/ocr_applier.csv"
        USING Outputters.Csv(outputHeader : true);

##### Key Phrase Extraction is now available as an Extractor and an updated Processor

In previous versions of the U-SQL cognitive libraries, key phrase extraction was only provided as a U-SQL processor. The processor worked well on data that is already in a rowset, e.g., operating on texts that are stored in a table. However, it only operated on data that fit into a string value, thus limiting the text to the string size limit of at most 128kB of UTF-8 encoded text data. It also extracted the keyphrases as ; delimited values in a string.

In the latest release of the cognitive libraries, the older `Cognition.Text.KeyPhraseExtractor` processor has now been replaced by the following extractor and processor. The extractor allows to perform the key phrase extraction directly on files, thus avoiding the size limit. As a trade-off, the scale is limited by the scale of operating on file sets (up to 10000s of files) compared to millions of rows with the processor. In addition, the key phrases are now returned in a `SqlArray<string>` thus being only limited by the row size.
 
1.	`Cognition.Text.KeyPhraseExtractor` Extractor

    For each text file it gets applied to, this U-SQL extractor returns one row per file providing the number of detected key phrases (column `NumPhrases` of type `int`) and the key phrases (column `KeyPhrase` of type `SqlArray<string>`).

    All columns have to be specified in the EXTRACT clause.

    It provides the following arguments to specify the column names with their defaults:

        Cognition.Text.KeyPhraseExtractor(outCol = "KeyPhrase", string numCol = "NumPhrases")

    _Example_

    The following statement extracts the key phrases from the specified files (War and Peace) together with their count. It uses the default column names.

        REFERENCE ASSEMBLY [TextKeyPhrase];

        @keyphrase_from_extractor = 
          EXTRACT FileName string,
                  NumPhrases int,
                  KeyPhrase SQL.ARRAY<string>
          FROM @"/usqlext/samples/cognition/{FileName}.txt"
          USING new Cognition.Text.KeyPhraseExtractor();

        @keyphrase_from_extractor_exploded =
          SELECT FileName, NumPhrases, T.KeyPhrase
          FROM @keyphrase_from_extractor
               CROSS APPLY EXPLODE (KeyPhrase) AS T(KeyPhrase);

        OUTPUT @keyphrase_from_extractor_exploded
        TO "/output/releasenotes/winter2017-18/keyphrase-extractor.csv"
        USING Outputters.Csv(outputHeader : true);


2.  `Cognition.Text.KeyPhraseProcessor` Processor

    For each row in its input rowset this U-SQL processor returns one row providing the detected key phrases (column `KeyPhrase` of type `SqlArray<string>`) for the specified input text column (column `Text` of type `string`).

    It provides the following arguments to specify the column names with their defaults:

        Cognition.Text.KeyPhraseProcessor(string txtCol = "Text", string outCol = "KeyPhrase")

    _Example_

    The following statement extracts the key phrases from the specified file's `Sentence` column (War and Peace).

        REFERENCE ASSEMBLY [TextKeyPhrase];

        @WarAndPeace =
          EXTRACT No int,
                  Year string,
                  Book string,
                  Chapter string,
                  Sentence string
          FROM @"/usqlext/samples/cognition/war_and_peace.csv"
          USING Extractors.Csv();

        @keyphrase =
          PROCESS @WarAndPeace
          PRODUCE No,
                  Year,
                  Book,
                  Chapter,
                  Sentence,
                  KeyPhrase SqlArray<string>
          READONLY No,
                   Year,
                   Book,
                   Chapter,
                   Sentence
          USING new Cognition.Text.KeyPhraseProcessor(txtCol:"Sentence");

        @keyphrase_exploded =
          SELECT No,
                 Year,
                 Book,
                 Chapter,
                 Sentence,
                 T.KeyPhrase
          FROM @keyphrase
               CROSS APPLY EXPLODE (KeyPhrase) AS T(KeyPhrase);

        OUTPUT @keyphrase_exploded
        TO "/output/releasenotes/winter2017-18/keyphrase-extractor.csv"
        ORDER BY No, Year, Book, Chapter
        USING Outputters.Csv(outputHeader : true);

###### Comment regarding Python, R, and Cognitive Libraries

In order to get access to the updates to the Python, R and Cognitive libraries, you have to install or refresh the U-SQL Extension libraries through the Azure Data Lake Analytics portal (currently under the Sample Scripts section). 

## Azure Data Lake Tools for Visual Studio New Capabilities

The Azure Data Lake Tools have done a lot of improvements in laying out the information. The following are some of the most important additions and changes that are available in versions 2.3.3000.4 and later.

#### ADL Tools for VisualStudio provides an improved Analytics Unit modeler to help improve a job's performance and cost

#### Job Submission's simple interface now makes it easier to change the allocated AUs

#### Improved visualization of the job execution graph inside a vertex

#### The job stage graph and job execution graph now indicates if the stage contains user-defined operators and what language they have been authored in

The job graph now indicates if the stage's vertices contain user-defined operators (UDOs). .NET UDOs and the R script invoking UDO are marked as in this picture:

....

#### Separate tab for enumerating all input and output data

#### Job View includes a link to the diagnostic file's folder

## Azure Portal Updates

#### The portal provides an improved Analytics Unit modeler to help improve a job's performance and cost


## PLEASE NOTE:

In order to get access to the new syntactic features and new tool capabilities on your local environment, you will need to refresh your ADL Tools. You can download the latest version for VS 2013 and 2015 from [here](http://aka.ms/adltoolsvs) or use the Check for Updates menu item in VS. If you are using VisualStudio 2017, you currently have to wait for the next VisualStudio 2017 refresh that should occur about every 6 to 8 weeks.

Otherwise you will not be able to use the new features during local run and submission to the cluster will give you syntax warnings for the new language features (you can ignore them and still submit the job).
