using AzureDataLake.Store;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ADL_Client_Tests
{
    [TestClass]
    public class FSPermissions_Tests : Base_Tests
    {


        [TestMethod]
        public void Parse_Rwx_Strings()
        {
            var p0 = new AzureDataLake.Store.FsPermission("rwx");
            Assert.AreEqual(7,p0.Integer);
            Assert.AreEqual(true, p0.Read);
            Assert.AreEqual(true, p0.Write);
            Assert.AreEqual(true, p0.Execute);

            var p1 = new AzureDataLake.Store.FsPermission("---");
            Assert.AreEqual(0, p1.Integer);
            Assert.AreEqual(false, p1.Read);
            Assert.AreEqual(false, p1.Write);
            Assert.AreEqual(false, p1.Execute);

            var p2 = new AzureDataLake.Store.FsPermission("r--");
            Assert.AreEqual(4, p2.Integer);
            Assert.AreEqual(true, p2.Read);
            Assert.AreEqual(false, p2.Write);
            Assert.AreEqual(false, p2.Execute);

            var p3 = new AzureDataLake.Store.FsPermission("-w-");
            Assert.AreEqual(2, p3.Integer);
            Assert.AreEqual(false, p3.Read);
            Assert.AreEqual(true, p3.Write);
            Assert.AreEqual(false, p3.Execute);

            var p4 = new AzureDataLake.Store.FsPermission("--x");
            Assert.AreEqual(1, p4.Integer);
            Assert.AreEqual(false, p4.Read);
            Assert.AreEqual(false, p4.Write);
            Assert.AreEqual(true, p4.Execute);

            var p5 = new AzureDataLake.Store.FsPermission("r-x");
            Assert.AreEqual(5, p5.Integer);
            Assert.AreEqual(true, p5.Read);
            Assert.AreEqual(false, p5.Write);
            Assert.AreEqual(true, p5.Execute);

        }

        [TestMethod]
        public void Verify_Permission_Inversion()
        {
            var p0 = new AzureDataLake.Store.FsPermission("rwx");
            Assert.AreEqual(7, p0.Integer);
            Assert.AreEqual(true, p0.Read);
            Assert.AreEqual(true, p0.Write);
            Assert.AreEqual(true, p0.Execute);

            var p1 = p0.Invert();
            Assert.AreEqual(0, p1.Integer);
            Assert.AreEqual(false, p1.Read);
            Assert.AreEqual(false, p1.Write);
            Assert.AreEqual(false, p1.Execute);

        }

        [TestMethod]
        public void Verify_Permission_And_Operator()
        {
            var p1 = new AzureDataLake.Store.FsPermission("rwx").AndWith( new FsPermission("---"));
            Assert.AreEqual(0, p1.Integer);
            Assert.AreEqual(false, p1.Read);
            Assert.AreEqual(false, p1.Write);
            Assert.AreEqual(false, p1.Execute);

            var p2 = new AzureDataLake.Store.FsPermission("rwx").AndWith(new FsPermission("-w-"));
            Assert.AreEqual(2, p2.Integer);
            Assert.AreEqual(false, p2.Read);
            Assert.AreEqual(true, p2.Write);
            Assert.AreEqual(false, p2.Execute);

            var p3 = new AzureDataLake.Store.FsPermission("rwx").AndWith(new FsPermission("r-x"));
            Assert.AreEqual(5, p3.Integer);
            Assert.AreEqual(true, p3.Read);
            Assert.AreEqual(false, p3.Write);
            Assert.AreEqual(true, p3.Execute);

        }

        [TestMethod]
        public void Verify_Permission_Or_Operator()
        {
            var p1 = new AzureDataLake.Store.FsPermission("rwx").OrWith(new FsPermission("---"));
            Assert.AreEqual(7, p1.Integer);
            Assert.AreEqual(true, p1.Read);
            Assert.AreEqual(true, p1.Write);
            Assert.AreEqual(true, p1.Execute);

            var p2 = new AzureDataLake.Store.FsPermission("---").OrWith(new FsPermission("-w-"));
            Assert.AreEqual(2, p2.Integer);
            Assert.AreEqual(false, p2.Read);
            Assert.AreEqual(true, p2.Write);
            Assert.AreEqual(false, p2.Execute);

            var p3 = new AzureDataLake.Store.FsPermission("r--").OrWith(new FsPermission("--x"));
            Assert.AreEqual(5, p3.Integer);
            Assert.AreEqual(true, p3.Read);
            Assert.AreEqual(false, p3.Write);
            Assert.AreEqual(true, p3.Execute);

        }

        [TestMethod]
        public void Roundtrip_Rwx_Strings()
        {
            var p1 = new AzureDataLake.Store.FsPermission("rwx");
            Assert.AreEqual("rwx", p1.ToRwxString());

            var p2 = new AzureDataLake.Store.FsPermission("---");
            Assert.AreEqual("---", p2.ToRwxString());

            var p3 = new AzureDataLake.Store.FsPermission(5);
            Assert.AreEqual("r-x", p3.ToRwxString());

        }
    }
}