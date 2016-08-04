# Portal Release Notes 2016-08-01
--------------------------
## Securing Files and Folders in Azure Data Lake Store

Previously, we had account level access control for ADLS. In this release, we give users granular control over the access list for individual folders and files. Simply browse to the folder or file you want to set the access control on and click on the "Access" command.

For greater compatilibity with HDFS, we've implemented the full POSIX ACL system. More details can be found in the blog post [here](https://blogs.msdn.microsoft.com/azuredatalake/2016/07/31/introducing-file-and-folder-acls-for-azure-data-lake-store/)

## Securing Catalogs and Databases in Azure Data Lake Analytics

Previously, any user that had access to submit jobs to an ADLA account can access and create anything in the Catalog. In this release, you can control who can access Catalogs and Databases and what permissions they have.

To manage permissions on a Catalog/Database:
1. Open the ADLA account
1. Go to Data Explorer
1. Click on the Catalog or Database
1. Click Manage Access

## Simplified adding a new user to Azure Data Lake Analytics

With the increased in control over Catalog, Database, Folder, and File access, additional steps are required to give access to the dependencies ADLA needs to submit a job. We added an "Add New User" wizard in Azure Data Lake Analytics to automate the process needed to add a new user to submit a job.
To run the wizard, simply click on the "Add User Wizard" button in the ADLA overview.