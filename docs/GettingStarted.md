# Getting Started with Azure Data Lake

------------

### To create a new account

To create a new Azure Data Lake account, follow these simple steps:

#### Email your TAP Buddy your Azure subscription ID
Send your Subscription ID to your TAP Buddy to receive access to Azure Data Lake. This step will take up to 1 business day to be completed.

#### Create a new account with PowerShell or the Portal

If you want to create a new Azure Data Lake account, you can do so through the Portal or PowerShell:

* [Azure PowerShell](PowerShell/FirstSteps.md)
* [Azure Portal](./AzurePortal/FirstSteps.md)

#### Manage ADL Users
With the new accounts, we will be leveraging a combination of the [Azure RBAC](https://azure.microsoft.com/en-us/documentation/articles/role-based-access-control-configure/) and Filesystem ACLs to manage access.

To simplify access management, we recommend using security groups.

See this doc: [Securing ADL Best Practices](https://github.com/MicrosoftBigData/AzureDataLake/blob/master/docs/General/Security%20Best%20Practices.md)

### Get access to an existing account

Contact your Data Lake Account Owner (the person who created the account).  They can give new users permission to access the ADL account.

#### Check for existing accounts

1. Follow the Installation and Initialization procedure in the [First Steps Guide](https://github.com/MicrosoftBigData/AzureDataLake/blob/master/docs/PowerShell/FirstSteps.md)
2.  Open a new PowerShell window. Run the following PowerShell cmdlets to see if you already have access to a Data Lake account.

    Get-AzureDataLakeAccount


------------

### Useful links

* [Getting Started](GettingStarted.md)
* Tools
    * [Azure Portal](AzurePortal/FirstSteps.md)
    * [Data Lake PowerShell](PowerShell/FirstSteps.md)
    * [SDK](SDK/FirstSteps.md)
