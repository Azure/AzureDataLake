# Getting Started with Azure Data Lake Store

------------

### To create a new account

You can create a new Azure Data Lake Store account through the Portal or PowerShell

* [Azure PowerShell](PowerShell/FirstSteps.md)
* [Azure Portal](./AzurePortal/FirstSteps.md)

#### Manage ADLS Users with Role-based Access Control

We will leverage two features to control access:
* [Azure RBAC](https://azure.microsoft.com/en-us/documentation/articles/role-based-access-control-configure/) 
* ADLS Filesystem ACLs

To simplify access management, we recommend using Azure Active Directory security groups.

Please read [Securing ADLS Best Practices](https://github.com/MicrosoftBigData/AzureDataLake/blob/master/docs/General/Security%20Best%20Practices.md)

### Get access to an existing ADLS account

If you want access to an ADLS account, contact the the person who created the account (The "Owner"). Only they can permissions to new users.

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
