Data Export & Import
====================

 

Summary
-------

These instructions show the steps to enable other ADLA accounts to be able to
use the GHT data.

 

Overview of Steps for Consumers
-------------------------------

First send email to EMAIL\@MICROSOFT.COM requesting access. Provide the name of
a “Microsoft Account” (i.e. outlook.com, live.com, etc.). You will receive an
email confirming you have permissions.

 

If you haven’t already create an ADLA account via portal.azure.com.

 

Then submit the **import.usql** script found here:

 

https://github.com/Microsoft/ghinsights/tree/master/ghinsights/dataexport

 

This will take a while, once it is done you will have a copy of the
**GithubAnalytics **U-SQL Database in your account. and then you will be able to
run U-SQL jobs to query that data.

 

Note: The data is copied over to your ADLA Catalog (which uses ADLS for
storage). Keep in mind you are paying for the costs of storing it.

 

In the Future
-------------

 

The process of using the **GitHubAnalytics **database will be much simpler. You
will “mount” the database into your ADLA account. This is both much simpler and
much faster. Also you don’t have to pay the storage costs.

 

 
-
