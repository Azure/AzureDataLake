using System;
using System.Collections.Generic;
using System.Linq;

namespace AzureDataLake.Store
{
    public class FsAclEntry
    {
        public AclType Type;
        public string Name;
        public FsPermission? Permission;

        public override string ToString()
        {
            var rwx = this.Permission.Value.ToRwxString();
            var aclType = AclTypeToString(this.Type);

            string s = String.Format("{0}:{1}:{2}", aclType, this.Name, rwx);
            return s;
        }

        private FsAclEntry()
        {
            
        }

        public FsAclEntry(AclType type, string name, FsPermission permission)
        {
            this.Type = type;

            if (name == null)
            {
                name = "";
            }
            this.Name = name;
            this.Permission = permission;
        }

        public FsAclEntry(string entry)
        {
            var tokens = entry.Split(':');

            string type_str = tokens[0].ToLowerInvariant();
            string user = tokens[1];
            if ((type_str == "user") && (user.Length == 0))
            {
                this.Type = AclType.OwningUser;
            }
            else if ((type_str == "user") && (user.Length > 0))
            {
                this.Type = AclType.NamedUser;
            }
            else if ((type_str == "group") && (user.Length == 0))
            {
                this.Type = AclType.OwningGroup;
            }
            else if ((type_str == "group") && (user.Length > 0))
            {
                this.Type = AclType.NamedGroup;
            }
            else if (type_str == "mask")
            {
                this.Type = AclType.Mask;
            }
            else if (type_str == "other")
            {
                this.Type = AclType.Other;
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }

            this.Name = user;
            this.Permission = new FsPermission(tokens[2]);
        }

        public static string AclTypeToString(AclType type)
        {
            string aclType = "ERROR";
            if (type == AclType.Mask)
            {
                aclType = "mask";
            }
            else if (type == AclType.NamedGroup)
            {
                aclType = "group";
            }
            else if (type == AclType.NamedUser)
            {
                aclType = "user";
            }
            else if (type == AclType.Other)
            {
                aclType = "other";
            }
            else if (type == AclType.OwningGroup)
            {
                aclType = "group";
            }
            else if (type == AclType.OwningUser)
            {
                aclType = "user";
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
            return aclType;
        }

        public FsAclEntry AndWith(FsPermission mask)
        {
            var new_perm = this.Permission.Value.AndWith(mask);
            return this.CloneWithPermission(new_perm);
        }

        public FsAclEntry OrWith(FsPermission mask)
        {
            var new_perm = this.Permission.Value.OrWith(mask);
            return this.CloneWithPermission(new_perm);
        }

        public FsAclEntry CloneWithPermission(FsPermission permission)
        {
            var new_entry = new FsAclEntry(this.Type, this.Name, permission);
            return new_entry;
        }

        public static string EntriesToString(IEnumerable<FsAclEntry> entries)
        {
            var strings = entries.Select(e => e.ToString());
            var s = String.Join(",", strings);
            return s;
        }
    }
}