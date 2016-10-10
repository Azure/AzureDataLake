using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ADL_Client_Tests.Store
{
    [TestClass]
    public class FSUri_Tests : Base_Tests
    {
        [TestMethod]
        public void FSPath_Construct_from_string()
        {
            var u0 = new AzureDataLake.Store.FsUri("adl://ACCOUNT.azuredatalakestore.net/users/USER1");
            Assert.AreEqual("account",u0.Account);
            Assert.AreEqual("/users/USER1", u0.Path);

            var u1 = new AzureDataLake.Store.FsUri("adl://ACCOUNT.azuredatalakestore.net/users/");
            Assert.AreEqual("account", u1.Account);
            Assert.AreEqual("/users/", u1.Path);

            var u2 = new AzureDataLake.Store.FsUri("adl://ACCOUNT.azuredatalakestore.net/users");
            Assert.AreEqual("account", u2.Account);
            Assert.AreEqual("/users", u2.Path);

            var u3 = new AzureDataLake.Store.FsUri("adl://ACCOUNT.azuredatalakestore.net/");
            Assert.AreEqual("account", u3.Account);
            Assert.AreEqual("/", u3.Path);

            var u4 = new AzureDataLake.Store.FsUri("adl://ACCOUNT.azuredatalakestore.net");
            Assert.AreEqual("account", u4.Account);
            Assert.AreEqual("/", u4.Path);

        }

        [TestMethod]
        public void FSPath_Construct_from_accountg_and_path()
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


        [TestMethod]
        public void FSPath_verify_constructor_normalizes_account_name()
        {
            var u0 = new AzureDataLake.Store.FsUri("adl://ACCOUNT.azuredatalakestore.net/users/USER1");
            Assert.AreEqual("adl://account.azuredatalakestore.net/users/USER1", u0.ToUriString());
            Assert.AreEqual("account", u0.Account);


            var u1 = new AzureDataLake.Store.FsUri("swebhdfs://ACCOUNT.azuredatalakestore.net/users/USER1");
            Assert.AreEqual("adl://account.azuredatalakestore.net/users/USER1", u1.ToUriString());
            Assert.AreEqual("account", u1.Account);

            var u2 = new AzureDataLake.Store.FsUri("webhdfs://ACCOUNT.azuredatalakestore.net/users/USER1");
            Assert.AreEqual("adl://account.azuredatalakestore.net/users/USER1", u2.ToUriString());
            Assert.AreEqual("account", u2.Account);

            var u3 = new AzureDataLake.Store.FsUri("adl://ACCOUNT.azuredatalakestore.net");
            Assert.AreEqual("adl://account.azuredatalakestore.net/", u3.ToUriString());
            Assert.AreEqual("account", u3.Account);

            var u4 = new AzureDataLake.Store.FsUri("adl://ACCOUNT.azuredatalakestore.net/");
            Assert.AreEqual("adl://account.azuredatalakestore.net/", u4.ToUriString());
            Assert.AreEqual("account", u4.Account);

            var u5 = new AzureDataLake.Store.FsUri("adl://ACCOUNT.azuredatalakestore.net\\");
            Assert.AreEqual("adl://account.azuredatalakestore.net/", u5.ToUriString());
            Assert.AreEqual("account", u5.Account);

        }
    }
}
