# Azure Data Lake - General FAQ

### List of FAQs
* [Azure Data Lake](http://aka.ms/adlfaq)
* [ADL Analytics](http://aka.ms/adlafaq)
* [ADL Store](http://aka.ms/adlsfaq)
* [U-SQL ](http://aka.ms/usqlfaq)

### Q: When will ADL Store and ADL Analytics be "Generally Available"?

In CY2016.

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

### Q: Does ADL Support Parquet Files?

* **ADL Store**: You can move Parquet files into ADL Store from a source HDFS system. (You don't need to worry about any block size fdifferences)
* **HDInsifght**: An HDInsight cluster can read Parquet files stored in ADL Store.
* **ADL Analytics**: ADL Analytics support for Parquet files is currently under investigation and is something on our roadmap

