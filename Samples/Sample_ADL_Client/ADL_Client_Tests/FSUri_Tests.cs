using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ADL_Client_Tests
{
    [TestClass]
    public class FSUri_Tests : Base_Tests
    {
        [TestMethod]
        public void FSPath_Constructor_Root_1()
        {
            var u0 = new AzureDataLake.Store.FsUri("adl://ACCOUNT.azuredatalakestore.net/users/USER1");
            Assert.AreEqual("account",u0.Account);
            Assert.AreEqual("/users/USER1", u0.Path);
        }

        [TestMethod]
        public void FSPath_Constructor_Root_2()
        {
            var u0 = new AzureDataLake.Store.FsUri("adl://ACCOUNT.azuredatalakestore.net/users/");
            Assert.AreEqual("account", u0.Account);
            Assert.AreEqual("/users/", u0.Path);
        }

        [TestMethod]
        public void FSPath_Constructor_Root_3()
        {
            var u0 = new AzureDataLake.Store.FsUri("adl://ACCOUNT.azuredatalakestore.net/users");
            Assert.AreEqual("account", u0.Account);
            Assert.AreEqual("/users", u0.Path);
        }

        [TestMethod]
        public void FSPath_Constructor_Root_4()
        {
            var u0 = new AzureDataLake.Store.FsUri("adl://ACCOUNT.azuredatalakestore.net/");
            Assert.AreEqual("account", u0.Account);
            Assert.AreEqual("/", u0.Path);
        }

        [TestMethod]
        public void FSPath_Constructor_Root_5()
        {
            var u0 = new AzureDataLake.Store.FsUri("adl://ACCOUNT.azuredatalakestore.net");
            Assert.AreEqual("account", u0.Account);
            Assert.AreEqual("/", u0.Path);
        }

        [TestMethod]
        public void FSPath_Constructor_Root_6()
        {
            var u0 = new AzureDataLake.Store.FsUri("ACCOUNT",null);
            Assert.AreEqual("account", u0.Account);
            Assert.AreEqual("/", u0.Path);

            var u1 = new AzureDataLake.Store.FsUri("ACCOUNT", "");
            Assert.AreEqual("account", u1.Account);
            Assert.AreEqual("/", u1.Path);

            var u2 = new AzureDataLake.Store.FsUri("ACCOUNT", "\\");
            Assert.AreEqual("account", u2.Account);
            Assert.AreEqual("/", u2.Path);

            var u3 = new AzureDataLake.Store.FsUri("ACCOUNT", "/");
            Assert.AreEqual("account", u3.Account);
            Assert.AreEqual("/", u3.Path);

        }


    }
}
