# Azure Data Lake Store & Analytics FAQ

# General Questions

### Q: When will ADL Store and ADL Analytics be "Generally Available"?

In CY2016.

### Q: What network ports do ADL Store adn and  Analytics use

Both ADL Store and ADL Analytics use HTTPS for all communication. The only port they use is port 443.

# ADL Store

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



# ADL Analytics

### Does ADL Analytics include a Machine Learning component?

Currently, no.

This is something we are working on for the future.

### Can I read/write files using code that is running within a U-SQL User-defined Operator (UDO)?

No. This is disallowed.

# U-SQL

###

### Q: Can I get a single value back from a rowset?

You may be trying to do something like ehat is shown below - and expect MaxDate to be a single scalar value

    @MaxSize = SELECT MAX(Size) AS MaxSize FROM input;

U-SQL does not support retrieving a single scalar value from a rowset. You can only retrieve another rowset.

Thus, @MaxSize is not a value of type long, but instead is a rowset with a single row and a single column of type long called MaxSize.







