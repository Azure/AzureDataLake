# Azure Data Lake Store & Analytics FAQ

# ADL Store


### Q: I am an Owner/Contributor on an ADLS Account, Why don'I t have access to data in the ADLS Account.

This is *by design* Owner/Contributor is about managing the account not data access

Data Access is only available via ACLs. 

NOTE: For ADLS Public Preview, there is a single ACL on the root folder of an ADLS account.


### Q: I am an Owner/Contributor on an ADLS Account, why can't I modify ACLs on the root folder.

The most common reason is the you were not the person who created the ADLS account.

For ADLS Public Preview, the only user who can change the ACL on the root folder is the Owner who created the ADLS account.

By General Availability, all the users listed as Owners for the ADLS account will be able to set the ACL on the root folder.



# ADL Analytics



### Does ADL Analytics include a Machine Learning component?

Currently, no.

This is something we are working on for the future.

