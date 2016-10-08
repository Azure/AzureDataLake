using System.Collections.Generic;

namespace AzureDataLake.Store
{
    public class FsAcl
    {
        public string Group;
        public string Owner;
        public FsPermission? OwnerPermission;
        public FsPermission? GroupPermission;
        public FsPermission? OtherPermission;

        public List<FsAclEntry> Entries;

        public FsAcl(Microsoft.Azure.Management.DataLake.Store.Models.AclStatus acl)
        {
            this.Group = acl.Group;
            this.Owner = acl.Owner;

            if (acl.Permission.HasValue)
            {
                if (acl.Permission > 777)
                {
                    throw new System.ArgumentOutOfRangeException();
                }
                if (acl.Permission < 0)
                {
                    throw new System.ArgumentOutOfRangeException();
                }

                string s = acl.Permission.Value.ToString("000");
                this.OwnerPermission = new AzureDataLake.Store.FsPermission(int.Parse(s[0].ToString()));
                this.GroupPermission = new AzureDataLake.Store.FsPermission(int.Parse(s[1].ToString()));
                this.OtherPermission = new AzureDataLake.Store.FsPermission(int.Parse(s[2].ToString()));
            }

            this.Entries = new List<FsAclEntry>( acl.Entries.Count);
            foreach (string e in acl.Entries)
            {
                var acl_entry = new FsAclEntry(e);
                this.Entries.Add(acl_entry);
            }
        }
    }
}