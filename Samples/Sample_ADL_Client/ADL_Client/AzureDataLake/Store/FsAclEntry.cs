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

            string s = string.Format("{0}:{1}:{2}", aclType, this.Name, rwx);
            return s;
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
                throw new System.ArgumentOutOfRangeException();
            }
            return aclType;
        }
    }
}