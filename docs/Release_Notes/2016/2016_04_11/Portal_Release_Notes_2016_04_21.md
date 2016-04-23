##Track and audit events in your Azure Data Lake Store and Analytic accounts
One of the broad questions that administrators of accounts want to understand is “what is happening in accounts I manage?” Recently, we added the ability for admins to get Diagnostic Logs for Azure Data Lake Store (ADLS) and Azure Data Lake Analytics (ADLA). Administrators can use the Diagnostic Logs to get an understanding of the operations that are happening in ADLA and ADLS accounts. To enable this feature, go to "Settings" -> "Diagnostic Settings".
![](/docs/img/Portal/DiagnosticSettings.png "")

##Easily find and get the latest version of Data Lake tools
See a list of all the tools for Azure Data Lake from Microsoft with the new Tools blade. You can find it in "Settings" -> "Tools".

![](/docs/img/Portal/Tools.png "")

##Browse additional catalog types from the web
You can now browse through Procedures, Credentials, Views, and External Data Sources in the Azure Data Lake Analytics Data Explorer.

![](/docs/img/Portal/ADLAObjects.png "")	

##Quickly find a specific job using its Job ID
Job List filtering now supports filtering by Job ID. 

![](/docs/img/Portal/JobFilter.png "")

##At a glance see the size of input and output files
Understand the sizes of input and output files in the job graph.

![JobGraphSize](/docs/img/Portal/JobGraphSize.png "Job Graph Size")	

##Automate deployment of accounts using generated Azure Resource Manager (ARM) templates
Azure Resources can be deployed with Azure Resource Manager (ARM) templates (https://azure.microsoft.com/en-us/documentation/articles/resource-group-template-deploy/). You can use the Azure Portal to generate the ARM template used to create Azure Data Lake Store or Azure Data Lake Analytics accounts by clicking on "Automation options" during account creation.

![automation](/docs/img/Portal/AutomationOptions.png "Automation Options")
