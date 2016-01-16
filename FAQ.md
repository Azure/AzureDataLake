# Azure Data Lake Store & Analytics FAQ

# General Questions

### Q: When will ADL Store and ADL Analytics be "Generally Available"?

In CY2016.

### Q: Is Azure Data Lake (Store, Analytics, or HDInsight) going to offered as an "on-premises" product?

We currently have no plans to offer on-premises versions of these services. 

### Q: Does Azure Data Lake use TLS(SSL)?

Yes. Azure Data Lake Store & Analytics both use HTTPS with TLS (SSL) for all network communication.

### Q: What network ports do ADL Store and ADL Analytics use?

Because both ADL Store and ADL Analytics use HTTPS for all communication. The only port they use is port 443.

### Q: What are the  restrictions on the name of an ADL Store & ADL Analytics account?

* Only lowercase alphabetical and numbers are allowed
* Whitespace is not allowed
* Punctuation such as . , ; : are not allowed
* No special characters such as - + _
* Size: The names must be between 3 to 24 characters in length.


### Q: I am an Owner/Contributor on an ADLS Account, Why don'I t have access to data in the ADLS Account.

This is *by design* Owner/Contributor is about managing the account not data access

Access to data is controlled solely through filesystem ACLs. 

NOTE: For ADLS Public Preview, there is a single ACL on the root folder of an ADLS account.

### Q: I am an Owner/Contributor on an ADLS Account, why can't I modify ACLs on the root folder.

For ADLS Public Preview, the only user who can change the ACL on the root folder is the Owner who created the ADLS account.

By ADLS General Availability, all the users listed as Owners for the ADLS account will be able to set the ACL on the root folder.

### Q: Can I set an ACL on a subfolder or file?

For ADLS Public Preview, there is a single ACL on the root folder of an ADLS account.

By  ADLS General Availability , ACLs will be available on any folder and on any file.

### Q: Do I have to pay for Visual Studio to use Azure Data Lake?

No. Azure Data Lake Tools for Visual Studio works with Visual Studio Community Editions which are free.


# ADL Analytics

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

