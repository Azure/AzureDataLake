using Microsoft.Azure.Management.DataLake.Store.Models;

namespace AzureDataLake.Store
{
    public class FsFileStatus
    {
        public FsUnixTime? ExpirationTime;
        public FsUnixTime? AccessTime;
        public FsUnixTime? ModificationTime;
        public long BlockSize;
        public long? ChildrenNum;
        public long Length;
        public string Group;
        public string Owner;
        public string PathSuffix;
        public string Permission;
        public FileType Type;
        public FsFileStatus(FileStatusProperties fs)
        {
            this.ExpirationTime = FsUnixTime.TryParseDouble(fs.ExpirationTime);
            this.AccessTime= new FsUnixTime(fs.AccessTime.Value);
            this.ModificationTime= new FsUnixTime(fs.ModificationTime.Value);
            this.BlockSize = fs.BlockSize.Value;
            this.ChildrenNum = fs.ChildrenNum;
            this.Length = fs.Length.Value;
            this.Group = fs.Group;
            this.Owner = fs.Owner;
            this.PathSuffix = fs.PathSuffix;
            this.Permission = fs.Permission;
            this.Type = fs.Type.Value;


            /*
             */
        }
    }
}