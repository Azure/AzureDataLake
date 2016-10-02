using AzureDataLake.Store;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ADL_Client_Tests
{
    [TestClass]
    public class FSPermissions_Tests : Base_Tests
    {


        [TestMethod]
        public void Test1()
        {
            var p0 = new AzureDataLake.Store.FSPermission("rwx");
            Assert.AreEqual(7,p0.BitValue);
            Assert.AreEqual(true, p0.Read);
            Assert.AreEqual(true, p0.Write);
            Assert.AreEqual(true, p0.Execute);

            var p1 = new AzureDataLake.Store.FSPermission("---");
            Assert.AreEqual(0, p1.BitValue);
            Assert.AreEqual(false, p1.Read);
            Assert.AreEqual(false, p1.Write);
            Assert.AreEqual(false, p1.Execute);

            var p2 = new AzureDataLake.Store.FSPermission("r--");
            Assert.AreEqual(4, p2.BitValue);
            Assert.AreEqual(true, p2.Read);
            Assert.AreEqual(false, p2.Write);
            Assert.AreEqual(false, p2.Execute);

            var p3 = new AzureDataLake.Store.FSPermission("-w-");
            Assert.AreEqual(2, p3.BitValue);
            Assert.AreEqual(false, p3.Read);
            Assert.AreEqual(true, p3.Write);
            Assert.AreEqual(false, p3.Execute);

            var p4 = new AzureDataLake.Store.FSPermission("--x");
            Assert.AreEqual(1, p4.BitValue);
            Assert.AreEqual(false, p4.Read);
            Assert.AreEqual(false, p4.Write);
            Assert.AreEqual(true, p4.Execute);

            var p5 = new AzureDataLake.Store.FSPermission("r-x");
            Assert.AreEqual(5, p5.BitValue);
            Assert.AreEqual(true, p5.Read);
            Assert.AreEqual(false, p5.Write);
            Assert.AreEqual(true, p5.Execute);

        }

        [TestMethod]
        public void Test2()
        {
            var p0 = new AzureDataLake.Store.FSPermission("rwx");
            Assert.AreEqual(7, p0.BitValue);
            Assert.AreEqual(true, p0.Read);
            Assert.AreEqual(true, p0.Write);
            Assert.AreEqual(true, p0.Execute);

            var p1 = p0.Invert();
            Assert.AreEqual(0, p1.BitValue);
            Assert.AreEqual(false, p1.Read);
            Assert.AreEqual(false, p1.Write);
            Assert.AreEqual(false, p1.Execute);

        }

        [TestMethod]
        public void Test3()
        {
            var p1 = new AzureDataLake.Store.FSPermission("rwx").AndWith( new FSPermission("---"));
            Assert.AreEqual(0, p1.BitValue);
            Assert.AreEqual(false, p1.Read);
            Assert.AreEqual(false, p1.Write);
            Assert.AreEqual(false, p1.Execute);

            var p2 = new AzureDataLake.Store.FSPermission("rwx").AndWith(new FSPermission("-w-"));
            Assert.AreEqual(2, p2.BitValue);
            Assert.AreEqual(false, p2.Read);
            Assert.AreEqual(true, p2.Write);
            Assert.AreEqual(false, p2.Execute);

            var p3 = new AzureDataLake.Store.FSPermission("rwx").AndWith(new FSPermission("r_x"));
            Assert.AreEqual(5, p3.BitValue);
            Assert.AreEqual(true, p3.Read);
            Assert.AreEqual(false, p3.Write);
            Assert.AreEqual(true, p3.Execute);

        }

        [TestMethod]
        public void Test4()
        {
            var p1 = new AzureDataLake.Store.FSPermission("rwx").OrWith(new FSPermission("---"));
            Assert.AreEqual(7, p1.BitValue);
            Assert.AreEqual(true, p1.Read);
            Assert.AreEqual(true, p1.Write);
            Assert.AreEqual(true, p1.Execute);

            var p2 = new AzureDataLake.Store.FSPermission("---").OrWith(new FSPermission("-w-"));
            Assert.AreEqual(2, p2.BitValue);
            Assert.AreEqual(false, p2.Read);
            Assert.AreEqual(true, p2.Write);
            Assert.AreEqual(false, p2.Execute);

            var p3 = new AzureDataLake.Store.FSPermission("r--").OrWith(new FSPermission("--x"));
            Assert.AreEqual(5, p3.BitValue);
            Assert.AreEqual(true, p3.Read);
            Assert.AreEqual(false, p3.Write);
            Assert.AreEqual(true, p3.Execute);

        }

        [TestMethod]
        public void Test5()
        {
            var p1 = new AzureDataLake.Store.FSPermission("rwx");
            Assert.AreEqual("rwx", p1.ToRwxString());

            var p2 = new AzureDataLake.Store.FSPermission("---");
            Assert.AreEqual("---", p2.ToRwxString());

            var p3 = new AzureDataLake.Store.FSPermission(5);
            Assert.AreEqual("r-x", p3.ToRwxString());

        }
    }
}