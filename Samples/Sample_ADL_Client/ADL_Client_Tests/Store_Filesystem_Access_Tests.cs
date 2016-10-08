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

            var permissions_before = this.adls_fs_client.GetPermissions(fname);

            // find all the named user entries that have write access
            var entries_before = permissions_before.Entries.Where(e => e.Type == AclType.NamedUser).Where(e=>e.Permission.Value.Write).ToList();
            Assert.IsTrue(entries_before.Count>0);

            // Remove write access for all those entries
            var perms_mask = new FsPermission("r-x");
            foreach (var entry in entries_before)
            {
                var new_entry_x = new FsAclEntry(entry.Type,entry.Name,entry.Permission.Value.AndWith( perms_mask));
                this.adls_fs_client.ModifyACLs(fname, new_entry_x.ToString());
            }

            var permissions_after = this.adls_fs_client.GetPermissions(fname);
            // find all the named user entries that have write access
            var entries_after = permissions_after.Entries.Where(e => e.Type == AclType.NamedUser).Where(e => e.Permission.Value.Write).ToList();
            // verify that there are no such entries
            Assert.AreEqual(0, entries_after.Count);
        }

    }
}

