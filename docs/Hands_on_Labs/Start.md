Getting Started with the Azure Data Lake Hands-on Labs
======================================================

Documents
---------

All the hands-on-lab documents, including this one, are located at
http://aka.ms/AzureDataLakeHandsOnLabs.

Preparing for the Labs
----------------------

The Azure Data Lake hands-on labs require you to have various Azure services
provisioned. Some labs require a single service, while others require three or
four.

List of Azure Services need to Complete all the Hands-On-Labs
-------------------------------------------------------------

-   Azure Data Lake Store

-   Azure Data Lake Analytics

-   Azure Data Lake Storage Account

-   Azure Data Factory


Manual Steps for Creating the Required Services in the Azure Portal
-------------------------------------------------------------------

Below are the manual steps required to create the services from within the
portal

### Before you begin:

-   Pick an Azure subscription you want to use. The services can be in separate
    subscriptions.

-   Pick a Location (Region) to use. All the Data Lake services should be in the
    same location. The Azure Data Factory account can be in any region

-   You can choose any Azure Resource Group that you want. It won’t affect the
    how the labs are done. We recommend using one like ....

    "username_adl_hol"

### Step 1: Login to the Azure Portal

Go to http://portal.azure.com

### Step 2: Create Azure Data Lake Analytics & Store accounts

-   Click **New \> Data + Analytics \> Data Lake Analytics**

-   Select a **Name **for the account such as “contosoadla”

-   Select the **Subscription**, **Resource Group**, and **Location **you want
    to use

-   Click **Data Lake Store**

-   Click **Create a New Data Lake Store**

-   The **Name** of the Store will already be filled in, but you can change it
    so if you want to something like “contosoadls”

-   Click **OK**

-   Enable **Pin to Dashboard**

-   Click **Create**

### Step 3: Create a Windows Storage Account

-   Click **New \> Data + Storage \> Storage Account**

-   Select a **Name **for the account such as “contosowasb”

-   Select the **Subscription**, **Resource Group**, and **Location** you want
    to use

-   Enable **Pin to Dashboard**

-   Click **Create**

### Step 3: Create a Azure Data Factory Account

-   Click **New \> Data + Analytics \> Data Factory**

-   Select a **Name** for the account such as “contosoadf”

-   Select the **Subscription**, **Resource Group**, and Location you want to
    use

-   Enable **Pin to Dashboard**

-   Click **Create**
