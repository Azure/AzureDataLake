using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ADL_Client_Tests.Analytics
{
    [TestClass]
    public class Analytics_Catalog_Tests : Base_Tests
    {
        [TestMethod]
        public void List_Databases()
        {
            this.Initialize();
            foreach (var page in this.adla_catalog_client.ListDatabasesPaged())
            {
                foreach (var db in page)
                {
                    System.Console.WriteLine("DB {0}",db.Name);
                }
            }
        }

    }

}
