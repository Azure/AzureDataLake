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


import adlhelper

print("DEMO STARTING")


# define constants
tenant = "microsoft.onmicrosoft.com"
subscription_id = '045c28ea-c686-462f-9081-33c34e871ba3'
adls_account_name = 'datainsightsadhoc'
adla_account_name = 'datainsightsadhoc'

# Handle Authentication
auth_session = adlhelper.AuthenticatedSession( subscription_id , tenant, r"C:\src\tr23python\adl_demo_tokencache.pickle")
token = auth_session.Token
credentials =  auth_session.Credentials

# Client construction
resource_clients = adlhelper.DataLakeResourceClients( auth_session )
store_clients = adlhelper.DataLakeStoreClients( auth_session, adls_account_name)
analytics_clients = adlhelper.DataLakeAnalyticsClients( auth_session )

# Use the Clients ------------------------------------------------

# List ADLA Accounts
adla_accounts = resource_clients.AnalyticsAccountClient.account.list()
for a in adla_accounts:
    print("ADLA: " + a.name)

# List ADLS Accounts
adls_accounts = resource_clients.StoreAccountClient.account.list()
for a in adls_accounts:
    print("ADLS: " + a.name)

# List Files in ADLS
files = store_clients.FileSystemClient.ls( "/system" )
for f in files:
    print(f)

# List 10 Jobs in ADLS

jobs = analytics_clients.JobClient.job.list( adla_account_name )
jobs = itertools.islice(jobs,10) # comment this out if you want all the jobs
for j in jobs:
    print("---------------------------------")
    print(j.name)
    print(j.submit_time)
    print(j.submitter)

print("DEMO FINISHED")