# Hands-on  Lab: Automating Microsoft Azure Data Lake Store tasks with Azure CLI

# Introduction

In this lab you will learn how to use Azure CLI commands to interact with Azure Data Lake Store (ADLS).

# Getting started


## Prerequisites

Before you can start the lab exercises, you will need various Azure services provisioned for you. Follow the instructions here: [Start](Start.md). 

This process only takes a few minutes. Once your services are configured you can proceed with the lab.


## Setting up your computer

To complete this lab, you will need a computer with the latest version of [Azure CLI (0.10.2)](https://azure.microsoft.com/documentation/articles/xplat-cli-install/) installed.

## More information

For more help and guidance, refer to the following resources.

Tutorials:

* [Get started with Azure Data Lake Store using Azure CLI](https://azure.microsoft.com/en-us/documentation/articles/data-lake-store-get-started-cli/)

# Exercise 0: Launching Azure CLI
In this exercise you will launch Azure CLI, which is needed in order to complete the tasks in this lab.

1. Open a new terminal window.

2. Run the following command to log in to Azure CLI:
 
        azure login
 
3. At the authentication prompt, use the credentials provided by the instructor to log in.

5. To choose a different subscription, run the following:

        azure account set <subscriptionNameOrId>

# Exercise 1: Exploring ADL Store CLI commands
In this exercise, you will explore the ADL Store commands that are available for you to use in Azure CLI.

1. In the terminal, run the following to list the ADL Store accounts that are available to you:
 
        azure datalake store account list
 
2. Run the following to list all of the ADLS Store commands that you can use:
 
        azure datalake store -h
 
3. Run the following to get help on a specific command (in this case, the file creation command):
 
        azure datalake store filesystem create -h


# Exercise 2: Creating and appending to files in Data Lake Store:
In this exercise you will list files within your Data Lake Store account, you'll create a file, and you'll append to that file.

1. List files and directories within the root directory of your Data Lake Store account.
      * Use the command ``azure datalake store filesystem list``.

2. Create a new file within the root directory of your Data Lake Store account.
      * Use the command ``azure datalake store filesystem create``.

3. Add content to to the newly created file. 
      * Use the command ``azure datalake store filesystem addcontent``.

# Exercise 3: Reading data from Data Lake Store:
In this exercise you will read a file, download a file, then delete a file from your Data Lake Store account.

1. Get details of the file that you created in Exercise 2. 
      * Use the command ``azure datalake store filesystem show``.

2. Display the contents of the file.
      * Use the command ``azure datalake store filesystem read``.

2. Download the file.
      * Use the command ``azure datalake store filesystem export``.

2. Delete the file.
      * Use the command ``azure datalake store filesystem delete``.
