# Common Azure imports
import adal
import azure.mgmt.resource.resources
import msrestazure.azure_active_directory

# ADLS imports
import azure.mgmt.datalake.store
import azure.datalake.store

# ADLA imports
import azure.mgmt.datalake.analytics

# All other imports
import os
import sys
import itertools


# In[ ]:

clientid_azure_xplat_cli = '04b07795-8ddb-461a-bbee-02f9e1bf7b46'

def get_user_token_interactive(the_tenant, clientid, token_filename) :
    flag_ignore_cache = False # Unless you are debugging something with tokens, Recommend leave this as False
    flag_open_devicelogin_webpage = False 
    flag_save_token = True
  
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

    if (flag_save_token) :
        pickle.dump( token, open( token_filename, "wb" ) )

    # Needed for ADLS dataplane operations
    token.update({'access': token['accessToken'], 
        'resource': RESOURCE,
        'refresh': token.get('refreshToken', False),
        'time': time.time(), 'tenant': the_tenant, 'client': clientid})

    return token


class AuthenticatedSession :

    def __init__( self , subscriptionid, tenant , token_filename) :
        self.SubscriptionID = subscriptionid
        self.ClientID = clientid_azure_xplat_cli
        self.Tenant = tenant
        self.Token = get_user_token_interactive(tenant, self.ClientID, token_filename)
        self.Credentials =  msrestazure.azure_active_directory.AADTokenCredentials(self.Token, self.ClientID)

class DataLakeResourceClients :

    def __init__( self , auth_session ) :
        self.AnalyticsAccountClient = azure.mgmt.datalake.analytics.DataLakeAnalyticsAccountManagementClient( auth_session.Credentials, auth_session.SubscriptionID)
        self.StoreAccountClient = azure.mgmt.datalake.store.DataLakeStoreAccountManagementClient( auth_session.Credentials, auth_session.SubscriptionID)
        self.ResourceClient = azure.mgmt.resource.resources.ResourceManagementClient( auth_session.Credentials, auth_session.SubscriptionID)

class DataLakeAnalyticsClients :

    def __init__( self , auth_session ) :
        self.JobClient = azure.mgmt.datalake.analytics.DataLakeAnalyticsJobManagementClient(auth_session.Credentials, 'azuredatalakeanalytics.net')
        self.CatalogClient = azure.mgmt.datalake.analytics.DataLakeAnalyticsCatalogManagementClient(auth_session.Credentials, 'azuredatalakeanalytics.net')

class DataLakeStoreClients :

    def __init__( self , auth_session, account_name ) :
        self.AccountName = account_name
        self.FileSystemClient = azure.datalake.store.core.AzureDLFileSystem(auth_session.Token, store_name = self.AccountName)
