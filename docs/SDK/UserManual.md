# User Manual: Azure .NET SDK with Data Lake

This guide assumes you have previously followed the steps in the main [Getting Started guide](../GettingStarted.md) and the SDK [First Steps guide](FirstSteps.md).

------------

### User Manual

There are many classes available as part of the .NET SDK for Data Lake.

Here is a list of the interfaces, their properties, and their function headers:

#### Microsoft.Azure.Management.DataLake.IDataLakeManagementClient
    // Summary:
    //     Gets the API version.
    string ApiVersion { get; }


    //
    // Summary:
    //     Gets the URI used as the base for all cloud service requests.
    Uri BaseUri { get; }


    //
    // Summary:
    //     Gets subscription credentials which uniquely identify Microsoft Azure subscription.
    //     The subscription ID forms part of the URI for every service call.
    SubscriptionCloudCredentials Credentials { get; }


    //
    // Summary:
    //     Operations for managing DataLake accounts
    IDataLakeAccountOperations DataLakeAccount { get; }


    //
    // Summary:
    //     Gets or sets the initial timeout for Long Running Operations.
    int LongRunningOperationInitialTimeout { get; set; }


    //
    // Summary:
    //     Gets or sets the retry timeout for Long Running Operations.
    int LongRunningOperationRetryTimeout { get; set; }



    // Summary:
    //     The Get Operation Status operation returns the status of the specified operation.
    //     After calling an asynchronous operation, you can call Get Operation Status
    //     to determine whether the operation has succeeded, failed, or is still in
    //     progress.
    //
    // Parameters:
    //   azureAsyncOperation:
    //     Location value returned by the Begin operation.
    //
    //   cancellationToken:
    //     Cancellation token.
    //
    // Returns:
    //     The response body contains the status of the specified asynchronous operation,
    //     indicating whether it has succeeded, is inprogress, or has failed. Note that
    //     this status is distinct from the HTTP status code returned for the Get Operation
    //     Status operation itself. If the asynchronous operation succeeded, the response
    //     body includes the HTTP status code for the successful request. If the asynchronous
    //     operation failed, the response body includes the HTTP status code for the
    //     failed request and error information regarding the failure.
    Task<Microsoft.Azure.Management.DataLake.Models.AzureAsyncOperationResponse> GetLongRunningOperationStatusAsync(string azureAsyncOperation, CancellationToken cancellationToken);


#### Microsoft.Azure.Management.DataLake.IDataLakeAccountOperations
    // Summary:
    //     Creates the specified DataLake DataLakeAccount.This supplies the user with
    //     computation services for BigData workloads
    //
    // Parameters:
    //   resourceGroupName:
    //     The name of the resource group the account will be associated with.
    //
    //   parameters:
    //     Parameters supplied to the create DataLake account operation.
    //
    //   cancellationToken:
    //     Cancellation token.
    //
    // Returns:
    //     The response body contains the status of the specified asynchronous operation,
    //     indicating whether it has succeeded, is inprogress, or has failed. Note that
    //     this status is distinct from the HTTP status code returned for the Get Operation
    //     Status operation itself. If the asynchronous operation succeeded, the response
    //     body includes the HTTP status code for the successful request. If the asynchronous
    //     operation failed, the response body includes the HTTP status code for the
    //     failed request and error information regarding the failure.
    Task<AzureAsyncOperationResponse> BeginCreateAsync(string resourceGroupName, DataLakeAccountCreateOrUpdateParameters parameters, CancellationToken cancellationToken);


    //
    // Summary:
    //     Deletes the DataLake DataLakeAccount object specified by the account name.
    //
    // Parameters:
    //   resourceGroupName:
    //     The name of the resource group.
    //
    //   accountName:
    //     The name of the account to delete
    //
    //   cancellationToken:
    //     Cancellation token.
    //
    // Returns:
    //     The response body contains the status of the specified asynchronous operation,
    //     indicating whether it has succeeded, is inprogress, or has failed. Note that
    //     this status is distinct from the HTTP status code returned for the Get Operation
    //     Status operation itself. If the asynchronous operation succeeded, the response
    //     body includes the HTTP status code for the successful request. If the asynchronous
    //     operation failed, the response body includes the HTTP status code for the
    //     failed request and error information regarding the failure.
    Task<AzureAsyncOperationResponse> BeginDeleteAsync(string resourceGroupName, string accountName, CancellationToken cancellationToken);


    //
    // Summary:
    //     Updates the DataLake account object specified by the accountName with the
    //     contents of the account object.
    //
    // Parameters:
    //   resourceGroupName:
    //     The name of the resource group.
    //
    //   parameters:
    //     Parameters supplied to the update DataLake account operation.
    //
    //   cancellationToken:
    //     Cancellation token.
    //
    // Returns:
    //     The response body contains the status of the specified asynchronous operation,
    //     indicating whether it has succeeded, is inprogress, or has failed. Note that
    //     this status is distinct from the HTTP status code returned for the Get Operation
    //     Status operation itself. If the asynchronous operation succeeded, the response
    //     body includes the HTTP status code for the successful request. If the asynchronous
    //     operation failed, the response body includes the HTTP status code for the
    //     failed request and error information regarding the failure.
    Task<AzureAsyncOperationResponse> BeginUpdateAsync(string resourceGroupName, DataLakeAccountCreateOrUpdateParameters parameters, CancellationToken cancellationToken);


    //
    // Summary:
    //     Creates the specified DataLake DataLakeAccount.
    //
    // Parameters:
    //   resourceGroupName:
    //     The name of the resource group.
    //
    //   parameters:
    //     Parameters supplied to the create DataLake account operation.
    //
    //   cancellationToken:
    //     Cancellation token.
    //
    // Returns:
    //     The response body contains the status of the specified asynchronous operation,
    //     indicating whether it has succeeded, is inprogress, or has failed. Note that
    //     this status is distinct from the HTTP status code returned for the Get Operation
    //     Status operation itself. If the asynchronous operation succeeded, the response
    //     body includes the HTTP status code for the successful request. If the asynchronous
    //     operation failed, the response body includes the HTTP status code for the
    //     failed request and error information regarding the failure.
    Task<AzureAsyncOperationResponse> CreateAsync(string resourceGroupName, DataLakeAccountCreateOrUpdateParameters parameters, CancellationToken cancellationToken);


    //
    // Summary:
    //     Deletes the DataLake DataLakeAccount object specified by the account name.
    //
    // Parameters:
    //   resourceGroupName:
    //     The name of the resource group.
    //
    //   accountName:
    //     The name of the account to delete
    //
    //   cancellationToken:
    //     Cancellation token.
    //
    // Returns:
    //     The response body contains the status of the specified asynchronous operation,
    //     indicating whether it has succeeded, is inprogress, or has failed. Note that
    //     this status is distinct from the HTTP status code returned for the Get Operation
    //     Status operation itself. If the asynchronous operation succeeded, the response
    //     body includes the HTTP status code for the successful request. If the asynchronous
    //     operation failed, the response body includes the HTTP status code for the
    //     failed request and error information regarding the failure.
    Task<AzureAsyncOperationResponse> DeleteAsync(string resourceGroupName, string accountName, CancellationToken cancellationToken);


    //
    // Summary:
    //     Gets the DataLake DataLakeAccount object specified by the account name.
    //
    // Parameters:
    //   resourceGroupName:
    //     The name of the resource group.
    //
    //   accountName:
    //     The name of the account to retrieve
    //
    //   cancellationToken:
    //     Cancellation token.
    //
    // Returns:
    //     DataLake DataLakeAccount information.
    Task<DataLakeAccountGetResponse> GetAsync(string resourceGroupName, string accountName, CancellationToken cancellationToken);


    //
    //
    // Parameters:
    //   operationStatusLink:
    //     Location value returned by the Begin operation.
    //
    //   cancellationToken:
    //     Cancellation token.
    //
    // Returns:
    //     The Create DataLakeAccount operation response.
    Task<DataLakeAccountCreateUpdateOrDeleteResponse> GetCreateOrUpdateStatusAsync(string operationStatusLink, CancellationToken cancellationToken);


    //
    //
    // Parameters:
    //   operationStatusLink:
    //     Location value returned by the Begin operation.
    //
    //   cancellationToken:
    //     Cancellation token.
    //
    // Returns:
    //     The Create DataLakeAccount operation response.
    Task<DataLakeAccountCreateUpdateOrDeleteResponse> GetDeleteStatusAsync(string operationStatusLink, CancellationToken cancellationToken);


    //
    // Summary:
    //     Lists the DataLake DataLakeAccount objects within the subscription or within
    //     a specific resource group.
    //
    // Parameters:
    //   resourceGroupName:
    //     The name of the resource group.
    //
    //   parameters:
    //     Query parameters. If null is passed returns all DataLakeAccount items.
    //
    //   cancellationToken:
    //     Cancellation token.
    //
    // Returns:
    //     DataLake DataLakeAccount list information.
    Task<DataLakeAccountListResponse> ListAsync(string resourceGroupName, DataLakeAccountListParameters parameters, CancellationToken cancellationToken);


    //
    // Summary:
    //     Gets the next page of the DataLake DataLakeAccount objects within the subscription
    //     or within a specific resource group with the link to the next page, if any.
    //
    // Parameters:
    //   nextLink:
    //     The url to the next Kona account page.
    //
    //   cancellationToken:
    //     Cancellation token.
    //
    // Returns:
    //     DataLake DataLakeAccount list information.
    Task<DataLakeAccountListResponse> ListNextAsync(string nextLink, CancellationToken cancellationToken);


    //
    // Summary:
    //     Updates the DataLake account object specified by the accountName with the
    //     contents of the account object.
    //
    // Parameters:
    //   resourceGroupName:
    //     The name of the resource group.
    //
    //   parameters:
    //     Parameters supplied to the update DataLake account operation.
    //
    //   cancellationToken:
    //     Cancellation token.
    //
    // Returns:
    //     The response body contains the status of the specified asynchronous operation,
    //     indicating whether it has succeeded, is inprogress, or has failed. Note that
    //     this status is distinct from the HTTP status code returned for the Get Operation
    //     Status operation itself. If the asynchronous operation succeeded, the response
    //     body includes the HTTP status code for the successful request. If the asynchronous
    //     operation failed, the response body includes the HTTP status code for the
    //     failed request and error information regarding the failure.
    Task<AzureAsyncOperationResponse> UpdateAsync(string resourceGroupName, DataLakeAccountCreateOrUpdateParameters parameters, CancellationToken cancellationToken);


#### Microsoft.Azure.Management.DataLakeFileSystem.IDataLakeFileSystemManagementClient
    // Summary:
    //     Gets the API version.
    string ApiVersion { get; }


    //
    // Summary:
    //     Gets subscription credentials which uniquely identify Microsoft Azure subscription.
    //     The subscription ID forms part of the URI for every service call.
    SubscriptionCloudCredentials Credentials { get; }


    //
    // Summary:
    //     Gets the URI used as the base for all cloud service requests.
    string DataLakeServiceUri { get; set; }


    //
    // Summary:
    //     Operations for managing the DataLake filesystem
    IFileSystemOperations FileSystem { get; }


    //
    // Summary:
    //     Gets or sets the initial timeout for Long Running Operations.
    int LongRunningOperationInitialTimeout { get; set; }


    //
    // Summary:
    //     Gets or sets the retry timeout for Long Running Operations.
    int LongRunningOperationRetryTimeout { get; set; }


#### Microsoft.Azure.Management.DataLakeFileSystem.IFileSystemOperations
    // Summary:
    //     Appends to the file specified in the link that was returned from BeginAppend.
    //
    // Parameters:
    //   fileAppendRequestLink:
    //     The link to the file to append to including all required parameters.
    //
    //   streamContents:
    //     The file contents to include when appending to the file.
    //
    //   cancellationToken:
    //     Cancellation token.
    //
    // Returns:
    //     A standard service response including an HTTP status code and request ID.
    Task<Microsoft.Azure.AzureOperationResponse> AppendAsync(string fileAppendRequestLink, Stream streamContents, CancellationToken cancellationToken);


    //
    // Summary:
    //     Initiates a file append request, resulting in a return of the data node location
    //     that will service the request.
    //
    // Parameters:
    //   filePath:
    //     The path to the file to create.
    //
    //   accountName:
    //     The name of the account to use
    //
    //   bufferSize:
    //     The optional buffer size to use when appending data
    //
    //   cancellationToken:
    //     Cancellation token.
    //
    // Returns:
    //     The data from the file requested.
    Task<FileCreateOpenAndAppendResponse> BeginAppendAsync(string filePath, string accountName, int? bufferSize, CancellationToken cancellationToken);


    //
    // Summary:
    //     Initiates a file creation request, resulting in a return of the data node
    //     location that will service the request.
    //
    // Parameters:
    //   filePath:
    //     The path to the file to create.
    //
    //   accountName:
    //     The name of the account to use
    //
    //   parameters:
    //     The optional parameters to use when creating the file
    //
    //   cancellationToken:
    //     Cancellation token.
    //
    // Returns:
    //     The data from the file requested.
    Task<FileCreateOpenAndAppendResponse> BeginCreateAsync(string filePath, string accountName, FileCreateParameters parameters, CancellationToken cancellationToken);


    //
    // Summary:
    //     Gets the data associated with the file handle requested.
    //
    // Parameters:
    //   filePath:
    //     The path to the file to open.
    //
    //   accountName:
    //     The name of the account to retrieve
    //
    //   parameters:
    //     The optional parameters to pass to the open operation
    //
    //   cancellationToken:
    //     Cancellation token.
    //
    // Returns:
    //     The data from the file requested.
    Task<FileCreateOpenAndAppendResponse> BeginOpenAsync(string filePath, string accountName, FileOpenParameters parameters, CancellationToken cancellationToken);


    //
    // Summary:
    //     Checks if the specified access is available at the given path.
    //
    // Parameters:
    //   path:
    //     The path to the file or folder to check access for.
    //
    //   accountName:
    //     The name of the account to use
    //
    //   fsAction:
    //     File system operation read/write/execute in string form, matching regex pattern
    //     '[rwx-]{3}'
    //
    //   cancellationToken:
    //     Cancellation token.
    //
    // Returns:
    //     A standard service response including an HTTP status code and request ID.
    Task<Microsoft.Azure.AzureOperationResponse> CheckAccessAsync(string path, string accountName, string fsAction, CancellationToken cancellationToken);


    //
    // Summary:
    //     Concatenates the list of files into the target file.
    //
    // Parameters:
    //   destinationPath:
    //     The path to the destination file resulting from the concatenation.
    //
    //   accountName:
    //     The name of the account to use
    //
    //   paths:
    //     A list of comma seperated absolute FileSystem paths without scheme and authority
    //
    //   cancellationToken:
    //     Cancellation token.
    //
    // Returns:
    //     A standard service response including an HTTP status code and request ID.
    Task<Microsoft.Azure.AzureOperationResponse> ConcatAsync(string destinationPath, string accountName, string paths, CancellationToken cancellationToken);


    //
    // Summary:
    //     Creates the file specified in the link that was returned from BeginCreate.
    //
    // Parameters:
    //   fileCreateRequestLink:
    //     The link to the file to create including all required parameters.
    //
    //   streamContents:
    //     The file contents to include when creating the file. This parameter is required,
    //     however it can be an empty stream. Just not null.
    //
    //   cancellationToken:
    //     Cancellation token.
    //
    // Returns:
    //     The data from the file requested.
    Task<FileCreateOpenAndAppendResponse> CreateAsync(string fileCreateRequestLink, Stream streamContents, CancellationToken cancellationToken);


    //
    // Summary:
    //     Creates a symbolic link.
    //
    // Parameters:
    //   sourcePath:
    //     The path to the directory or file to create a symlink of.
    //
    //   accountName:
    //     The name of the account to use
    //
    //   destinationPath:
    //     The path to create the symlink at
    //
    //   createParent:
    //     If the parent directories do not exist, indicates if they should be created.
    //
    //   cancellationToken:
    //     Cancellation token.
    //
    // Returns:
    //     A standard service response including an HTTP status code and request ID.
    Task<Microsoft.Azure.AzureOperationResponse> CreateSymLinkAsync(string sourcePath, string accountName, string destinationPath, bool? createParent, CancellationToken cancellationToken);


    //
    // Summary:
    //     Deletes the requested file or folder, optionally recursively.
    //
    // Parameters:
    //   filePath:
    //     The path to the file or folder to delete.
    //
    //   accountName:
    //     The name of the account to use
    //
    //   recursive:
    //     The optional switch indicating if the delete should be recursive
    //
    //   cancellationToken:
    //     Cancellation token.
    //
    // Returns:
    //     The result of the request or operation.
    Task<FileOperationResultResponse> DeleteAsync(string filePath, string accountName, bool recursive, CancellationToken cancellationToken);


    //
    // Summary:
    //     Gets ACL entries on a file or folder.
    //
    // Parameters:
    //   filePath:
    //     The path to the directory or file to get ACLs on.
    //
    //   accountName:
    //     The name of the account to use
    //
    //   cancellationToken:
    //     Cancellation token.
    //
    // Returns:
    //     DataLakeFileSystem Acl information.
    Task<AclStatusResponse> GetAclStatusAsync(string filePath, string accountName, CancellationToken cancellationToken);


    //
    // Summary:
    //     Gets the file content summary object specified by the file path.
    //
    // Parameters:
    //   filePath:
    //     The path to the file to open.
    //
    //   accountName:
    //     The name of the account to use
    //
    //   cancellationToken:
    //     Cancellation token.
    //
    // Returns:
    //     DataLakeFileSystem Account information.
    Task<ContentSummaryResponse> GetContentSummaryAsync(string filePath, string accountName, CancellationToken cancellationToken);


    //
    // Summary:
    //     Gets the file status object specified by the file path.
    //
    // Parameters:
    //   filePath:
    //     The path to the file to open.
    //
    //   accountName:
    //     The name of the account to use
    //
    //   cancellationToken:
    //     Cancellation token.
    //
    // Returns:
    //     DataLakeFileSystem Account information.
    Task<FileStatusResponse> GetFileStatusAsync(string filePath, string accountName, CancellationToken cancellationToken);


    //
    // Summary:
    //     Gets the home directory for the specified account.
    //
    // Parameters:
    //   accountName:
    //     The name of the account to use
    //
    //   cancellationToken:
    //     Cancellation token.
    //
    // Returns:
    //     DataLakeFileSystem Account information.
    Task<HomeDirectoryResponse> GetHomeDirectoryAsync(string accountName, CancellationToken cancellationToken);


    //
    // Summary:
    //     Initiates a file append request, resulting in a return of the data node location
    //     that will service the request. DO NOT USE DIRECTLY.  Call BeginAppend and
    //     BeginAppendAsync instead. This ensures proper following of WebHDFS redirects
    //
    // Parameters:
    //   filePath:
    //     The path to the file to create.
    //
    //   accountName:
    //     The name of the account to use
    //
    //   bufferSize:
    //     The optional buffer size to use when appending data
    //
    //   cancellationToken:
    //     Cancellation token.
    //
    // Returns:
    //     The data from the file requested.
    Task<FileCreateOpenAndAppendResponse> InternalBeginAppendAsync(string filePath, string accountName, long? bufferSize, CancellationToken cancellationToken);


    //
    // Summary:
    //     Initiates a file creation request, resulting in a return of the data node
    //     location that will service the request. DO NOT USE DIRECTLY. Call BeginCreate
    //     and BeginCreateAsync instead. This ensures proper following of WebHDFS redirects
    //
    // Parameters:
    //   filePath:
    //     The path to the file to create.
    //
    //   accountName:
    //     The name of the account to use
    //
    //   parameters:
    //     The optional parameters to use when creating the file
    //
    //   cancellationToken:
    //     Cancellation token.
    //
    // Returns:
    //     The data from the file requested.
    Task<FileCreateOpenAndAppendResponse> InternalBeginCreateAsync(string filePath, string accountName, FileCreateParameters parameters, CancellationToken cancellationToken);


    //
    // Summary:
    //     Gets the data associated with the file handle requested. DO NOT USE DIRECTLY.
    //     Call Open and OpenAsync instead. This ensures proper following of WebHDFS
    //     redirects
    //
    // Parameters:
    //   filePath:
    //     The path to the file to open.
    //
    //   accountName:
    //     The name of the account to retrieve
    //
    //   parameters:
    //     The optional parameters to pass to the open operation
    //
    //   cancellationToken:
    //     Cancellation token.
    //
    // Returns:
    //     The data from the file requested.
    Task<FileCreateOpenAndAppendResponse> InternalBeginOpenAsync(string filePath, string accountName, FileOpenParameters parameters, CancellationToken cancellationToken);


    //
    // Summary:
    //     Gets the list of file status objects specified by the file path.
    //
    // Parameters:
    //   filePath:
    //     The path to the file to open.
    //
    //   accountName:
    //     The name of the account to use
    //
    //   parameters:
    //     Query parameters. If null is passed returns all file status items.
    //
    //   cancellationToken:
    //     Cancellation token.
    //
    // Returns:
    //     DataLakeFileSystem file status list information.
    Task<FileStatusesResponse> ListFileStatusAsync(string filePath, string accountName, DataLakeFileSystemListParameters parameters, CancellationToken cancellationToken);


    //
    // Summary:
    //     Creates a directory.
    //
    // Parameters:
    //   path:
    //     The path to the directory or file to create.
    //
    //   accountName:
    //     The name of the account to use
    //
    //   permission:
    //     The optional permissions to set on the directories
    //
    //   cancellationToken:
    //     Cancellation token.
    //
    // Returns:
    //     The result of the request or operation.
    Task<FileOperationResultResponse> MkdirsAsync(string path, string accountName, string permission, CancellationToken cancellationToken);


    //
    // Summary:
    //     Modifies existing ACL entries on a file or folder.
    //
    // Parameters:
    //   filePath:
    //     The path to the directory or file to modify ACLs on.
    //
    //   accountName:
    //     The name of the account to use
    //
    //   aclSpec:
    //     The ACL spec included in ACL modification operations in the format '[default:]user|group|other::r|-w|-x|-'
    //
    //   cancellationToken:
    //     Cancellation token.
    //
    // Returns:
    //     A standard service response including an HTTP status code and request ID.
    Task<Microsoft.Azure.AzureOperationResponse> ModifyAclEntriesAsync(string filePath, string accountName, string aclSpec, CancellationToken cancellationToken);


    Task<Microsoft.Azure.AzureOperationResponse> MsConcatAsync(string destinationPath, string accountName, Stream streamContents, CancellationToken cancellationToken);


    //
    // Summary:
    //     Gets the data associated with the file handle requested. DO NOT USE DIRECTLY.
    //     Call Open and OpenAsync instead. This ensures proper following of WebHDFS
    //     redirects
    //
    // Parameters:
    //   fileOpenRequestLink:
    //     The link to the file to open including all required parameters.
    //
    //   cancellationToken:
    //     Cancellation token.
    //
    // Returns:
    //     The data from the file requested.
    Task<FileOpenResponse> OpenAsync(string fileOpenRequestLink, CancellationToken cancellationToken);


    //
    // Summary:
    //     Removes the existing ACL on a file or folder.
    //
    // Parameters:
    //   filePath:
    //     The path to the directory or file to remove ACL on.
    //
    //   accountName:
    //     The name of the account to use
    //
    //   cancellationToken:
    //     Cancellation token.
    //
    // Returns:
    //     A standard service response including an HTTP status code and request ID.
    Task<Microsoft.Azure.AzureOperationResponse> RemoveAclAsync(string filePath, string accountName, CancellationToken cancellationToken);


    //
    // Summary:
    //     Removes existing ACL entries on a file or folder.
    //
    // Parameters:
    //   filePath:
    //     The path to the directory or file to remove ACLs on.
    //
    //   accountName:
    //     The name of the account to use
    //
    //   aclSpec:
    //     The ACL spec included in ACL removal operations in the format '[default:]user|group|other::r|-w|-x|-'
    //
    //   cancellationToken:
    //     Cancellation token.
    //
    // Returns:
    //     A standard service response including an HTTP status code and request ID.
    Task<Microsoft.Azure.AzureOperationResponse> RemoveAclEntriesAsync(string filePath, string accountName, string aclSpec, CancellationToken cancellationToken);


    //
    // Summary:
    //     Removes default ACL entries on a file or folder.
    //
    // Parameters:
    //   filePath:
    //     The path to the directory or file to remove default ACLs on.
    //
    //   accountName:
    //     The name of the account to use
    //
    //   cancellationToken:
    //     Cancellation token.
    //
    // Returns:
    //     A standard service response including an HTTP status code and request ID.
    Task<Microsoft.Azure.AzureOperationResponse> RemoveDefaultAclAsync(string filePath, string accountName, CancellationToken cancellationToken);


    //
    // Summary:
    //     Rename a directory or file.
    //
    // Parameters:
    //   sourcePath:
    //     The path to the directory to move.
    //
    //   accountName:
    //     The name of the account to use
    //
    //   destinationPath:
    //     The path to move the file or folder to
    //
    //   cancellationToken:
    //     Cancellation token.
    //
    // Returns:
    //     The result of the request or operation.
    Task<FileOperationResultResponse> RenameAsync(string sourcePath, string accountName, string destinationPath, CancellationToken cancellationToken);


    //
    // Summary:
    //     Sets ACL entries on a file or folder.
    //
    // Parameters:
    //   filePath:
    //     The path to the directory or file to set ACLs on.
    //
    //   accountName:
    //     The name of the account to use
    //
    //   aclSpec:
    //     The ACL spec included in ACL creation operations in the format '[default:]user|group|other::r|-w|-x|-'
    //
    //   cancellationToken:
    //     Cancellation token.
    //
    // Returns:
    //     A standard service response including an HTTP status code and request ID.
    Task<Microsoft.Azure.AzureOperationResponse> SetAclAsync(string filePath, string accountName, string aclSpec, CancellationToken cancellationToken);


    //
    // Summary:
    //     Sets the owner of a file or folder.
    //
    // Parameters:
    //   sourcePath:
    //     The path to the directory or file to set the owner on.
    //
    //   accountName:
    //     The name of the account to use
    //
    //   owner:
    //     The username who is the owner of a file/directory, if empty remains unchanged.
    //
    //   group:
    //     The name of a group, if empty remains unchanged.
    //
    //   cancellationToken:
    //     Cancellation token.
    //
    // Returns:
    //     A standard service response including an HTTP status code and request ID.
    Task<Microsoft.Azure.AzureOperationResponse> SetOwnerAsync(string sourcePath, string accountName, string owner, string group, CancellationToken cancellationToken);


    //
    // Summary:
    //     Sets the permission of the file or folder.
    //
    // Parameters:
    //   sourcePath:
    //     The path to the directory or file to set permissions on.
    //
    //   accountName:
    //     The name of the account to use
    //
    //   permission:
    //     A string octal representation of the permission (i.e 'rwx')
    //
    //   cancellationToken:
    //     Cancellation token.
    //
    // Returns:
    //     A standard service response including an HTTP status code and request ID.
    Task<Microsoft.Azure.AzureOperationResponse> SetPermissionAsync(string sourcePath, string accountName, string permission, CancellationToken cancellationToken);


    //
    // Summary:
    //     Sets the value of the replication factor.
    //
    // Parameters:
    //   sourcePath:
    //     The path to the directory or file to create a replication of.
    //
    //   accountName:
    //     The name of the account to use
    //
    //   replication:
    //     The number of replications of a file.
    //
    //   cancellationToken:
    //     Cancellation token.
    //
    // Returns:
    //     The result of the request or operation.
    Task<FileOperationResultResponse> SetReplicationAsync(string sourcePath, string accountName, short? replication, CancellationToken cancellationToken);


    //
    // Summary:
    //     Sets the access or modification time on a file or folder.
    //
    // Parameters:
    //   sourcePath:
    //     The path to the directory or file to set permissions on.
    //
    //   accountName:
    //     The name of the account to use
    //
    //   modificationTime:
    //     The modification time of a file/directory. If -1 remains unchanged
    //
    //   accessTime:
    //     The access time of a file/directory. If -1 remains unchanged
    //
    //   cancellationToken:
    //     Cancellation token.
    //
    // Returns:
    //     A standard service response including an HTTP status code and request ID.
    Task<Microsoft.Azure.AzureOperationResponse> SetTimesAsync(string sourcePath, string accountName, long modificationTime, long accessTime, CancellationToken cancellationToken);


#### Microsoft.Azure.Management.DataLakeFileSystem.Uploading.IFrontEndAdapter

    // Summary:
    //     Appends the given byte array to the end of a given stream.
    //
    // Parameters:
    //   streamPath:
    //     The relative path to the stream.
    //
    //   data:
    //     An array of bytes to be appended to the stream.
    //
    //   offset:
    //     The offset at which to append to the stream.
    //
    //   length:
    //     The number of bytes to append (starting at 0).
    //
    // Exceptions:
    //   System.ArgumentNullException:
    //     If the data to be appended is null or empty.
    void AppendToStream(string streamPath, byte[] data, long offset, int length);


    //
    // Summary:
    //     Concatenates the given input streams (in order) into the given target stream.
    //      At the end of this operation, input streams will be deleted.
    //
    // Parameters:
    //   targetStreamPath:
    //     The relative path to the target stream.
    //
    //   inputStreamPaths:
    //     An ordered array of paths to the input streams.
    void Concatenate(string targetStreamPath, string[] inputStreamPaths);


    //
    // Summary:
    //     Creates a new, empty stream at the given path.
    //
    // Parameters:
    //   streamPath:
    //     The relative path to the stream.
    //
    //   overwrite:
    //     Whether to overwrite an existing stream.
    void CreateStream(string streamPath, bool overwrite);


    //
    // Summary:
    //     Deletes an existing stream at the given path.
    //
    // Parameters:
    //   streamPath:
    //     The relative path to the stream.
    void DeleteStream(string streamPath);


    //
    // Summary:
    //     Gets a value indicating the length of a stream, in bytes.
    //
    // Parameters:
    //   streamPath:
    //     The relative path to the stream.
    //
    // Returns:
    //     The length of the stream, in bytes.
    long GetStreamLength(string streamPath);


    //
    // Summary:
    //     Determines if the stream with given path exists.
    //
    // Parameters:
    //   streamPath:
    //     The relative path to the stream.
    //
    // Returns:
    //     True if the stream exists, false otherwise.
    bool StreamExists(string streamPath);


------------

### Useful links

Browse the following pages:

* [Getting Started](../GettingStarted.md)
* Tools
    * [Azure Portal](../AzurePortal/FirstSteps.md)
    * [PowerShell](../PowerShell/FirstSteps.md)
    * [SDK](../SDK/FirstSteps.md)
