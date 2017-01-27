# Azure Data Lake Analytics FAQ

### Does ADL Analytics include a Machine Learning component?

Currently, no.

This is something we are working on for the future.

### How can I use the PowerShell cmdlet Submit-AzureRmDataLakeAnalyticsJob to submit C# Code with my U-SQL Script?

Submit-AzureRmDataLakeAnalyticsJob can't simply submit a .cs (C# Source code file) along with a U-SQL script.

To achieve the same effect:
1.	Compile the .cs file into a dll using Visual Studio or the C# compiler
2.	Upload the dll file into a location of your choosing in the ADL Store account.
3.	Submit a U-SQL script that contains this at the top
    
    DROP ASSEMBLY IF EXISTS myassembly;
    CREATE ASSEMBLY myassembly FROM "/myfolder/code.dll";
    REFERENCE myassembly; 

### Where can I find the U-SQL FAQ?

https://github.com/MicrosoftBigData/usql/blob/master/FAQ.md

### How do a provision a new user into my ADL Analytics account?

Here all the tasks to provision a user:
- ADLA: Add them to the “Data Lake Analytics Developer” role
- ADLS: Add them to the “Reader” role
- ADLS: Give them RWX access on the root path “/”

This ensures that a user:
- Can submit jobs into the ADLA account
- Can view the ADLA Azure Portal
- Can view the ADLS Azure Portal
- Can read and write data in the ADLA account

### Can I submit a U-SQL Job from the Command Line?

Yes, We have a couple of options

* **Azure PowerShell** https://docs.microsoft.com/en-us/azure/data-lake-analytics/data-lake-analytics-get-started-powershell
* **Azure CLI**  https://docs.microsoft.com/en-us/azure/data-lake-analytics/data-lake-analytics-get-started-cli
* **Data Lake Analytics .NET SDK** https://docs.microsoft.com/en-us/azure/data-lake-analytics/data-lake-analytics-get-started-net-sdk


