# Make sure you've got the latest modules 
# pip install azure -U 
# pip install azure-mgmt-resource -U 
# pip install azure-mgmt-datalake-store -U 
# pip install azure-datalake-store -U 


import msrestazure.azure_active_directory

# ADLS imports
import azure.mgmt.datalake.store
import azure.datalake.store
#from azure.datalake.store import core
#from azure.datalake.store import lib
#from azure.datalake.store import multithread


# ADLA imports
import azure.mgmt.datalake.analytics 
#from azure.mgmt.datalake.analytics import account
#from azure.mgmt.datalake.analytics import job
#from azure.mgmt.datalake.analytics import catalog

# Common Azure imports
import azure.mgmt.resource.resources

# All other imports
import os
import sys
import adal
import itertools

def get_user_token_interactive(the_tenant, clientid, token_filename) :

    flag_ignore_cache = False # Unless you are debugging something with tokens, Recommend leave this as False
    flag_open_devicelogin_webpage = True 
  
    import pickle
    import webbrowser
    import time
    RESOURCE = 'https://management.core.windows.net/'
    authority_host_url = "https://login.microsoftonline.com"
    authority_url = authority_host_url + '/' + the_tenant
    context = adal.AuthenticationContext(authority_url)
    devicelogin_url = "https://aka.ms/devicelogin"

    read_from_cache = os.path.isfile(token_filename) and (not(flag_ignore_cache))
    if (read_from_cache) :
        token  = pickle.load( open( token_filename, "rb" ) )
        refresh_token = token['refreshToken']
        token = context.acquire_token_with_refresh_token( refresh_token,clientid,RESOURCE)
    else:
        code = context.acquire_user_code(RESOURCE, clientid)
        message = code['message'] 
        print(message)

        if (flag_open_devicelogin_webpage) :
            if ( (message != None) and (type(message) is str)) :
                if (devicelogin_url in message) :
                    webbrowser.open(devicelogin_url, new=0)

        token = context.acquire_token_with_device_code(RESOURCE, code, clientid)

    pickle.dump( token, open( token_filename, "wb" ) )

    # Needed for ADLS dataplane operations
    token.update({'access': token['accessToken'], 
        'resource': RESOURCE,
        'refresh': token.get('refreshToken', False),
        'time': time.time(), 'tenant': the_tenant, 'client': clientid})

    return token


# define constants
clientid = '04b07795-8ddb-461a-bbee-02f9e1bf7b46'
tenant = "microsoft.onmicrosoft.com"
subscription_id = '045c28ea-c686-462f-9081-33c34e871ba3'
adls_account_name = 'datainsightsadhoc'
adla_account_name = 'datainsightsadhoc'

# Get token
token = get_user_token_interactive(tenant, clientid, r"c:\temp\adl_demo_tokencache.pickle")

credentials =  msrestazure.azure_active_directory.AADTokenCredentials(token, clientid)

print ('Constructing clients')

adlaAcctClient = azure.mgmt.datalake.analytics.DataLakeAnalyticsAccountManagementClient(credentials, subscription_id)
adlsAcctClient = azure.mgmt.datalake.store.DataLakeStoreAccountManagementClient(credentials, subscription_id)
resourceClient = azure.mgmt.resource.resources.ResourceManagementClient(credentials, subscription_id)
adlaJobClient = azure.mgmt.datalake.analytics.DataLakeAnalyticsJobManagementClient(credentials, 'azuredatalakeanalytics.net')
adlaCatalogClient = azure.mgmt.datalake.analytics.DataLakeAnalyticsCatalogManagementClient(credentials, 'azuredatalakeanalytics.net')
adlsFileSystemClient = azure.datalake.store.core.AzureDLFileSystem(token, store_name = adls_account_name)

print ('Done constructing clients')

## List the existing Data Lake Analytics accounts
accounts = adlaAcctClient.account.list()
accounts = list(accounts) # Collect all the items into one list
for a in accounts:
    print("ADLA: " + a.name)


## List the existing Data Lake Store accounts
accounts = adlsAcctClient.account.list()
accounts = list(accounts) # Collect all the items into one list
for a in accounts:
    print("ADLS: " + a.name)

## List 10 jobs in an account
jobs = adlaJobClient.job.list( adla_account_name )
jobs = itertools.islice(jobs,10) # comment this out if you want all the jobs
for j in jobs:
    print("---------------------------------")
    print(j.name)
    print(j.submit_time)
    print(j.submitter)
    

## List files  in an account
files = adlsFileSystemClient.ls()
for f in files:
    print(f)
