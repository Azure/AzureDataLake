using System;
using System.Linq;
using AzureDataLake.Store;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ADL_Client_Tests
{

    [TestClass]
    public class Store_Tests : Base_Tests
    {
 

        [TestMethod]
        public void Get_AD_Tenant_ID()
        {
            this.Initialize();
            var directory = AzureDataLake.Authentication.Directory.Resolve("microsoft.com");
        }

        [TestMethod]
        public void List_ADLS_Accounts()
        {
            this.Initialize();
            var adls_accounts = this.adls_mgmt_client.ListAccounts();
            foreach (var a in adls_accounts)
            {
                System.Console.WriteLine("Store {0} ", a.Name);
            }
        }

        [TestMethod]
        public void List_Files_Recursive()
        {
            this.Initialize();

            int page_count = 0;
            int child_count = 0;

            var pages = this.adls_fs_client.ListFilesRecursive(AzureDataLake.Store.FSPath.Root, 4);
            foreach (var page in pages)
            {
                foreach (var child in page.Children)
                {
                    child_count++;
                }
                page_count++;

                if (page_count == 3)
                {
                    break;
                }
            }

            Assert.AreEqual(3,page_count);
            Assert.AreEqual(3*4,child_count);
        }

        [TestMethod]
        public void List_Files_Non_Recursive()
        {
            this.Initialize();

            int page_count = 0;
            int child_count = 0;

            var pages = this.adls_fs_client.ListFiles(AzureDataLake.Store.FSPath.Root, 4);
            foreach (var page in pages)
            {
                foreach (var child in page.Children)
                {
                    child_count++;
                }
                page_count++;

            }

        }

        [TestMethod]
        public void Basic_File_Scenario()
        {
            this.Initialize();
            var dir = create_test_dir();
            
            var fname = dir.Append("foo.txt");
            this.adls_fs_client.CreateFile(fname, "HelloWorld", true);
            Assert.IsTrue( this.adls_fs_client.Exists(fname));
            var fi = this.adls_fs_client.GetFileInformation(fname);
            Assert.AreEqual(10,fi.FileStatus.Length);

            using (var s = this.adls_fs_client.OpenFileForReadText(fname))
            {
                var content = s.ReadToEnd();
                Assert.AreEqual("HelloWorld",content);
            }

            this.adls_fs_client.Delete(dir,true);
            Assert.IsFalse(this.adls_fs_client.Exists(fname));
            Assert.IsFalse(this.adls_fs_client.Exists(dir));

        }

        private FSPath create_test_dir()
        {
            var dir = new AzureDataLake.Store.FSPath("/test_adl_demo_client");

            if (this.adls_fs_client.Exists(dir))
            {
                this.adls_fs_client.Delete(dir, true);
            }

            this.adls_fs_client.CreateDirectory(dir);

            if (!this.adls_fs_client.Exists(dir))
            {
                Assert.Fail();
            }
            return dir;
        }

        [TestMethod]
        public void ACLs_Scenario()
        {
            this.Initialize();
            var dir = create_test_dir();

            var fname = dir.Append("foo.txt");
            if (this.adls_fs_client.Exists(fname))
            {
                this.adls_fs_client.Delete(fname);
            }
            this.adls_fs_client.CreateFile(fname, "HelloWorld", true);

            var x = this.adls_fs_client.GetPermissions(fname);
            var y = new AzureDataLake.Store.FSAcl(x);

            Assert.AreEqual(true, y.OwnerPermission.Value.Read);
            Assert.AreEqual(true, y.OwnerPermission.Value.Write);
            Assert.AreEqual(true, y.OwnerPermission.Value.Execute);

            Assert.AreEqual(true, y.GroupPermission.Value.Read);
            Assert.AreEqual(true, y.GroupPermission.Value.Write);
            Assert.AreEqual(true, y.GroupPermission.Value.Execute);

            Assert.AreEqual(false, y.OtherPermission.Value.Read);
            Assert.AreEqual(false, y.OtherPermission.Value.Write);
            Assert.AreEqual(false, y.OtherPermission.Value.Execute);

            this.adls_fs_client.ModifyACLs(fname, "other::r-x");

            var x2 = this.adls_fs_client.GetPermissions(fname);
            var y2 = new AzureDataLake.Store.FSAcl(x2);

            Assert.AreEqual(true, y2.OwnerPermission.Value.Read);
            Assert.AreEqual(true, y2.OwnerPermission.Value.Write);
            Assert.AreEqual(true, y2.OwnerPermission.Value.Execute);

            Assert.AreEqual(true, y2.GroupPermission.Value.Read);
            Assert.AreEqual(true, y2.GroupPermission.Value.Write);
            Assert.AreEqual(true, y2.GroupPermission.Value.Execute);

            Assert.AreEqual(true, y2.OtherPermission.Value.Read);
            Assert.AreEqual(false, y2.OtherPermission.Value.Write);
            Assert.AreEqual(true, y2.OtherPermission.Value.Execute);


            int dsx = 1;


        }


    }


}

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

    public struct FSPermission
    {
        public int value;
        public FSPermission(int i)
        {
            if (i > 7)
            {
                throw new ArgumentOutOfRangeException(nameof(i));
            }

            if (i < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(i));
            }

            this.value = i;
        }

        public FSPermission(string s)
        {
            if (s.Length !=3)
            {
                throw new ArgumentOutOfRangeException(nameof(s));
            }

            this.value = 0;
            this.Read = (s[0] == 'r' || s[0] == 'R');
            this.Write = (s[1] == 'w' || s[1] == 'W');
            this.Execute = (s[2] == 'X' || s[2] == 'X');
        }

        public bool Read
        {
            get
            {
                return (0x4 & this.value) != 0;
            }
            set
            {
                if (value)
                {
                    this.value |= 0x4;
                }
                else
                {
                    this.value &= ~0x4;
                }
            }
        }

        public bool Write
        {
            get
            {
                return (0x2 & this.value) != 0;
            }
            set
            {
                if (value)
                {
                    this.value |= 0x2;
                } 
                else
                {
                    this.value &= ~0x2;
                }
            }
        }
        public bool Execute
        {
            get
            {
                return (0x1 & this.value) != 0;
            }
            set
            {
                if (value)
                {
                    this.value |= 0x1;
                }
                else
                {
                    this.value &= ~0x1;
                }
            }
        }

    }
}

