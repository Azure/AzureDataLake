using System;
using System.Linq;
using AzureDataLake.Store;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ADL_Client_Tests
{
    [TestClass]
    public class Store_Filesystem_Tests : Base_Tests
    {
        [TestMethod]
        public void List_Files_Recursive()
        {
            this.Initialize();

            int page_count = 0;
            int child_count = 0;

            var pages = this.adls_fs_client.ListFilesRecursive(AzureDataLake.Store.FsPath.Root, 4);
            foreach (var page in pages)
            {
                foreach (var child in page.FileItems)
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

            var pages = this.adls_fs_client.ListFiles(AzureDataLake.Store.FsPath.Root, 4);
            foreach (var page in pages)
            {
                foreach (var fileitem in page.FileItems)
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
            var cfo = new AzureDataLake.Store.CreateFileOptions();
            cfo.Overwrite = true;
            this.adls_fs_client.CreateFileWithContent(fname, "HelloWorld", cfo);
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

    }
}

