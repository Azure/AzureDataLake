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

(Link to the U-SQL FAQ)[https://github.com/MicrosoftBigData/usql/blob/master/FAQ.md]

