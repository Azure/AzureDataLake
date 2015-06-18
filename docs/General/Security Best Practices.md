#Securing Kona and Azure Data Lake Best Practices

##Managing Users##
With the new accounts, we will be leveraging a combination of the [Azure RBAC](https://azure.microsoft.com/en-us/documentation/articles/role-based-access-control-configure/) and Filesystem ACLs to manage access.

Since we separated Kona and Data Lake, the access permissions for these services are managed separately. To manage access to both services using one interface, we recommend using security groups.

We will walk you through how to create a security group and using it to control access to both Kona and Azure Data Lake. Once we've added the security group, we can control access just by simply adding/removing users to the group.

###Setting Up Security Groups in AAD###
We recommend these steps to simplify access management:

1) Navigate to your Azure Active Directory and Select it
 
![](../img/Migration/AAD.png)

2) Select the Directory you're using with your Kona and Azure Data Lake Service. If you're using a Corporate AAD, check with your IT Team on how you can manage/create Security Groups.

![](../img/Migration/AADDirectorySelected.png)

3) Click on "Groups" near the top of the screen

![](../img/Migration/AADGroupSelected.png)

4) Click on the "Add Group" button at the bottom of the screen

![](../img/Migration/AADAddGroup.png)

5) Choose meaningful names that represent the level of permissions the group has.

![](../img/Migration/AADAddGroup2.png)

6) Add users to this group using the "Add User" command at the bottom of the screen

###Managing Users for Kona###
For Kona, we're leveraging [Azure RBAC](https://azure.microsoft.com/en-us/documentation/articles/role-based-access-control-configure/). You can refer to the guide for more information but the basic steps are:

1) Open the Kona service

2) Click on the User icon in the Essentials Bar

![](../img/Migration/AzureRbacResource.png)

3) Click on the Add icon in the top command bar

![](../img/Migration/AzureRbacAdd.png)

4) Click on the "Contributor" role, basically this role restricts users from adding/removing other users. Definition for each role can be found in the [Azure RBAC](https://azure.microsoft.com/en-us/documentation/articles/role-based-access-control-configure/) guide.

![](../img/Migration/AzureRbacSelectRole.png)

5) Search for users or groups that you want to give access (eg. the group created above)

![](../img/Migration/AzureRbacSelectUser.png)

6) Click Select

7) Now this group has access to the Kona Service

###Managing Users for Azure Data Lake###
For Azure Data Lake, we're leveraging 2 components to secure access:
1) Portal and Management operations are controlled by [Azure RBAC](https://azure.microsoft.com/en-us/documentation/articles/role-based-access-control-configure/).
Follow the same steps above but in the context of an Azure Data Lake service to enable access.

2) File System and Data operations are controlled by ACLs set on the Azure Data Lake. At the moment we need to help you add the Group to the Data Lake access control list (in the future this can be done in the API/PS/UI). We need the Object ID of the group to do this.

    To get the Object ID:

    1) Click on the Group to see the Details of the Group

![](../img/Migration/AADGroupDetails.png)

    2) Click on Properties and copy the Object ID

![](../img/Migration/AADGroupProperties.png)

    3) Send us the following information to konaonboard@microsoft.com:
    + Subscription ID
    + Data Lake Account Name
    + Object ID
    + [Any additional information for Data Migration (see above)]

###Adding/Removing Users###
Now that the same security group has been added to both Kona and Azure Data Lake, you can simply add/remove users to the security group to manage access.
