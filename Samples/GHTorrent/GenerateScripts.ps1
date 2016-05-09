#
#SELECT account
#

# Login as a user to the correct subscription
# Login-AzureRmAccount
# Select-AzureRmSubscription -SubscriptionId b44f0353-37bd-4376-bb56-351d8622535f


#Get a list of tables that we will be writing to a file
$tables = Get-AzureRmDataLakeAnalyticsCatalogItem -Account ghtdev -ItemType Table -Path ghtorrent.dbo

function doublequote( $s )
{
    $output = "`"" + $s + "`""
    $output
}

Write-Host
Write-Host
Write-Host
Write-Host // BEGIN --------------------------------------------------------------------
Write-Host // This script runs on the ghtdev account by a Developer in the ghtdev account
Write-Host

#Get the script that will write the table to csv files
foreach ($table in $tables)
{
    $tablename = $table.DatabaseName+"."+$table.SchemaName+"."+$table.Name
    $outputpath = "@`"/StagingData/"+ $tablename + "`""
    Write-Host "OUTPUT $tablename TO $outputpath USING Outputters.Tsv();";
}
 
Write-Host
Write-Host // END ----------------------------------------------------------------------
Write-Host
Write-Host
Write-Host


Write-Host
Write-Host
Write-Host
Write-Host // BEGIN --------------------------------------------------------------------
Write-Host // This script runs on the GHT consumers account 
Write-Host

#Get the DDL statements that will recreate the tables
foreach ($table in $tables)
{
    $base="CREATE TABLE "+$table.Name+"(";
	#Read and generate the schema
    for ($i = 0; $i -lt $table.ColumnList.Count; $i++)
    {
        $col = $table.ColumnList[$i];
        $base += $col.Name + " ";
        $type = $col.Type.Replace("System.","").Replace("32","");
        if (!$type.Contains("DateTime") -And !$type.Contains("Single"))
        {
            $type=$type.ToLower();
        }
        $base += $type;
        $base +=",";
    }
    $index=$table.IndexList[0];
    $base+="INDEX "+$index.Name+" CLUSTERED (";
    
	#Read indices and ordering
    for ($i = 0; $i -lt $index.IndexKeys.Count; $i++)
    {
        $base+=$index.IndexKeys[$i].Name;
        if ($index.IndexKeys[$i].Descending)
        {
            $base += " DESC";
        }
        else 
        {
            $base += " ASC";
        }
        if ($i -ne $index.Columns.Count-1)
        {
            $base +=",";
        }
        else 
        {
            $base +=")";
        }
    }

    $distribution = $index.DistributionInfo;
    $partition=" PARTITIONED BY ";
	
	#Read partitioning info
    if ($distribution.Type -eq 2)
    {
        $partition+="HASH (";
    }
    elseif ($distribution.Type -eq 5)
    {
        $partition+="RANGE (";
    }
    elseif ($distribution.Type -eq 6)
    {
        $partition+="ROUND ROBIN(";
    }
    
	#Read ordering for partitions
    for ($i = 0; $i -lt $distribution.Keys.Count; $i++)
    {

         $partition+=$distribution.Keys[$i].Name;
        
        if (!($distribution.Type -eq 2))
        {

            if ($distribution.Keys[$i].Descending)
            {
                $partition += " DESC";
            }
            else 
            {
                $partition += " ASC";
            }
        }


        if ($i -ne $index.Columns.Count-1)
        {
            $partition+=","
        }
        else 
        {
            $partition+=")";
        }

    }
    
    $base+=$partition;
    $base+=");"
    Write-Host $base;
}




#Get the DDL statements that will read from the TSV files the data to insert into the tables created above
foreach ($table in $tables){    
    $base="@populate = EXTRACT ";
	#Read schema for insert statements
    for ($i = 0; $i -lt $table.ColumnList.Count; $i++){
        $col = $table.ColumnList[$i];
        $base += $col.Name + " ";
        $type = $col.Type.Replace("System.","").Replace("32","");
        if (!$type.Contains("DateTime") -And !$type.Contains("Single")){
            $type=$type.ToLower();
        }
        $base += $type;
        if ($i -ne $table.ColumnList.Count-1){
            $base +=",";
        }
        else {
            $base +=" ";
        }
    }

    $src_adls = "adl://ghtdev.azuredatalakestore.net"
    $tablename = $table.DatabaseName+"."+$table.SchemaName+"."+$table.Name
    $srcfile = $src_adls + "/StagingData/" + $tablename 
    $base+="FROM `"$srcfile`" USING Extractors.Csv(); INSERT INTO "+ $tablename +" SELECT * FROM @populate;";
    Write-Host $base;
}

Write-Host
Write-Host // END ----------------------------------------------------------------------
Write-Host
Write-Host
Write-Host
