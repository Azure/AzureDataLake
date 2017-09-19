# U-SQL Summer Release Notes 2017-09-19
--------------------------
## Pending and Upcoming Deprecations and Breaking Changes

Please review your code and make sure you are cleaning your existing code to be ready for the following 
deprecations of U-SQL preview features.

**Please note: Previously announced deprecation items are now deprecated and raise errors instead of warnings!**

Follow our [Azure Data Lake Blog](http://blogs.msdn.microsoft.com/azuredatalake) for more details and future announcements of deprecation timelines.

#### U-SQL jobs will introduce an upper limit for the number of table-backing files being read

U-SQL tables are backed by files. Each table partition is mapped to its own file, and each `INSERT` statement adds an additional file (unless a table is rebuild with `ALTER TABLE REBUILD`). 

If the file count of a table (or set of tables) grows beyond a certain limit and the query predicate cannot eliminate files (e.g., due to too many insertions), there is a large likely-hood that the compilation times out after 25 minutes. 

In previous releases, there was no limit on the number of table files read by a single job. In the current release we raise the following warning, if the number of table-backing files exceeds the limit of 3000 files per job:

>Warning: WrnExceededMaxTableFileReadThreshold

>Message: Script exceeds the maximum number of table or partition files allowed to be read. 

This message will be upgraded to an error message in a future refresh. 

If your current job compiles but receives the warning today, we will provide a way to keep the warning in the next release.

Please note: This limit only applies to reading from table-backing files. Normal files don't have an explicit limit and have a much higher limit in practice since they use a different execution plan. The limit also only applies to files that are actually read and ignores the files in table partitions which are not used by the query.

In a future release, we will work on addressing this limit (no timeline yet). If you feel this is important to your scenario, please add your vote to the [feature request](https://feedback.azure.com/forums/327234-data-lake/suggestions/19050232-increase-number-of-u-sql-table-files-partitions-th).

#### Table-valued functions will disallow result variable names to conflict with parameter names

In a future refresh, table-valued functions will disallow a result variable from having the same name as any of the function's parameters. Currently the following warning is being raised:

> Warning: WrnReturnRowsetNameConflictWithParameter

> Message: Return rowset name {0} conflicts with a parameter name.

This message will be upgraded to an error message in a future refresh. 

Please change the names to disambiguate your return and parameter names.

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

In the next refresh, we will give you an option to enable this change so you can test your existing scripts and see if you are going to be impacted. At some additional point in the future, we will change the default behavior of that switch and always map the empty field to null.

#### Disallowing user variables that start with `@@`

Due to a parser bug, users currently can declare variable names that use the reserved system variable namespace that is indicated with a double at-sign (`@@`). 

There is no warning in this refresh, but there will be a warning in the next refresh and an error soon after.

This means a declaration like 

	DECLARE @@orders_file string = "sales/summary/2016/02/order16.txt";

should be avoided and will fail in the future.

#### Disallow U-SQL identifiers in C# delegate bodies in scripts

Since C# delegate bodies are normal C# from a binding perspective, U-SQL identifiers should be passed in as parameters to the delegate instead.

## Breaking Changes

#### Previously announced deprecation items are now removed. Please check previous release notes for details

The recent refreshes raise errors for the following deprecated items (click on link for details):

1. [The '{col:*}' File Set pattern has been deprecated](https://github.com/Azure/AzureDataLake/blob/master/docs/Release_Notes/2016/2016_09_12/USQL_Release_Notes_2016_09_12.md#deprecation-of-col-file-set-pattern) and raises the following error: 

    `E_CSC_USER_STARFORMATSPECIFIERISDEPRECATED: The '*' format specifier for enumeration, i.e. {virtualColumn:*}, was deprecated and removed.`

    Use '{col}' without the wildcard format specifier instead.

2. [The '{date:hh}' and '{date:h}' File Set patterns have been deprecated](https://github.com/Azure/AzureDataLake/blob/master/docs/Release_Notes/2016/2016_09_12/USQL_Release_Notes_2016_09_12.md#datetime-file-set-pattern-will-require-hh-instead-of-hh-for-the-hour-pattern-to-align-with-24h-clock-semantics) and raise the following error:

    `E_CSC_USER_LOWERCASEHOURFORMATSPECIFIERSAREDEPRECATED: The 'hh' and 'h' format specifiers on DateTime virtual columns were deprecated and removed.`

    Use the 'HH' and 'H' format specifier instead. For example: '{date:HH}' and '{date:H}'

3. [`DROP CREDENTIAL` has been removed](https://github.com/Azure/AzureDataLake/blob/master/docs/Release_Notes/2017/2017_04_24/USQL_Release_Notes_2017_04_24.md#drop-credential-ddl-will-start-to-raise-an-error-in-an-upcoming-refresh) and raises the following error message:

    `E_CSC_USER_CREDENTIALDDLISREMOVED: CREATE/ALTER/DROP CREDENTIAL statements used in the script are removed from U-SQL language.`

    Instead, use the credential management commandlets in the latest Azure Powershell SDK version.

#### The Built-in extractors' verification of the UTF-8 encoding correctness has been strengthened

In previous releases, the following verifications were applied, throwing user-error exceptions to reject any of the following malformed UTF-8 inputs:

- Reject if first byte suggests a code point greater than 0x1FFFFF (which would require > 4 bytes of UTF-8).
- Reject if a trailing byte isn’t in the range from 0x80 through 0xBF (which means reject “lonely” leading bytes in the range from 0xC0 through 0xFF).
- Reject incomplete multi-byte characters.

The following new verifications have been introduced in the new release and now raise an error:

- Reject leading bytes in the range from 0x80 through 0xBF.  (These values are reserved for use as trailing bytes in multi-byte UTF8 encodings.)
- Reject overlong encodings. That is, make sure that every Unicode code point is encoded in the minimum number of UTF-8 code-unit bytes that can express that code-point.
- Reject code-points from 0x110000 through 0x1FFFFF (which are defined by Unicode specifications as being out of range, and which cannot be expressed in UTF-16, the in-memory string representation used by .NET).
- Reject encoding of half-surrogates.  The Unicode standard reserves all code points in the range 0xD800 to 0xDFFF for use as part of UTF-16 surrogate pairs, and must not be encoded in UTF-8 as distinct code points (even if a “high surrogate” is immediately followed by a “low surrogate”). 

If your job used to run, you can turn these new verifications off by setting the `@@InternalDebug` system flag `USQLLaxUtf8Validation` flag to on by adding the following statement to your script:

    SET @@InternalDebug = "USQLLaxUtf8Validation:on";

Please note that we **strongly recommend** that you fix such bad input data at the source. If your data is in an ANSI encoding such as Windows-1252 but has passed with UTF-8 encoding, please upvote the [feature request for ANSI code page support](https://feedback.azure.com/forums/327234-data-lake/suggestions/13077555-add-ansi-code-page-support-for-built-in-extractors).

#### API changes for the cognitive extension libraries

Azure Data Lake Analytics provides a set of libraries for running Python and R code and use some of the cognitive processing capabilities on images and text that can be installed as U-SQL extensions via the Azure Data Lake Analytics Portal. 

These assemblies are currently considered to be in a preview release stage. Therefore, more changes may occur in future refreshes, such as moving the text assemblies to use the same extractor/applier model as the image assemblies.


The recent refresh introduced the following breaking changes:

##### Image Tagging Processor `Cognition.Vision.ImageTagger` changed data type of `Tags` column from `string` to `SQL.MAP<string, float?>`

The image tagging processor `Cognition.Vision.ImageTagger` changed the resulting `Tags` column's data type from `string` to `SQL.MAP<string, float?>` in order to provide more detailed confidence level information for each returned tag. 

##### The `Cognition.Vision.EmotionaAnalyzer` UDO has been removed

This UDO has been replaced by an equivalent extractor and applier. For more details on the replacements see below (TBD: Add forward link).

##### The `Cognition.Vision.FaceDetector` UDO has been removed

This UDO has been replaced by an equivalent extractor and applier. For more details on the replacements see below (TBD: Add forward link).

##### The assembly `TextCommon` has been removed for the set of deployed assemblies

This assembly provided a function called `Cognition.Text.Splitter("KeyPhrase")` which provided the same capabilities as the C# expression `KeyPhrase.Split(';')`. Please use the C# expression instead.

**NOTE: If you already have installed an earlier version of the extensions, you will have to reinstall the newest version from the Azure Portal.**

## Major U-SQL Bug Fixes, Performance and Scale Improvements

Besides many internal improvements and fixes to reported bugs, we would like to call out the following major bug fixes and improvements to performance and scalability of the language.

#### Job graph build time optimization. 

This performance optimization improves the job initialization time for large jobs.

#### U-SQL Python Support bug fixes and performance and scale improvements

##### Improvements to python library deployment via unzip

Python libraries deployed via unzip are now cached across all vertices that run in the same physical container, thus reducing the vertex creation time overhead.

##### Enabling Python Extensions in Local Run mode

To use the Python extensions in local run mode set the `PY_HOME` environment variable to point to your local python deployment. The extensions will search for `<PY_HOME>/<version>/python.exe` which should be `<PY_HOME>/3.5.1/python.exe` by default.

#### U-SQL R Extensions bug fixes and performance and scale improvements

##### R Engine has been upgraded

The R engine deployed by the R extensions has been upgraded to MRS 9.1.

## U-SQL Preview Features

We currently have the following U-SQL features in preview. A feature in preview means that we are still finalizing the implementation of the feature, but are soliciting feedback and want to make it available ahead of a full release due to their value to the scenarios they address and the ability to learn more from our customers.

**Since we are still testing these features, you are required to opt in. Please [contact us](mailto:usql@microsoft.com) if you want to explore any of these capabilities and the opt-in is not provided in the description below.**

#### Input File Set scales orders of magnitudes better (now with additional improvements!) (opt-in statements are provided)

Previously, U-SQL's file set pattern on `EXTRACT` expressions ran into compile time time-outs around 800 to 5000 files. 

U-SQL's file set pattern now scales to many more files and generates more efficient plans.

For example, a U-SQL script querying over 2500 files in our telemetry system previously took over 10 minutes to compile 
now compiles in 1 minute and the script now executes in 9 minutes instead of over 35 minutes using a lot less AUs.

We  have compiled scripts that access 30'000 files and have seen customers successfully compile scripts with over 150'000 files.

The preview feature can be turned on by adding the following statement to your script:

	SET @@FeaturePreviews = "FileSetV2Dot5:on";

During file set expansion, the compiler makes one synchronous store enumeration request per subfolder that is being expanded. We have implemented a new feature to expand file sets asynchronously using thread pool threads to improve throughput. The feature is off by default and can be turned on for preview with the following flag:

	SET @@FeaturePreviews = "AsyncCompilerStoreAccess:on";

If you want to turn on both, you can either specify them in two statements or write a single statement of the form

	SET @@FeaturePreviews = "FileSetV2Dot5:on, AsyncCompilerStoreAccess:on";

#### Automatic GZip compression on `OUTPUT` statement is now in preview (opt-in statement is provided)

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


#### DiagnosticStream support in .Net User-code

The U-SQL .NET extension model added a new object called `Microsoft.Analytics.Diagnostics.DiagnosticStream` that allows to write some (limited) diagnostic information into a file in the job folder. 

The preview can be enabled with

    SET @@FeaturePreviews = "DIAGNOSTICS:ON";

##### Documentation

U-SQL provides a `DiagnosticStream` static class in `Microsoft.Analytics.Diagnostics` that gives the user the option to record diagnostic information that will be make available in the job folder. 

_Generated file:_

Each job will receive its own file in the job folder in ADLS at

`/system/jobservice/jobs/Usql/{yyyy}/{MM}/{dd}/{HH}/{mm}/{job-guid}/diagnosticstreams/diagnostic.xml`

The file contains the following information (`...` indicates repetition) as an XML fragment in UTF-8 encoding (meaning it can have no XML declaration and can have more than one top-level XML element):

    <Vertex name="vertexname" v="vertexversion" guid="vertex-guid-withoutbraces">
      <l>entitized-user-provided-content</l> ...
    </Vertex> ...

Note that each Vertex element appears in its own line and there are no CR/LF or additional whitespaces in its content (they are added in the example above for better visualization only).

The diagnosis has the following limits:

1.	Each `entitized-user-provided-content` cannot be larger than 100kb of UTF-16 encoded, pre-entitized data (e.g., the user writes `A\rB` which will be entitized to `A&xD;B`. This will be counted as 3 bytes).
2.	The overall diagnosis file size limit is 5GB. This includes all the XML tags and attribute information.

Since writing diagnosis information should not fail, too much data will be indicated by adding the following information:

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
5.	A vertex fails due to system-error: The last failed vertex is written with the content. The element representing the failed vertex gets an additional attribute `failed="true"`.

Diagnostic File Expiration:

Since the file is written into the job folder, its expiration is subject to the job data expiration. The user can either increase the expiration for the job, or copy the file into a separate location.

_Methods on the `DiagnosticStream` class_

    void DiagnosticStream.WriteLine(string user_provided_content)

Takes the `user_provided_content` and writes up to the first 100kb of the provided data (size determined based on UTF-16 encoding) into the diagnose stream into an `l` element after entitizing CR and LF using XML character code entities and entitizing < and & with the named XML character entities. If the data is more than 100kb an additional attribute `truncated="true"` will be added to the `l` element. Note that the data in the file will be UTF-8 encoded.

Example:

    DiagnosticStream.WriteLine("This is an example with an &");

Adds the following line to the vertex’ element content:

    <l>This is an example with an &amp;</l>

_System Variable controls Diagnostic Output_ (future feature, not yet available)

Often diagnostics may only be used under certain conditions (debug executions etc) and otherwise can lead to negative performance impact, even if the user wraps it with an if statement. 

Therefore, U-SQL will provide a system variable `@@Diagnostics` to control if the DiagnosticStream code gets invoked or not.

The two acceptable values are on and off. The default is off, which means that the DiagnosticStream code does not get invoked. The following script statement will turn it on:

    SET @@Diagnostics = "on";

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


#### A limited flexible-schema feature for U-SQL table-valued function parameters is now available for preview (requires opt-in)

This feature allows to write more generic U-SQL table-valued functions and procedures, where only part of the schema of a table parameter needs to be present.

## New U-SQL capabilities

#### U-SQL Packages can now contain `USE` statements

The [recently introduced U-SQL packages](https://github.com/Azure/AzureDataLake/blob/master/docs/Release_Notes/2017/2017_04_24/USQL_Release_Notes_2017_04_24.md#u-sql-adds-the-notion-of-packages) can now contain `USE DATABASE` and `USE SCHEMA` statements to set the internal static default database and/or schema context.

#### U-SQL adds Catalog Views

U-SQL has added catalog views for some of the most commonly used catalog objects such as databases, tables etc.. This allows U-SQL scripts to programmatically query information about catalog objects. The catalog views are modeled after SQL Server's `sys` catalog views but are in the U-SQL specific reserved schema called `usql` because the catalog views are containing U-SQL specific information and data types.

Catalog views will not contain objects that have been created as part of the same script and will only show the objects that the user submitting the query has the rights to see.

**The U-SQL catalog views are currently _not_ available in the local run environment.**

Future releases will add additional catalog views. 

The following is the list of currently supported catalog views, their schema and their descriptions.

##### `usql.databases`

The catalog view `usql.databases` provides a list of all databases belonging to the account's catalog.

_Schema_

| Column name | Type | Description |
|-------------|------|-------------|
| database_id_guid | Guid | The database's unique identifier (unique within an account) |
| name | string | The database name (unique within an account) |
| create_date | DateTime? | The date and time when the database was created (in UTC-0 time zone). It may be null for databases that were created before the system started to track this information. |
 
#### `usql.schemas`

The catalog view `usql.schemas` provides a list of all schemas in the current database context.

_Schema_

| Column name | Type | Description |
|-------------|------|-------------|
| schema_id_guid | Guid | The schema's unique identifier (unique within a database) |
| name | string | The schema name (unique within a database) |
| database_id_guid | Guid | The identifier of the database that contains the schema |

##### `usql.objects`

The catalog view `usql.objects` provides a list of objects belonging to database schemas in the current database context. Currently it only includes tables, views, table-valued functions and table types. Other objects such as procedures and packages will be added in the future.

_Schema_

| Column name | Type | Description |
|-------------|------|-------------|
| object_id_guid | Guid | The object's unique identifier |
| name | string | The object's name (in simple, non-qualified form) |
| schema_id_guid | Guid | The identifier of the schema that contains the object |
| type | string | The short form indication of the object type: TF - table-valued function, TT - table type, U - user table, V - view |
| type_desc | string | The long form description of the object type: TABLE_VALUED_FUNCTION, TABLE_TYPE, USER_TABLE, VIEW |
| create_date | DateTime? | The date and time of the object's creation (in UTC-0 time zone)
| modify_date | DateTime? | The date and time of the object's last modification (in UTC-0 time zone)

##### `usql.tables`

The catalog view `usql.tables` provides a list of all the tables belonging to the schemas in the current database context.

The schema inherits the columns from the `usql.objects` and adds the table specific properties to it.

_Schema_

| Column name | Type | Description |
|-------------|------|-------------|
| inherits `usql.objects`|
| is_external | bool | Indicates if this is an external table |
| data_source_id_guid | Guid? | The Data Source ID if the table is external, otherwise null |

##### `usql.views`

The catalog view `usql.views` provides a list of all the views belonging to the schemas in the current database context.

| Column name | Type | Description |
|-------------|------|-------------|
| inherits `usql.objects`|
| is_schema_inferred | bool | Indicates if the schema is inferred from the schema's query expression or has been explicitly provided |
| definition | string | The view's definition (if available) |

##### `usql.functions`

The catalog view `usql.functions` provides a list of all the views belonging to the schemas in the current database context.

| Column name | Type | Description |
|-------------|------|-------------|
| inherits `usql.objects`|
| is_user_defined | bool | Indicates if it is a user defined function |
| definition | string | The function's definition (if available) |

##### `usql.types`

The catalog view `usql.types` provides a list of all the built-in types as well as the user-defined types (both scalar and table types) belonging to the schemas in the current database context. Since user-defined types are not directly registered in the U-SQL catalog (but are part of the assemblies), they are not included in this view.

**CAUTION: This catalog view's content is subject to change in a future refresh!**

_Schema_

| Column name | Type | Description |
|-------------|------|-------------|
| type_id_guid | Guid | The types's unique identifier |
| name | string | The type's name alias (unique within the type family) |
| type_family | string | Name of the type system family, together with the type name's alias, uniquely identifies the type. U-SQL supports the following values: `SQL` = SQL Server Type system family, `C#` = C# type system family, `U-SQL` = U-SQL table types |
| qualified_name | string | Fully qualified name of the underlying C# type library representing the type. This name uniquely identifies the type. | 
| schema_id_guid | Guid | ID of the database schema the type belongs to |
| is_explicit_nullable  | bool? | `True` = Type can be marked explicitly as nullable (e.g. `int?`); `False` = Type cannot be marked explicitly as nullable (e.g. C#'s `string`); `Null` = Is a table type |
| is_user_defined | bool | `True` = User-defined type; `False` = Built-in data type |
| is_assembly_type | bool | `True` = Implementation of the type is defined in a CLR assembly; `False` = Type is based on a SQL Server system data type or is a U-SQL table type |
| is_table_type | bool| Indicates if the type is a table type |
| is_complex | bool | `True` = Type is considered complex for serialization and query purposes; `False` = Type is considered scalar for serialization and query purposes |
| precision | int? | Indicates the max precision of the type if it is a numeric type, 0 otherwise, null if it is not a built-in type |
| scale | int? | Indicates the max scale of the type if it is a numeric type, 0 otherwise, null if it is not a built-in type |
| is_nullable | bool | `False` = Type implicitly allows null value; `True` = Type does not implicitly allow null value
`Null` = Is a table type |
| is_precise | int | Supported values are: `0` = Type semantics is imprecise; `1` = Type semantics is precise; `2` = Type semantics regarding precision is determined based on other aspects (e.g., in complex types, if all referenced types are precise then the complex type is precise); `Null` = Is a table type |
| is_foldable | bool | `False` =   Expressions of this type cannot be constant-folded in query processing; `True` = Expressions of this type can be constant-folded; `Null` = Is a table type |
| is_comparable | bool? | `False` = Values of this type cannot be compared or ordered; `True` = Values of this type can be compared and ordered; `Null` = Is a table type |
| is_real_number | bool? | `False` = Value of this type is not a real number; `True` = Value of this type is a real number; `Null` = Not a built-in type |

##### `usql.columns`

The `usql.columns` catalog view provides a list of all the columns of tables and table types in the schemas of the current database context.  

_Schema_

| Column name | Type | Description |
|-------------|------|-------------|
| name | string | The column's name (unique within the object to which the column belongs) |
| object_id_guid | Guid | The identifier of the object to which the columns belongs |
| column_id | int | the column's positional id within the object to which the column belongs (unique within the object) |
| type_id_guid | Guid | The column type's identifier |
| max_length | int? | The column's maximum length in bytes, or -1 for variable sized columns such as columns of type string or byte[] or of a complex type. The value `null` is reserved for future use. |

##### `usql.indexes`

The `usql.indexes` catalog view provides a list of all the indices in the schemas of the current database context.  

Note that some of the values documented below, such as the values for non-clustered indices and column-store indices are there for possible future use and are not currently used.

_Schema_

| Column name | Type | Description |
|-------------|------|-------------|
| name | string | The name of the index |
| object_id_guid | Guid | The identifier of the object on which the index is defined |
| index_id | int | ordinal position (starting at 1) of the index within the object/table |
| type | int | The index type: `1` = Clustered; `2` = Nonclustered; `5` = Clustered Columnstore |
| type_desc | string | Description of the index type: `CLUSTERED`; `NONCLUSTERED`; `CLUSTERED COLUMNSTORE` | 
 
##### `usql.index_columns`

The `usql.index_columns` catalog view provides the columns on which the indices of the schemas of the current database context are defined.  

Note that some of the values documented below, such as the values for non-clustered indices, column-store indices, or included columns are there for possible future use and are not currently used.

_Schema_

| Column name | Type | Description |
|-------------|------|-------------|
| object_id_guid | Guid | The identifier of the object on which the index is defined |
| index_id | int | The ordinal position (starting at 1) of the index within the object/table |
| index_column_id  | int | The position of the index column within the index (unique within the index_id) |
| column_id | int | The position of the column within the object on which the index is specified (unique within object_id_guid)  or 0 if it is the row identifier (RID) in a nonclustered index |
| key_ordinal | int | Ordinal (1-based) within the set of key-columns, or 0 if it is not a key column, or it is a columnstore index |
| is_descending_key | bool | `True` = Index key column has a descending sort direction; `False` = Index key column has an ascending sort direction, or the column is part of a columnstore or hash index |
| ​is_included_column | bool | ​`True` = Column is a non-key column added to the index as an included column; `False` = Column is not an included column |
 
##### `usql.stats`

The `usql.stats` catalog view provides the statistics defined on tables in the current database context.  

_Schema_

| Column name | Type | Description |
|-------------|------|-------------|
| object_id_guid | Guid | The identifier of the object on which the statistics is defined |
| name | string | The name of the statistics, unique for the object on which the statistics is defined |
| stats_id_guid | Guid | The identifier of the statistics (unique within the object) |
| create_date | DateTime? | The date of creation of the statistics. Some older statistics may have a value of null if they were created before the date was being recorded. |
| update_date | DateTime? | The last update date of the statistics. Some older statistics may have a value of null if they were updated before the date was being recorded. |
| stat_data_byte | byte[] | The content of the statistics |

##### `usql.stats_columns`

The `usql.stats_columns` catalog view provides the columns on which the table statistics in the schemas of the current database context are defined.  

_Schema_

| Column name | Type | Description |
|-------------|------|-------------|
| object_id_guid | Guid | The identifier of the object on which the statistics is defined |
| stats_id_guid | Guid | The identifier of the statistics of which the column is part |
| stats_column_id | int | The ordinal position (starting at 1) of the column within the list of statistics columns |
| column_id | int | The position of the column within the object on which the statistics is specified (unique within object_id_guid) |
 
##### `usql.distributions`

The `usql.distributions` catalog view provides the distribution schemes for the tables in the schemas of the current database context.  

_Schema_

| Column name | Type | Description |
|-------------|------|-------------|
| object_id_guid | Guid | The identifier of the object for which the distribution is specified |
| index_id | int? | The identifier of the index for which the distribution is specified or `null` if it is specified directly on the table. |
| distribution_type | int | Distribution type: `2` = Hash; `5` = Range; `6` = Round Robin; `17` = Direct Hash |
| distribution_type_desc | string | Distribution type description: `HASH`; `RANGE`; `ROUND ROBIN`; `DIRECT HASH` | 
| distribution_count | int | The specified count of distribution buckets if specified, null or 0 otherwise |
 
##### `usql.distribution_columns`

The `usql.distribution_columns` catalog view provides the columns used by the distribution schemes for the tables in the schemas of the current database context.  

_Schema_

| Column name | Type | Description |
|-------------|------|-------------|
| object_id_guid | Guid | The identifier of the object for which the distribution is specified |
| index_id | int? | The identifier of the index for which the distribution is specified or `null` if it is specified directly on the table. |
| distribution_column_id | int | The ordinal position of the column in the distribution definition |
| column_id | int | The position of the column within the object on which the statistics is specified (unique within object_id_guid) |
| is_descending_key | bool | `True` = The distribution column has a descending sort direction; `False` = The distribution column has an ascending sort direction |
 
##### `usql.partitions`

The `usql.partitions` catalog view provides the partition schemes for the partitioned tables in the schemas of the current database context.  

_Schema_

| Column name | Type | Description |
|-------------|------|-------------|
| partition_id_guid | Guid | The partition identifier |
| object_id_guid | Guid | The identifier of the object on which the partition is specified. |
| index_id | int | The identifier of the index for which the partition is specified. |
 
##### `usql.partition_range_values`

The `usql.partition_range_values` catalog view provides details about the range partition schemes for the partitioned tables in the schemas of the current database context.  

_Schema_

| Column name | Type | Description |
|-------------|------|-------------|
| partition_id_guid | Guid | The identifier of the range partition |
| parameter_id | int | The ordinal position (starting at 1) of the parameter (unique within a partition scheme). Values in this column correspond to the parameter_id column of the `usql.partition_parameters` view for any particular partition_guid_id. |
| value | string | The lexical representation of the value |
 
##### `usql.partition_parameters`

The `usql.partition_parameters` catalog view provides details about the partition parameters for the partition schemes in the schemas of the current database context.  

_Schema_

| Column name | Type | Description |
|-------------|------|-------------|
| object_id_guid | Guid | The identifier of the object on which the partition is specified. |
| index_id | int | The identifier of the index for which the partition is specified. |
| parameter_id | int | The ordinal position (starting at 1) of the parameter (unique within a partition scheme).|
| column_id | int | The ordinal position of the column in the partition scheme | 

##### Some catalog view examples

1. The following script returns all databases of the submitting account for which the submitting user has enumeration rights:

    OUTPUT (SELECT * FROM usql.databases) TO "/output/databases.csv" USING Outputters.Csv(outputHeader:true);

2. The following script returns the tables with their fully qualified, quoted names as well as their column information (name and type and maximal possible field size) ordered alphabetically by table and in order of their column positions:

    @res =
      SELECT "[" + db.name + "].[" + s.name + "].[" + t.name + "]" AS table_name,
             c.name AS col_name,
             c.column_id AS col_pos,
             ct.qualified_name AS col_type,
             c.max_length == - 1 ? 
               ct.qualified_name == "System.String" ? 
                 128 * 1024 
               : ct.qualified_name == "System.Byte[]" ? 
                   4 * 1024 * 1024 
                 : - 1 
             : c.max_length AS col_max_length
      FROM usql.databases AS db 
      JOIN usql.schemas AS s ON db.database_id_guid == s.database_id_guid
      JOIN usql.tables AS t ON s.schema_id_guid == t.schema_id_guid
      JOIN usql.columns AS c ON c.object_id_guid == t.object_id_guid
      JOIN usql.types AS ct ON c.type_id_guid == ct.type_id_guid;

    OUTPUT @res
    TO "/output/tableinfo.csv"
    ORDER BY table_name, col_pos
    USING Outputters.Csv(outputHeader : true);

#### U-SQL supports paging queries with `OFFSET/FETCH` clause

U-SQL uses the ANSI SQL standard compliant [`OFFSET FETCH` clause](https://msdn.microsoft.com/en-us/library/azure/mt621321.aspx#off_F) to retrieve the top N rows. The clause also supports paging by specifying the numbers of rows to skip before returning the next N rows. U-SQL now allows to specify that offset.

Note that there is no guarantee that different invocations with the same or different offsets operate from a single snapshot of the ordered rowset. The set of returned rows may be non-deterministically impacted if the order specification is not deterministic (e.g., the order by clause is under-specified, so that multiple rows can be ordered in the same local position) or the data changes between different invocations.

The updated syntax and semantics of the [`Offset_Fetch`](https://msdn.microsoft.com/en-us/library/azure/mt621321.aspx#off_F) clause is:

_Syntax_

    Offset_Fetch :=                                                                                     
       ['OFFSET' integer_or_long_literal ('ROW' | 'ROWS')] [Fetch].
  
    Fetch :=  
       'FETCH' ['FIRST' | 'NEXT']  integer_or_long_literal ['ROW' | 'ROWS']  
       ['ONLY'].

The OFFSET/FETCH clause is the ANSI SQL-conformant way to specify getting the first number of rows. U-SQL makes many of the keywords optional to minimize the amount of typing required. If the `OFFSET x ROWS` clause is not specified, it defaults to `OFFSET 0 ROWS`. If the `FETCH` clause is not specified, then all the rows starting after the skipped rows are being returned.

The `OFFSET` and `FETCH` clauses allow specifying integer or long literal values. The value for the `FETCH` clause has to be in the range [1, 10000], otherwise an error is raised.
 
_Example_

The following script will separately return the top 2, the 3rd and 4th, and the rows ranked 5th and higher from the rowset `@raceresult` ordered on the `time` column:

    @raceresult = 
      SELECT * 
      FROM ( VALUES ("John", 23, 65.12)
                  , ("Joe", 22, 64.23)
                  , ("Adele", 25, 64.01)
                  , ("Mike", 45, 63.23)
                  , ("Vicky", 27, 65.03)
                  , ("Gunther", 34, 63.98)
                  , ("Daisy", 33, 64.23)
            ) AS res(name,age,time);

    @top2 = SELECT * FROM @raceresult ORDER BY time ASC FETCH 2;

    @pos3to4 = SELECT * FROM @raceresult ORDER BY time ASC OFFSET 2 ROWS FETCH NEXT 2;

    @pos5up = SELECT * FROM @raceresult ORDER BY time ASC OFFSET 4 ROWS;

    OUTPUT @top2
    TO "/output/top2.csv"
    USING Outputters.Csv(outputHeader : true);

    OUTPUT @pos3to4
    TO "/output/pos3to4.csv"
    USING Outputters.Csv(outputHeader : true);

    OUTPUT @pos5up
    TO "/output/pos5up.csv"
    USING Outputters.Csv(outputHeader : true);

Note that since the ordering by time is non-deterministic for two rows where the time values are the same, it is not guaranteed if both the rows for Joe and Daisy will appear in the results or if one of them may appear in both resulting files instead.

#### U-SQL Python Extension additions

##### scikit learn has been added to the default deployment

The scikit has been added by default now and can be used by all Python scripts without needing custom deployment.

#### U-SQL R Extension additions

##### A U-SQL Combiner for R code is now available

A U-SQL combiner is now available for join scenarios like prediction using R scripts. 

_Example_

Assume that we have the following R script in file `RinUSQL_PredictLinearModelwithRawModelUsingCombiner_asDF.R`:

    # do not return readonly columns and make sure that the column names are the same in usql and r scripts
    .libPaths(c(.libPaths(), getwd()))
    if (require(base64enc)==FALSE)
      {
        unzip('base64enc.zip')
        require(base64enc)
      }

    model <- unserialize(base64decode(rightFromUSQL$Model))
    pred = predict(model, leftFromUSQL)
    outputToUSQL=data.frame(Prediction=pred, MPar=rightFromUSQL$MPar, leftFromUSQL) 

This script takes the left and right combiner rowsets as dataframes `leftFromUSQL` and `rightFromUSQL` respectively and then return the resulting dataframe in `outputToUSQL`.

Then we can deploy it and use it with the following script to predict for example Iris species:

    REFERENCE ASSEMBLY [ExtR];

    DEPLOY RESOURCE @"/usqlext/samples/R/RinUSQL_PredictLinearModelwithRawModelUsingCombiner_asDF.R";
    DEPLOY RESOURCE @"/usqlext/samples/R/LMModelsRawStringIris.txt";
    DEPLOY RESOURCE @"/usqlext/samples/R/base64enc.zip";

    DECLARE @OutputFileModelSummary string = @"/usqlext/samples/R/LMModelsRawStringIris.txt";
    DECLARE @IrisData string =  @"/usqlext/samples/R/IrisWithPartitionIds.csv";
    DECLARE @OutputFilePredictions string = @"/my/R/Output/LMPredictionsRawStringCombinerIris.txt";
    DECLARE @PartitionCount int = 5;

    @Models = EXTRACT MPar int, Model string FROM @OutputFileModelSummary USING Extractors.Tsv();

    @ExtendedData =
        EXTRACT Par int,
                SepalLength double,
                SepalWidth double,
                PetalLength double,
                PetalWidth double,
                Species  string
        FROM @IrisData
        USING Extractors.Csv();

    @Predictions = COMBINE @ExtendedData AS Data
                      WITH @Models AS Models
                      ON Data.Par == Models.MPar
                   PRODUCE Prediction double, 
                           MPar int, Par int, 
                           SepalLength double,
                           SepalWidth double,
                           PetalLength double,
                           PetalWidth double,
                           Species  string
                   USING new Extension.R.Combiner(scriptFile:"RinUSQL_PredictLinearModelwithRawModelUsingCombiner_asDF.R", stringsAsFactors:false);

    OUTPUT @Predictions TO @OutputFilePredictions USING Outputters.Tsv();

#### U-SQL Cognitive Library additions

##### Image Tagging Extraction is now also available as an Extractor

In previous versions of the U-SQL cognitive libraries, image tagging "extraction" was only provided as a U-SQL processor. The processor works well on data that is already in a rowset, e.g., operating on images that are stored in a table. However, it can only operate on data that fits into a row, thus limiting the image to the row size limit of at most 4MB.

In the latest release of the cognitive libraries, the image tagging extraction can also be done with a U-SQL extractor. This allows to perform the tag discovery directly on files, thus avoiding the file size limit. As a trade-off, the scale is limited by the scale of operating on file sets (up to 10000s of files) compared to million of rows with the processor (which is still available).

The new `Cognition.Vision.ImageTagsExtractor` extractor will operate on a JPEG file and return a rowset with two columns called `NumObjects` of type `int` that provides the number of detected objects in the image and `Tags` that returns a `SQL.MAP<string, float?>` instance which contains the set of object tags as keys with a floating point value indicating the confidence of each of the tags as values.

It provides the following arguments to specify the relevant output column names with the defaults:

    ImageTagsExtractor(
        string numCol = "NumObjects", 
        string tagCol = "Tags") 

_Example_

The following script will extract the file names of all JPEG files with the extension `.jpg` together with their number of objects and their tags and confidences per file and write it into a file.

    REFERENCE ASSEMBLY ImageTagging;
    
    @tags = 
        EXTRACT FileName string,
                NumObjects int,
                Tags SQL.MAP<string, float?>
        FROM @"/usqlext/samples/cognition/{FileName:*}.jpg"
        USING new Cognition.Vision.ImageTagsExtractor();

    // Merge the Map into a string for outputting
    @tags_serialized =
        SELECT FileName,
               NumObjects,
               String.Join(";", Tags.Select(x => String.Format("{0}:{1}", x.Key, x.Value))) AS Tags
        FROM @tags;

    OUTPUT @tags_serialized TO "/output/tags.csv" USING Outputters.Csv(outputHeader:true);

##### Human face emotion detection is available as an Extractor and an Applier

The older `Cognition.Vision.EmotionAnalyzer` has been replaced with the following two user-defined operators:
 
1.	`Cognition.Vision.EmotionExtractor` Extractor

    For each JPEG file it gets applied to, this U-SQL extractor returns one row per face detected in the file (column `FaceIndex` of type `int`) with additional information about the detected face's bounding box (columns `RectX`, `RectY`, `Width`, `Height` all of type `float`), its recognized emotion (column `Emotion` of type `string`), the confidence value (column `Confidence` of type `float`) and the overall number of faces detected in the image (column `NumFaces` of type `int`).

    It provides the following arguments to specify the relevant output column names with their defaults:

        Cognition.Vision.EmotionExtractor(
            string numCol   = "NumFaces", 
            string indexCol = "FaceIndex", 
            string emtCol   = "Emotion", 
            string confCol  = "Confidence")`

    _Example_

    The following statement creates a rowset of all faces, their bounding boxes, and their emotions with the confidence level, extracted from all JPEG files with the file extension `.jpg` in the specified directory. It uses a different column name for the number of faces in the image column.

        REFERENCE ASSEMBLY ImageEmotion;

        @emotions_from_extractor =
          EXTRACT FileName string, 
                  NumFacesPerImage int, 
                  FaceIndex int, 
                  RectX float, RectY float, Width float, Height float, 
                  Emotion string, 
                  Confidence float
          FROM @"/usqlext/samples/cognition/{FileName:*}.jpg"
          USING new Cognition.Vision.EmotionExtractor(numCol:"NumFacesPerImage");


2.  `Cognition.Vision.EmotionApplier` Applier

    For each JPEG image provided as a byte array in the column with the default name `ImgData`, it returns one row per face detected in the image value with additional information about the detected face's bounding box, its recognized emotion, the confidence value, and the overall number of faces detected in the image (all with the same column names and types as the extractor).

    It provides the following arguments to specify the relevant input and output column names with their defaults:

        public EmotionApplier(
            string imgCol = "ImgData", 
            string numCol = "NumFaces", 
            string indexCol = "FaceIndex", 
            string emtCol = "Emotion", 
            string confCol = "Confidence") 

    _Example_

    The following statement creates a rowset of all faces, their bounding boxes, and their emotions with the confidence level from an input rowset called @imgs that contains the JPEG images in the column `image` and the image name in the column `ImageName`:

        REFERENCE ASSEMBLY ImageEmotion;

        @emotions_from_applier =
          SELECT ImageName,
                 Details.NumFaces,
                 Details.FaceIndex,
                 Details.RectX, Details.RectY, Details.Width, Details.Height,
                 Details.Emotion,
                 Details.Confidence
           FROM @imgs
                CROSS APPLY new Cognition.Vision.EmotionApplier(imgCol:"image") 
                  AS Details(
                       NumFaces int, 
                       FaceIndex int, 
                       RectX float, RectY float, Width float, Height float, 
                       Emotion string, 
                       Confidence float);

###### Human age and gender estimation is now available as Extractor and Applier

Like the emotion detectors, the age and gender estimation (face detection) cognitive functions also detect one or more human faces in an image and get back face rectangles for where in the image the faces are, along with face attributes like age and gender. There are two ways in U-SQL to extract age and gender from the face in an image, depending on whether the image is in a file or inside a rowset. These two UDOs replace the now removed `Cognition.Vision.FaceDetector` processor.

1.	`FaceDetectionExtractor` Extractor

    The `FaceDetectionExtractor` UDO is applied to each image file and it generates one row per face detected in the file. It returns the number of detected faces in the image (column `NumFaces` of type `int`), the current index of the face from all the faces recognized in the image for the returned row (column `FaceIndex` of type `int`) and its face rectangle (columns `RectX`, `RectY`, `Width`, `Height` of type `float`) along with the estimated age (column `FaceAge` of type `int`) and gender (column `FaceGender` of type `string`) for the current face.

    It provides the following arguments to specify the relevant output column names with their defaults:

    *TBD*
       
        public FaceDetectionExtractor(
            )

    _Example_

    The following statement creates a rowset of all faces, their bounding boxes and their ages and genders, extracted from all JPEG files with the file extension `.jpg` in the specified directory. 

        REFERENCE ASSEMBLY FaceSDK;

        @faces_from_extractor =
          EXTRACT FileName string, 
                  NumFaces int, 
                  FaceIndex int, 
                  RectX float, RectY float, Width float, Height float, 
                  FaceAge int, 
                  FaceGender string
          FROM @"/usqlext/samples/cognition/{FileName:*}.jpg"
          USING new Cognition.Vision.FaceDetectionExtractor();

2.	`FaceDetectionApplier` Applier

    The `FaceDetectionApplier` UDO is applied to each image in the byte array column with the default name `ImgData` and it generates one row per face detected in an image. It returns the number of detected faces in the image, current index of the face from all the faces recognized in the image for the returned row, and its face rectangle along with estimated age and gender for the current face index (with the default column names and types as in the `FaceDetectionExtractor` extractor).

    It provides the following arguments to specify the relevant input and output column names with their defaults:

    *TBD*
       
        public FaceDetectionApplier(
            string imgCol = "ImgData", 
            string numCol = "NumFaces", 
            string indexCol = "FaceIndex", 
            ...)

    _Example_

    The following statement creates a rowset of all faces, their bounding boxes, and their ages and gender from an input rowset called @imgs that contains the JPEG images in the column `ImgData` and the image name in the column `ImageName`:

        REFERENCE ASSEMBLY FaceSDK;

        @faces_from_applier = 
          SELECT ImageName,
                 Details.NumFaces,
                 Details.FaceIndex,
                 Details.RectX, Details.RectY, Details.Width, Details.Height,
                 Details.FaceAge,
                 Details.FaceGender
          FROM @imgs
               CROSS APPLY new Cognition.Vision.FaceDetectionApplier() 
               AS Details(
                    NumFaces int, 
                    FaceIndex int, 
                    RectX float, RectY float, Width float, Height float, 
                    FaceAge int, 
                    FaceGender string);

###### Comment regarding Python, R, and Cognitive Libraries

In order to get access to the updates to the Python, R and Cognitive libraries, you have to install or refresh the U-SQL Extension libraries through the Azure Data Lake Analytics portal (currently under the Sample Scripts section). 

## Azure Data Lake Tools for Visual Studio New Capabilities

The following are changes that have been added over the summer and are available in versions 2.3.0000.0 and later.

#### 	ADL Tools for VisualStudio now helps you generate the U-SQL EXTRACT statement

The ADL Tools for VS now helps you to generate the U-SQL `EXTRACT` statement for CSV/TSV-like data formats!

There are several ways to invoke it, either from the file preview page or by right-clicking on the file in your Azure Data Lake Explorer in the tools and choose `Create EXTRACT Script`:

![ADLTools Extract](https://github.com/Azure/AzureDataLake/blob/master/docs/img/ReleaseNotes/right-click-create-extract-script.png) 

Learn more about it at [https://blogs.msdn.microsoft.com/azuredatalake/2017/08/08/create-u-sql-extract-script-automatically/](https://blogs.msdn.microsoft.com/azuredatalake/2017/08/08/create-u-sql-extract-script-automatically/).

#### Python and R code-behind are supported for U-SQL project 

The ADL Tools for VS now support Python and R code-behind files in your U-SQL project.

Right click your `.usql` script in the Solution Explorer to add your Python or R UDOs, and call them with the specified code-behind file name in U-SQL. The ADL Tools will take care of uploading and deploying the file for you!

![ADLTools Python and R codebehind](https://github.com/Azure/AzureDataLake/blob/master/docs/img/ReleaseNotes/Python-R-codebehind.png) 

![ADLTools Calling Python and R codebehind](https://github.com/Azure/AzureDataLake/blob/master/docs/img/ReleaseNotes/Calling-Python-R-codebehind.png)

#### Simplifying debugging shared user code in ADL Tools for VisualStudio
	
You can now register your assemblies with debug info in the ADL Tools for VisualStudio. The debug info contains your assemblies source code and other debug related information, through which you or the users of your assemblies can debug the job failure caused by your assemblies more easily without having to provide the debug information through a separate communication. The registration looks like:

![ADLTools Debug Info Registration](https://github.com/Azure/AzureDataLake/blob/master/docs/img/ReleaseNotes/DebugInfoRegistration.png) 

#### ADL Tools for Visual Studio supports F1 help on U-SQL keywords

F1 help for U-SQL statements is enabled in the ADL Tools for Visual Studio. You can select the U-SQL keyword in your script that you want help for and press F1 to see the reference documentation for the selected keyword.

![ADLTools F1 Help](https://github.com/Azure/AzureDataLake/blob/master/docs/img/ReleaseNotes/F1Help.png) 

#### ADL Tools for Visual Studio allows temporary U-SQL scripts outside of a project

Sometimes you just want to write a script without creating a U-SQL project. Now you can create a temporary U-SQL script to run ad-hoc scripts. Right click your ADLA account in the Server Explorer and choose `Write a U-SQL Query`.
	
![ADLTools temporary U-SQL Script](https://github.com/Azure/AzureDataLake/blob/master/docs/img/ReleaseNotes/TempScript.png) 

##### ADL Tools in Visual Studio highlights all uses of a highlighted U-SQL variable

When selecting a specific U-SQL variable in the U-SQL script editor, all uses (declarations and uses) will be highlighted. 
	
![ADLTools Variable highlighting](https://github.com/Azure/AzureDataLake/blob/master/docs/img/ReleaseNotes/VarHighlighting.png) 

## Azure Portal Updates

#### The job graph in the Portal now indicates if the stage's vertices contain user-defined operators (UDOs)

The job graph in the Portal now indicates if the stage's vertices contain user-defined operators (UDOs) as in this picture:

![ADLPortal UDO marking](https://github.com/Azure/AzureDataLake/blob/master/docs/img/ReleaseNotes/Portal-UDOMarking.png)

## PLEASE NOTE:

In order to get access to the new syntactic features and new tool capabilities on your local environment, you will need to refresh your ADL Tools. You can download the latest version for VS 2013 and 2015 from [here](http://aka.ms/adltoolsvs) or use the Check for Updates menu item mentioned above. If you are using VisualStudio 2017, you currently have to wait for the next VisualStudio 2017 refresh that should occur about every 6 to 8 weeks.

Otherwise you will not be able to use the new features during local run and submission to the cluster will give you syntax warnings for the new language features (you can ignore them and still submit the job).
