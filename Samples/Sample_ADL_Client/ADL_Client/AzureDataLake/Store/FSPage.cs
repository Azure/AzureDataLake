using System.Collections.Generic;
using Microsoft.Azure.Management.DataLake.Store.Models;

namespace AzureDataLake.Store
{
    public class FSPage
    {
        public FSPath Path;
        public IList<FileStatusProperties> Children;
    }
}