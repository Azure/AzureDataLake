# Getting Started with Azure Data Lake Store

------------

### To create a new account

To create a new Azure Data Lake Store account, follow these simple steps:

#### Create a new account with PowerShell or the Portal

If you want to create a new Azure Data Lake Store account, you can do so through the Portal or PowerShell:

* [Azure PowerShell](PowerShell/FirstSteps.md)
* [Azure Portal](./AzurePortal/FirstSteps.md)

#### Manage ADLS Users

With the new accounts, we will be leveraging a combination of the [Azure RBAC](https://azure.microsoft.com/en-us/documentation/articles/role-based-access-control-configure/) and Filesystem ACLs to manage access.

To simplify access management, we recommend using security groups.

See this doc: [Securing ADLS Best Practices](https://github.com/MicrosoftBigData/AzureDataLake/blob/master/docs/General/Security%20Best%20Practices.md)

### Get access to an existing ADLS account

Contact your Azure Data Lake Store account owner (the person who created the account).  They can give new users permission to access the ADLS account.

#### Check for existing ADLS accounts

1. Follow the Installation and Initialization procedure in the [First Steps Guide](https://github.com/MicrosoftBigData/AzureDataLake/blob/master/docs/PowerShell/FirstSteps.md)
1.  Open a new PowerShell window. Run the following PowerShell cmdlet to see if you already have access to an Azure Data Lake Store account.

    Get-AzureRmDataLakeStoreAccount


------------

### Useful links

* [Getting Started](GettingStarted.md)
* Tools
    * [Data Lake Store in the Azure Portal](AzurePortal/FirstSteps.md)
    * [Data Lake Store PowerShell](PowerShell/FirstSteps.md)
    * [Data Lake Store .NET SDK](SDK/FirstSteps.md)
