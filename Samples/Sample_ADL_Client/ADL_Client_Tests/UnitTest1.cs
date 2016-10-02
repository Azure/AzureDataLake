using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ADL_Client_Tests
{
    [TestClass]
    public class UnitTest1
    {
        public bool init;

        public AzureDataLake.Authentication.AuthenticatedSession auth_session;
        public AzureDataLake.Analytics.AnalyticsJobClient adla_job_client;
        public AzureDataLake.Analytics.AnalyticsManagementClient adla_mgmt_client;
        public AzureDataLake.Store.StoreFileSystemClient adls_fs_client;
        public AzureDataLake.Store.StoreManagementClient adls_mgmt_client;
        public AzureDataLake.Subscription sub;
 

        [TestMethod]
        public void TestMethod1()
        {
            this.initialize();
            var directory = AzureDataLake.Authentication.Directory.Resolve("microsoft.com");
        }

        [TestMethod]
        public void TestMethod2()
        {
            this.initialize();
            var adls_accounts = this.adls_mgmt_client.ListStores();
            foreach (var a in adls_accounts)
            {
                System.Console.WriteLine("Store {0} ", a.Name);
            }
        }

        [TestMethod]
        public void TestMethod3()
        {
            this.initialize();
            var adla_accounts = this.adls_mgmt_client.ListAccounts();
            foreach (var a in adla_accounts)
            {
                System.Console.WriteLine("Analytics {0} ", a.Name);
            }

        }

        [TestMethod]
        public void TestMethod4()
        {
            this.initialize();
            var getjobs_options = new AzureDataLake.Analytics.GetJobListPagedOptions();
            getjobs_options.Top = 30;
            getjobs_options.OrderByField = AzureDataLake.Analytics.JobOrderByField.DegreeOfParallelism;
            getjobs_options.OrderByDirection = AzureDataLake.Analytics.JobOrderByDirection.Descending;
            var jobs = this.adla_job_client.GetJobList(getjobs_options).ToArray();

            foreach (var job in jobs)
            {
                //var job2 = AnalyticsClient.GetJob(job.JobId.Value);
                System.Console.WriteLine("submitter{0} dop {1}", job.Submitter, job.DegreeOfParallelism);
            }
        }


        [TestMethod]
        public void TestMethod5()
        {
            this.initialize();
            var sjo = new AzureDataLake.Analytics.SubmitJobOptions();
            sjo.ScriptText = "FOOBAR";
            sjo.JobName = "Test Job";
            var ji = this.adla_job_client.SubmitJob(sjo);

            System.Console.WriteLine("{0} {1} {2}", ji.Name, ji.JobId, ji.SubmitTime);

            var ji2 = this.adla_job_client.GetJob(ji.JobId.Value);

            Assert.AreEqual( ji.Name , ji2.Name);

        }

        [TestMethod]
        public void TestMethod6()
        {
            this.initialize();

            int page_count = 0;
            int child_count = 0;

            var pages = this.adls_fs_client.ListFilesRecursive("/", 4);
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
        public void TestMethod7()
        {
            this.initialize();

            int page_count = 0;
            int child_count = 0;

            var pages = this.adls_fs_client.ListFiles("/", 4);
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
        public void TestMethod8()
        {
            this.initialize();

            string dir = "/test_adl_demo_client";

            if (this.adls_fs_client.Exists(dir))
            {
                this.adls_fs_client.Delete(dir,true);
            }

            this.adls_fs_client.CreateDirectory(dir);

            if (!this.adls_fs_client.Exists(dir))
            {
                Assert.Fail();
            }

            
            string fname = dir + "/" + "foo.txt";
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

        public void initialize()
        {
            if (this.init == false)
            {
                this.auth_session = new AzureDataLake.Authentication.AuthenticatedSession("ADL_Demo_Client");
                auth_session.Authenticate();

                string store_account = "datainsightsadhoc";
                string analytics_account = "datainsightsadhoc";
                string subid = "045c28ea-c686-462f-9081-33c34e871ba3";
                this.sub = new AzureDataLake.Subscription(subid);
                this.init = true;

                this.adls_fs_client= new AzureDataLake.Store.StoreFileSystemClient(store_account, auth_session);
                this.adla_job_client= new AzureDataLake.Analytics.AnalyticsJobClient(analytics_account, auth_session);
                this.adls_mgmt_client= new AzureDataLake.Store.StoreManagementClient(sub, auth_session);
                this.adla_mgmt_client= new AzureDataLake.Analytics.AnalyticsManagementClient(sub, auth_session);

            }
        }
    }
}
