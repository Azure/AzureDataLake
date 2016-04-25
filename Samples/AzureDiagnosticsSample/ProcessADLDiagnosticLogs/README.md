 

 

First Register the Assemblies
-----------------------------

 

-   Right-click on the **AzureDiagnostics** project and select **Register
    Assembly**,

-   Under **Managed Assemblies** select **Newtonsoft.Json**

-   Click **Submit**

-   Right-click on AzureDiagnosticsExtractors project and select **Reference
    Assembly**,

-   Click **Submit**

 

Once you do this you should see three items listed in your Database’s Assemblies
folder

-   AzureDiagnostics

-   AzureDiagnosticsExtractors

-   Newtonsoft.Json

 

Reading a log from a U-SQL Script
---------------------------------

 

To use the Extractors, you U-SQL script will need to have these statements and
the beginning:

 

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
REFERENCE ASSEMBLY AzureDiagnostics;
REFERENCE ASSEMBLY AzureDiagnosticsExtractors;
REFERENCE ASSEMBLY [Newtonsoft.Json];
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

 

And in order for those REFERENCE ASSEMBLY commands to work the assemblies must
be registered in a U-SQL database.

 

Then call the extractors to get the data:

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
@adls_rows =
    EXTRACT Time DateTime,
            ResourceId string,
            Category string,
            OperationName string,
            ResultType string,
            ResultSignature string,
            CorrelationId string,
            Identity string,
            ADLS_StreamName string
    FROM @"/Input/ADLS_PT1H.json"
    USING new AzureDiagnosticsExtractors.DataLakeStoreExtractor();

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
@adla_rows =
    EXTRACT Time DateTime,
            ResourceId string,
            Category string,
            OperationName string,
            ResultType string,
            ResultSignature string,
            CorrelationId string,
            Identity string,
            ADLA_JobId string,
            ADLA_JobName string,
            ADLA_JobRuntimeName string,
            ADLA_StartTime DateTime?,
            ADLA_SubmitTime DateTime?,
            ADLA_EndTime DateTime?
            
    FROM @"/Input/ADLA_PT1H.json"
    USING new AzureDiagnosticsExtractors.DataLakeAnalyticsExtractor();
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

 

 
