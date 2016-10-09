using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ADL_Client_Tests
{
    [TestClass]
    public class FSPath_Tests : Base_Tests
    {
        [TestMethod]
        public void FSPath_Constructor_Root()
        {
            var p0 = new AzureDataLake.Store.FsPath("/");
            Assert.AreEqual("/",p0.ToString());

            var p1 = AzureDataLake.Store.FsPath.Root;
            Assert.AreEqual("/", p1.ToString());

            Assert.AreEqual(p1.ToString(), p1.ToString());
        }

        [TestMethod]
        public void FSPath_Construct_with_no_parameters()
        {
            bool caught = false;
            try
            {
                var p0 = new AzureDataLake.Store.FsPath(null);
            }
            catch (System.ArgumentNullException exc)
            {
                caught = true;

            }
            Assert.IsTrue(caught);
        }

        [TestMethod]
        public void FSPath_Construct_empty_string_should_fail()
        {
            bool caught = false;
            try
            {
                var p0 = new AzureDataLake.Store.FsPath("");
            }
            catch (System.ArgumentOutOfRangeException exc)
            {
                caught = true;

            }
            Assert.IsTrue(caught);
        }


        [TestMethod]
        public void FSPath_Constructor_Combine_Rooted()
        {
            var p0 = new AzureDataLake.Store.FsPath("/");
            var p1 = p0.Append("foo");
            var p2 = p0.Append("foo/bar");
            Assert.AreEqual("/", p0.ToString());
            Assert.AreEqual("/foo", p1.ToString());
            Assert.AreEqual("/foo/bar", p2.ToString());
            Assert.IsTrue(p0.IsRooted);
            Assert.IsTrue(p1.IsRooted);
            Assert.IsTrue(p2.IsRooted);
        }

        [TestMethod]
        public void FSPath_Constructor_Combine_Unrooted()
        {
            var p0 = new AzureDataLake.Store.FsPath("test");
            var p1 = p0.Append("foo");
            var p2 = p0.Append("foo/bar");
            Assert.AreEqual("test", p0.ToString());
            Assert.AreEqual("test/foo", p1.ToString());
            Assert.AreEqual("test/foo/bar", p2.ToString());
            Assert.IsFalse(p0.IsRooted);
            Assert.IsFalse(p1.IsRooted);
            Assert.IsFalse(p2.IsRooted);
        }

    }

}
