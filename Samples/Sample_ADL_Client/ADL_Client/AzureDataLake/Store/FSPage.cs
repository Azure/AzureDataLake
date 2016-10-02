using System.Collections.Generic;
using Microsoft.Azure.Management.DataLake.Store.Models;

namespace AzureDataLake.Store
{
    public class FsPage
    {
        public FsPath Path;
        public IList<FileStatusProperties> Children;
    }
}