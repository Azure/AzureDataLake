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
            string aclType = "ERROR";
            if (this.Type == AclType.Mask)
            {
                aclType = "mask";
            }
            else if (this.Type == AclType.NamedGroup)
            {
                aclType = "group";
            }
            else if (this.Type == AclType.NamedUser)
            {
                aclType = "user";
            }
            else if (this.Type == AclType.Other)
            {
                aclType = "other";
            }
            else if (this.Type == AclType.OwningGroup)
            {
                aclType = "group";
            }
            else if (this.Type == AclType.OwningUser)
            {
                aclType = "user";
            }
            else 
            {
                throw new System.ArgumentOutOfRangeException();
            }

            string s = string.Format("{0}:{1}:{2}", aclType, this.Name, rwx);
            return s;
        }
    }

    public enum AclType
    {
        OwningUser,
        OwningGroup,
        NamedUser,
        NamedGroup,
        Mask,
        Other
    }
}