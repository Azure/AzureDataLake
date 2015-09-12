<properties 
   pageTitle="Securing data stored in Azure Data Lake | Azure" 
   description="Learn how to secure data in Azure Data Lake using groups and access control lists" 
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
   ms.date="09/29/2015"
   ms.author="nitinme"/>

# Securing data stored in Azure Data Lake

Securing data in Azure Data Lake is a three-step approach.

1. Start by creating security groups in Azure Active Directory (AAD). These security groups are used to implement role-based access control (RBAC) in Azure Portal. For more information see [Role-based Access Control in Microsoft Azure](role-based-access-control-configure.md).

2. Assign the AAD security groups to the Azure Data Lake account. This controls access to the Data Lake account from the portal and management operations from the portal or APIs.

3. Assign the AAD security groups as access control lists (ACLs) on the Data Lake file system.

This article provides instructions on how to use the Azure portal to perform the above tasks. 

## Prerequisites

Before you begin this tutorial, you must have the following:

- **An Azure subscription**. See [Get Azure free trial](http://azure.microsoft.com/documentation/videos/get-azure-free-trial-for-testing-hadoop-in-hdinsight/).

## Create security groups in Azure Active Directory

For instructions on how to create AAD security groups and how to add users to the group, see [Managing security groups in Azure Active Directory](active-directory-accessmanagement-manage-groups.md).

## Assign the security group to Azure Data Lake accounts

When you assign security groups to Azure Data Lake accounts, you control the management operations on the account using the Azure portal and Azure Resource Manager APIs. In this section, you assign a security group to an Azure Data Lake account. 

1. Open the Data Lake account that you just created. From the left pane, click **Browse**, click **Data Lake**, and then from the Data Lake blade, click the account name to which you want to assign a security group.

2. In your Data Lake account blade, click the user icon.

	![Assign security group to Azure Data Lake account](./media/azure-data-lake-secure-data/adl.select.user.icon.png "Assign security group to Azure Data Lake account")

3. The **User** blade by default lists **Subscription admins** group as an owner. 

	![Add users and roles](./media/azure-data-lake-secure-data/adl.add.group.roles.png "Add users and roles")
 
	There are two ways to add a group and assign relevant roles.

	* Add a group to the account and then assign a role, or
	* Add a role and then assign groups to role.

	In this section, we look at the first approach, adding a group and then assigning roles. You can perform similar steps to first select a role and then assign groups to that role.
	
4. In the **Users** blade, click **Add** to open the **Add access** blade. In the **Add access** blade, click **Select a role**, and then select a role for the user group.

	 ![Add a role for the user](./media/azure-data-lake-secure-data/adl.add.user.1.png "Add a role for the user")

5. In the **Add access** blade, click **Add users** to open the **Add users** blade. In this blade, look for the security group you created earlier in Azure Active Directory. If you have a lot of groups to search from, use the text box at the top to filter on the group name.

	![Add a security group](./media/azure-data-lake-secure-data/adl.add.user.2.png "Add a security group")

	If you want to add a group/user that is not listed, you can invite them by using the **Invite** icon and specifying the e-mail address for the user/group.

6. Click **Select** and then click **OK**. You should see the security group added as shown below.

	![Security group added](./media/azure-data-lake-secure-data/adl.add.user.3.png "Security group added")

7. Your security group now has access to the Azure Data Lake account. If you want to provide access to specific users, you can add them to the security group. Similarly, if you want to revoke access for a user, you can remove them from the security group. You can also assign multiple security groups. 

## Assign security group as ACLs to the Azure Data Lake file system

By assigning security groups to the Azure Data Lake file system, you set access control on the data stored in Azure Data Lake. In the current release, you can only provide ACLs only at the root node of your file system.

1. In your Data Lake account blade, click **Data Explorer**.

	![Create directories in Data Lake account](./media/azure-data-lake-secure-data/adl.start.data.explorer.png "Create directories in Data Lake account")

2. In the **Data Explorer** blade, click the root of your account, and then in your account blade, click the **Access** icon.

	![Set ACLs on Data Lake file system](./media/azure-data-lake-secure-data/adl.acl.1.png "Set ACLs on Data Lake file system")

3. The **Access** blade lists the standard access (read-only) and custom access already assigned to the root. Click the **Add** icon to add custom-level ACLs.

	![List standard and custom access](./media/azure-data-lake-secure-data/adl.acl.2.png "List standard and custom access")

4. Click the **Add** icon to open the **Add Custom Access** blade. In this blade, click **Select User or Group**, and then in **Select User or Group** blade, look for the security group you created earlier in Azure Active Directory. If you have a lot of groups to search from, use the text box at the top to filter on the group name. Click the group you want to add and then click **Select**.

	![Add a group](./media/azure-data-lake-secure-data/adl.acl.3.png "Add a group")

5. Click **Select Permissions**, select the permissions you want to assign to that group, and then click **OK**.

	![Assign permissions to group](./media/azure-data-lake-secure-data/adl.acl.4.png "Assign permissions to group")

6. in the **Add Custom Access** blade, click **OK**. The newly added group, with the associated permissions, will now be listed in the **Access** blade.

	![Assign permissions to group](./media/azure-data-lake-secure-data/adl.acl.5.png "Assign permissions to group")

7. If required, you can also modify the access permissions after you have added the group. Just clear or select the check box for each permission type (Read, Write, Execute) based on whether you want to remove or assign that permission to the security group. Click **Save** to save the changes, or **Discard** to undo the changes.

## Remove security groups for an Azure Data Lake account

1. In your Data Lake account blade, click the user icon.

	![Assign security group to Azure Data Lake account](./media/azure-data-lake-secure-data/adl.select.user.icon.png "Assign security group to Azure Data Lake account")

2. In the **Users** blade click the security group you want to remove.

	![Security group to remove](./media/azure-data-lake-secure-data/adl.add.user.3.png "Security group to remove")

3. In the blade for the security group, click **Remove**.

	![Security group removed](./media/azure-data-lake-secure-data/adl.remove.group.png "Security group removed")

## Remove security group ACLs from Azure Data Lake file system

1. In your Data Lake account blade, click **Data Explorer**.

	![Create directories in Data Lake account](./media/azure-data-lake-secure-data/adl.start.data.explorer.png "Create directories in Data Lake account")

2. In the **Data Explorer** blade, click the root of your account, and then in your account blade, click the **Access** icon.

	![Set ACLs on Data Lake file system](./media/azure-data-lake-secure-data/adl.acl.1.png "Set ACLs on Data Lake file system")

3. In the **Access** blade, from the **Custom Access** section, click the security group you want to remove. In the **Custom Access** blade, click **Remove** and then click **OK**.

	![Assign permissions to group](./media/azure-data-lake-secure-data/adl.remove.acl.png "Assign permissions to group")


## See also

[ TBD: Add links ]
