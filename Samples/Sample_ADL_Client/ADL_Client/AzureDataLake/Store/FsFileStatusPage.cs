using System.Collections.Generic;
using Microsoft.Azure.Management.DataLake.Store.Models;

namespace AzureDataLake.Store
{
    public class FsFileStatusPage
    {
        public FsPath Path;
        public IList<FsFileStatus> FileItems;
    }
}