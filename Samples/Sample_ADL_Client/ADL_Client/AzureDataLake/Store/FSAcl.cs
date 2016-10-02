namespace AzureDataLake.Store
{
    public class FSAcl
    {
        public string Group;
        public string Owner;
        public FSPermission? OwnerPermission;
        public FSPermission? GroupPermission;
        public FSPermission? OtherPermission;


        public FSAcl(Microsoft.Azure.Management.DataLake.Store.Models.AclStatus acl)
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
                this.OwnerPermission = new AzureDataLake.Store.FSPermission(int.Parse(s[0].ToString()));
                this.GroupPermission = new AzureDataLake.Store.FSPermission(int.Parse(s[1].ToString()));
                this.OtherPermission = new AzureDataLake.Store.FSPermission(int.Parse(s[2].ToString()));
            }
        }
    }
}