<properties 
   pageTitle="Get started with Data Lake | Azure" 
   description="Use the portal to create a Data Lake account and perform basic operations in the Data Lake" 
   services="data-lake" 
   documentationCenter="" 
   authors="nitinme" 
   manager="paulettm" 
   editor="cgronlun"/>
 
<tags
   ms.service="data-lake"
   ms.devlang="na"
   ms.topic="article"
   ms.tgt_pltfrm="na"
   ms.workload="big-data" 
   ms.date="08/31/2015"
   ms.author="nitinme"/>

# Get started with Azure Data Lake using the portal

> [AZURE.SELECTOR]
- [Portal](azure-data-lake-get-started-portal.md)
- [PowerShell](azure-data-lake-get-started-powershell.md)

Learn how to create an Azure Data Lake account and perform basic operations such as create folders, upload and download data files, delete your account, etc. For more information about Data Lake, see [Azure Data Lake](azure-data-lake-overview.md).

## Prerequisites

Before you begin this tutorial, you must have the following:

- **An Azure subscription**. See [Get Azure free trial](http://azure.microsoft.com/documentation/videos/get-azure-free-trial-for-testing-hadoop-in-hdinsight/).

## Create an Azure Data Lake account

1. Sign on to the new [Azure portal](https://portal.azure.com).
2. Click **NEW**, click **Data + Storage**, and then click **Azure Data Lake**. Read the information in the **Azure Data Lake** blade, and then click **Create** in the bottom left corner of the blade.
3. In the **New Data Lake** blade, provide the following values:
	- **Name**: Enter a name for the Data Lake account.
	- **Pricing**: [ TBD: Complete pricing info ]	
	- **Subscription**: Click the Subscription blade and select the subscription you want to associate with the Data Lake account.
	- **Resource Group**. Select an existing resource group, or click **Create a resource group** to create one. A resource group is a container that holds related resources for an application. For more information, see [Resource Groups in Azure](resource-group-overview.md#resource-groups).
	- **Location**: Select a location. [ TBD: What's the importance of this location. Is the new resource group in the same location ]

4. Select **Pin to Startboard** if you want the Data Lake account to be accessible from the Startboard. This is required for following this tutorial.
5. Click **Create**. 

### HDFS for the Cloud

Azure Data Lake is built from the ground-up as a native Hadoop file system compatible with HDFS, working out-of-the-box with the Hadoop ecosystem including Azure HDInsight, Revolution-R Enterprise, and industry Hadoop distributions like Hortonworks and Cloudera. 

### Unlimited storage, petabyte files, and massive throughput

Azure Data Lake has unbounded scale with no limits to the amount of data that can be stored in a single account and can store very large files of petabyte range. Azure Data Lake is built for running large analytic systems that require massive throughput to query and analyze petabytes of data. It can handle high volumes of small writes at low latency making it optimized for near real-time scenarios like website analytics, Internet of Things (IoT), analytics from sensors, etc.

### Enterprise-ready

Azure Data Lake leverages Azure Active Directory to provide identity and access management for all your data. It also provides data reliability by replicating your data assets to guard against any unexpected failures. This enables enterprises to factor Azure Data Lake in their solutions as an important part of their existing data platform.

### Data in any format

Azure Data Lake is built as a distributed file store allowing you to store relational and non-relational data without transformation or schema definition. This allows you to store all of your data and analyze them in their native format.

## How is Azure Data Lake different from Azure Storage?

<< TODO: Add more info >>

Azure Storage is a generic storage repository that allows you to store data for any use case. In contrast, Azure Data Lake is a storage repository optimized for big data solutions. This includes the capability to stores files that are petabytes in size, provides higher throughput, and has built-in integration with Hadoop.

<< TODO: Include a table comparison >>

| Feature                                | Azure Data Lake | Azure Storage |
|----------------------------------------|-----------------|---------------|
| Maximum file size                      | --              | --            |
| Types of data that can be stored       | --              | --            |
| Cost                                   | --              | --            |
| Compatibility with big data offerings  | --              | --            |

## How do I start using Azure Data Lake

See << TODO: Link to Hero tutorial >>, on how to provision an Azure Data Lake account. Once you have provisioned Azure Data Lake, you can learn how to use big data offerings, such as << TODO: Official name of Kona >>, Azure HDInsight, and Hortonworks HDP with Azure Data Lake to run your big data workloads.

- << TODO: Link to using ADL with Kona >>
- << TODO: Link to using ADL with HDInsight >>
- << TODO: Link to using ADL with HDP >>  
