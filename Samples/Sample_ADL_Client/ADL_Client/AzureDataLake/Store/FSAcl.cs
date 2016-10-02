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
                var tokens = e.Split(':');
                var acl_entry = new FsAclEntry();

                string type_str = tokens[0].ToLowerInvariant();
                string user = tokens[1];
                if ((type_str == "user") && (user.Length == 0))
                {
                    acl_entry.Type = AclType.OwningUser;
                }
                else if ((type_str == "user") && (user.Length > 0))
                {
                    acl_entry.Type = AclType.NamedUser;
                }
                else if ((type_str == "group") && (user.Length == 0))
                {
                    acl_entry.Type = AclType.OwningGroup;
                }
                else if ((type_str == "group") && (user.Length > 0))
                {
                    acl_entry.Type = AclType.NamedGroup;
                }
                else if (type_str == "mask")
                {
                    acl_entry.Type = AclType.Mask;
                }
                else if (type_str == "other")
                {
                    acl_entry.Type = AclType.Other;
                }
                else 
                {
                    throw new System.ArgumentOutOfRangeException();
                }

                acl_entry.Name = user;
                acl_entry.Permission = new FsPermission(tokens[2]);

                this.Entries.Add(acl_entry);
            }
        }
    }
}