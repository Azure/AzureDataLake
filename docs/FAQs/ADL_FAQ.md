# Azure Data Lake - General FAQ

[http://aka.ms/adlfaq](http://aka.ms/adlfaq)
 
### List of FAQs
* [Azure Data Lake](http://aka.ms/adlfaq)
* [ADL Analytics](http://aka.ms/adlafaq)
* [ADL Store](http://aka.ms/adlsfaq)
* [U-SQL ](http://aka.ms/usqlfaq)

### Q: Is Azure Data Lake (Store, Analytics, or HDInsight) going to offered as an "on-premises" product?

We currently have no plans to offer on-premises versions of these services. 

### Q: Does Azure Data Lake use TLS(SSL)?

Yes. Azure Data Lake Store & Analytics both use HTTPS with TLS (SSL) for all network communication.

### Q: What network ports do ADL Store and ADL Analytics use?

Because both ADL Store and ADL Analytics use HTTPS for all communication. The only port they use is port 443.

### Q: What are the  restrictions on the name of an ADL Store & ADL Analytics account?

* Only lowercase alphabetical and numbers are allowed
* Whitespace is not allowed
* Punctuation such as . , ; : are not allowed
* No special characters such as - + _
* Size: The names must be between 3 to 24 characters in length.

### Q: Do I have to pay for Visual Studio to use Azure Data Lake?

No. Azure Data Lake Tools for Visual Studio works with Visual Studio Community Editions which are free.

### Q: Which versions of Visual Studio work with Azure Data Lake Tools for Visual Studio?

These versions are supported:
- VS 2012
- VS 2013
- VS 2015

These Editions are supported 
- Ultimate (Enterprise for VS2015)
- Premium
- Pro
- Community

These Editions are NOT supported
- Visual Studio Express
- Visual Studio Test Edition

### Q: Why does the "Data Lake" menu  not show in Visual Studio even when i installed the ADL tool for VS?

We have changed the Data Lake Tools for Visual Studio plugin release cycle and now it is released with Azure SDK (from 2.9 on). Since Azure SDK has a large installation base, in order not to disturb users who don't use Data Lake service, we now hide the Data Lake menu. You can "activite" the menu by creating a Data Lake project, or open Server Explorer, etc.

### Q: Does ADL Support Parquet Files?

* **Data Lake Store**: You can move Parquet files into Data Lake Store from a source HDFS system. (You don't need to worry about any block size fdifferences)
* **HDInsight**: An HDInsight cluster can read Parquet files stored in Data Lake Store.
* **Data Lake Analytics**: Data Lake Analytics support for Parquet files is currently under investigation and is something on our roadmap

