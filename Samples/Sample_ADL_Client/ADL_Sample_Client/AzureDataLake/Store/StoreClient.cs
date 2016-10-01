using System.Collections.Generic;
using ADL=Microsoft.Azure.Management.DataLake;
using Microsoft.Azure.Management.DataLake.Store;

namespace AzureDataLake.Store
{
    public class StoreClient : ClientBase
    {
        private ADL.Store.DataLakeStoreAccountManagementClient store_mgmt_client;
        private ADL.Store.DataLakeStoreFileSystemManagementClient store_fs_client;

        public StoreClient(string account, AzureDataLake.Authentication.AuthenticatedSession authSession) :
            base(account,authSession)
        {
            store_mgmt_client = new ADL.Store.DataLakeStoreAccountManagementClient(this.AuthenticatedSession.Credentials);
            store_fs_client = new ADL.Store.DataLakeStoreFileSystemManagementClient(this.AuthenticatedSession.Credentials);
        }


        public IList<ADL.Store.Models.FileStatusProperties> List(string path)
        {
            var listSize = 100;
            var result = store_fs_client.FileSystem.ListFileStatus(this.Account, path, listSize);
            var files = result.FileStatuses.FileStatus;
            return files;
        }

        public IEnumerable<RecurseResult> ListPagedRecursive(string path, int pagesize)
        {
            var queue = new Queue<string>();

            queue.Enqueue(path);
            while (queue.Count > 0)
            {
                string cp = queue.Dequeue();
                foreach (var page in ListPaged(cp, pagesize))
                {
                    var r = new RecurseResult();
                    r.Path = cp;
                    r.Children = page;
                    yield return r;

                    foreach (var item in page)
                    {
                        if (item.Type.HasValue && item.Type.Value == ADL.Store.Models.FileType.DIRECTORY)
                        {
                            queue.Enqueue(cp + "/" +item.PathSuffix);
                        }
                    }
                }
            }
        }

        public IEnumerable<IList<ADL.Store.Models.FileStatusProperties>> ListPaged(string path, int pagesize)
        {
            string after = null;
            while (true)
            {
                var result = store_fs_client.FileSystem.ListFileStatus(this.Account, path, pagesize,after);

                if (result.FileStatuses.FileStatus.Count > 0)
                {
                    yield return result.FileStatuses.FileStatus;
                    after = result.FileStatuses.FileStatus[result.FileStatuses.FileStatus.Count - 1].PathSuffix;
                }
                else
                {
                    break;
                }

            }
        }
        
        public void CreateDirectory(string path)
        {
            var result = store_fs_client.FileSystem.Mkdirs(this.Account, path);
        }

        public void Delete(string path)
        {
            var result = store_fs_client.FileSystem.Delete(this.Account, path);
        }

        public void Delete(string path, bool recursive)
        {
            var result = store_fs_client.FileSystem.Delete(this.Account, path, recursive );
        }

        public void CreateFile(string path, byte[] bytes, bool overwrite)
        {
            var memstream = new System.IO.MemoryStream(bytes);
            store_fs_client.FileSystem.Create(this.Account, path,memstream,overwrite);
        }

        public void CreateFile(string path, string content, bool overwrite)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(content);
            this.CreateFile(path, bytes, overwrite);
        }

        public ADL.Store.Models.FileStatusResult GetFileInformation( string path )
        {
            var info = store_fs_client.FileSystem.GetFileStatus(this.Account, path);
            return info;
        }

        public ADL.Store.Models.FileStatusResult TryGetFileInformation(string path)
        {
            try
            {
                var info = store_fs_client.FileSystem.GetFileStatus(this.Account, path);
                return info;
            }
            catch (Microsoft.Rest.Azure.CloudException e)
            {
                if (e.Response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                throw;
            }
        }

        public bool Exists(string path)
        {
            var info = this.TryGetFileInformation(path);
            return (info != null);
        }

        public ADL.Store.Models.AclStatus GetPermissions(string path)
        {
            var acl = this.store_fs_client.FileSystem.GetAclStatus(this.Account, path);
            var acl2 = acl.AclStatus;
            return acl2;
        }

        public void ModifyACLs(string path,string perms)
        {
            this.store_fs_client.FileSystem.ModifyAclEntries(this.Account,  path, perms);
        }
    }
}