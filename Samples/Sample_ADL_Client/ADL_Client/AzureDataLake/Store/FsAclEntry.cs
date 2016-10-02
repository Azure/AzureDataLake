namespace AzureDataLake.Store
{
    public class FsAclEntry
    {
        public AclType Type;
        public string Name;
        public FsPermission? Permission;
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