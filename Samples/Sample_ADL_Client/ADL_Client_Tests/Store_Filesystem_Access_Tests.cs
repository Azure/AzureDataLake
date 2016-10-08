using System;
using System.Linq;
using AzureDataLake.Store;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ADL_Client_Tests
{
    [TestClass]
    public class Store_Filesystem_Access_Tests : Base_Tests
    {

        private FsPath create_test_dir()
        {
            var dir = new AzureDataLake.Store.FsPath("/test_adl_demo_client");

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

            var cfo = new AzureDataLake.Store.CreateFileOptions();
            cfo.Overwrite = true;
            this.adls_fs_client.CreateFile(fname, "HelloWorld", cfo);

            var y = this.adls_fs_client.GetPermissions(fname);

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

            var y2 = this.adls_fs_client.GetPermissions(fname);

            Assert.AreEqual(true, y2.OwnerPermission.Value.Read);
            Assert.AreEqual(true, y2.OwnerPermission.Value.Write);
            Assert.AreEqual(true, y2.OwnerPermission.Value.Execute);

            Assert.AreEqual(true, y2.GroupPermission.Value.Read);
            Assert.AreEqual(true, y2.GroupPermission.Value.Write);
            Assert.AreEqual(true, y2.GroupPermission.Value.Execute);

            Assert.AreEqual(true, y2.OtherPermission.Value.Read);
            Assert.AreEqual(false, y2.OtherPermission.Value.Write);
            Assert.AreEqual(true, y2.OtherPermission.Value.Execute);
        }

        [TestMethod]
        public void ACLs_Scenario_Find_Entries_With_ReadAccess()
        {
            this.Initialize();
            var dir = create_test_dir();

            var fname = dir.Append("foo.txt");
            if (this.adls_fs_client.Exists(fname))
            {
                this.adls_fs_client.Delete(fname);
            }

            var cfo = new AzureDataLake.Store.CreateFileOptions();
            cfo.Overwrite = true;
            this.adls_fs_client.CreateFile(fname, "HelloWorld", cfo);

            var permissions1 = this.adls_fs_client.GetPermissions(fname);

            var entries1 = permissions1.Entries.Where(e => e.Permission.Value.Write).ToList();
            Assert.AreEqual(7, entries1.Count);

            var userentries1 = entries1.Where(e => e.Type == AclType.NamedUser).Where(e=>e.Permission.Value.Write).ToList();

            foreach (var entry in userentries1)
            {
                var new_entry = string.Format("user:{0}:r-x",entry.Name);
                this.adls_fs_client.ModifyACLs(fname, new_entry);
            }

            var permissions2 = this.adls_fs_client.GetPermissions(fname);
            var entries2 = permissions2.Entries.Where(e => e.Permission.Value.Write).ToList();

            var userentries2 = entries2.Where(e => e.Type == AclType.NamedUser).Where(e => e.Permission.Value.Write).ToList();
            Assert.AreEqual(0, userentries2.Count);

            int x = 1;
        }

    }
}

