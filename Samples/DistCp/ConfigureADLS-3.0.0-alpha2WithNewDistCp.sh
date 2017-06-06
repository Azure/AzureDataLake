#! /bin/bash

machine_login_usn=$1
adls_account_name=$2
tenant_id=$3
client_id=$4
client_credential=$5

hn=`hostname`

distcp_jar_name='hadoop-distcp-2.9.0-SNAPSHOT-cbs-java1.7compat.jar'
distcp_jar_url='http://azuresscripts.blob.core.windows.net/distcp'

adl_sdk_jar_name='azure-data-lake-store-sdk-2.1.5.jar'
adl_sdk_jar_url='http://central.maven.org/maven2/com/microsoft/azure/azure-data-lake-store-sdk/2.1.5'

adl_driver_jar_name='hadoop-azure-datalake-3.0.0-alpha2.jar'
adl_driver_jar_url='http://central.maven.org/maven2/org/apache/hadoop/hadoop-azure-datalake/3.0.0-alpha2'


function ConfigureADLSWithNewDistCp()
{
	rm $adl_sdk_jar_name
	wget $adl_sdk_jar_url/$adl_sdk_jar_name	
	rm $adl_driver_jar_name
	wget $adl_driver_jar_url/$adl_driver_jar_name

	rm $distcp_jar_name
	wget $distcp_jar_url/$distcp_jar_name
	
	jackson_files_colon=`find $HADOOP_HOME/share/hadoop/tools/lib/ -name jackson-core* | tr '\n' ':'`
	jackson_files_csv=`echo $jackson_files_colon | tr ':' ','`
	
	unset HADOOP_CLASSPATH
	export HADOOP_CLASSPATH=$jackson_files_colon/home/$machine_login_usn/$adl_sdk_jar_name:/home/$machine_login_usn/$adl_driver_jar_name:/home/$machine_login_usn/$distcp_jar_name:`hadoop classpath`
	export LIBJARS=$jackson_files_csv/home/$machine_login_usn/$adl_sdk_jar_name,/home/$machine_login_usn/$adl_driver_jar_name,/home/$machine_login_usn/$distcp_jar_name
	
	echo $HADOOP_CLASSPATH
	echo $LIBJARS

	alias adlsHdfs='hdfs dfs -libjars $LIBJARS -D fs.AbstractFileSystem.adl.impl=org.apache.hadoop.fs.adl.Adl -D fs.adl.impl=org.apache.hadoop.fs.adl.AdlFileSystem -D dfs.adls.oauth2.access.token.provider.type=ClientCredential -D dfs.adls.oauth2.refresh.url=https://login.windows.net/$tenant_id/oauth2/token -D dfs.adls.oauth2.client.id=$client_id -D dfs.adls.oauth2.credential=$client_credential'

	alias adlsHadoopDistCp='hadoop distcp -libjars $LIBJARS -D fs.AbstractFileSystem.adl.impl=org.apache.hadoop.fs.adl.Adl -D fs.adl.impl=org.apache.hadoop.fs.adl.AdlFileSystem -D dfs.adls.oauth2.access.token.provider.type=ClientCredential -D dfs.adls.oauth2.refresh.url=https://login.windows.net/$tenant_id/oauth2/token -D dfs.adls.oauth2.client.id=$client_id -D dfs.adls.oauth2.credential=$client_credential'
}

function Test() {
	adlsHdfs -ls adl://$adls_account_name.azuredatalakestore.net/
}

function TestDistCp() {
	dirName='tmp'
	cpyBuffSize=4194304
	blksPerChnk=10
	nMappers=4
	adlsHdfs -rmr -skipTrash adl://$adls_account_name.azuredatalakestore.net/*
	adlsHadoopDistCp -blocksperchunk $blksPerChnk -m $nMappers -copybuffersize $cpyBuffSize -bandwidth 10000 /$dirName/ adl://$adls_account_name.azuredatalakestore.net/
	adlsHdfs -ls adl://$adls_account_name.azuredatalakestore.net/$dirName/
}

function PrintLine() {
	DATE=`date +%Y-%m-%d" "%H:%M:%S,%3N`
	echo "$DATE - $1"
}

function Main() {
    PrintLine "Configure ADLS with new DistCp on host: $hn"
	
	# all params are mandatory
    if [ ! -z "$machine_login_usn" -a "$machine_login_usn" != " " ]
        then
            PrintLine "User Name provided: $machine_login_usn"
        else
            PrintLine "User Name not provided! => EXIT"
            exit
    fi
	
	    if [ ! -z "$adls_account_name" -a "$adls_account_name" != " " ]
        then
            PrintLine "ADLS account provided: $adls_account_name"
        else
            PrintLine "ADLS account NOT provided! => EXIT"
            exit
    fi
    
    if [ ! -z "$tenant_id" -a "$tenant_id" != " " ]
        then
            PrintLine "Tenant ID provided: $tenant_id"
        else
            PrintLine "Tenant ID NOT provided! => EXIT"
            exit
    fi
    
    if [ ! -z "$client_id" -a "$client_id" != " " ]
        then
            PrintLine "Client ID provided: $client_id"
        else
            PrintLine "Client ID NOT provided! => EXIT"
            exit
    fi
    
    if [ ! -z "$client_credential" -a "$client_credential" != " " ]
        then
            PrintLine "Client credential provided: $client_credential"
        else
            PrintLine "Client credential NOT provided! => EXIT"
            exit
    fi
    
	ConfigureADLSWithNewDistCp
	Test
}

Main

