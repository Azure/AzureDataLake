using System.Collections.Generic;
using Microsoft.Azure.Management.DataLake.Store.Models;

namespace AzureDataLake.Store
{
    public class RecurseResult
    {
        public string Path;
        public IList<FileStatusProperties> Children;
    }
}