using System.Collections.Generic;
using ADL=Microsoft.Azure.Management.DataLake;
using Microsoft.Azure.Management.DataLake.Store;

namespace AzureDataLake.Store
{
    public class StoreFileSystemClient : AccountClientBase
    {
        private ADL.Store.DataLakeStoreFileSystemManagementClient _adls_filesys_rest_client;

        public StoreFileSystemClient(string account, AzureDataLake.Authentication.AuthenticatedSession authSession) :
            base(account,authSession)
        {
            _adls_filesys_rest_client = new ADL.Store.DataLakeStoreFileSystemManagementClient(this.AuthenticatedSession.Credentials);
        }

        public IEnumerable<FSPage> ListFilesRecursive(string path, int pagesize)
        {
            var queue = new Queue<string>();
            queue.Enqueue(path);

            while (queue.Count > 0)
            {
                string cp = queue.Dequeue();

                foreach (var page in ListFiles(cp, pagesize))
                {
                    yield return page;

                    foreach (var item in page.Children)
                    {
                        if (item.Type.HasValue && item.Type.Value == ADL.Store.Models.FileType.DIRECTORY)
                        {
                            queue.Enqueue(cp + "/" +item.PathSuffix);
                        }
                    }
                }
            }
        }

        public IEnumerable<FSPage> ListFiles(string path, int pagesize)
        {
            string after = null;
            while (true)
            {
                var result = _adls_filesys_rest_client.FileSystem.ListFileStatus(this.Account, path, pagesize,after);

                if (result.FileStatuses.FileStatus.Count > 0)
                {
                    var page = new FSPage();
                    page.Path = path;

                    page.Children = result.FileStatuses.FileStatus;
                    yield return page;
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
            var result = _adls_filesys_rest_client.FileSystem.Mkdirs(this.Account, path);
        }

        public void Delete(string path)
        {
            var result = _adls_filesys_rest_client.FileSystem.Delete(this.Account, path);
        }

        public void Delete(string path, bool recursive)
        {
            var result = _adls_filesys_rest_client.FileSystem.Delete(this.Account, path, recursive );
        }

        public void CreateFile(string path, byte[] bytes, bool overwrite)
        {
            var memstream = new System.IO.MemoryStream(bytes);
            _adls_filesys_rest_client.FileSystem.Create(this.Account, path,memstream,overwrite);
        }

        public void CreateFile(string path, string content, bool overwrite)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(content);
            this.CreateFile(path, bytes, overwrite);
        }

        public ADL.Store.Models.FileStatusResult GetFileInformation( string path )
        {
            var info = _adls_filesys_rest_client.FileSystem.GetFileStatus(this.Account, path);
            return info;
        }

        public ADL.Store.Models.FileStatusResult TryGetFileInformation(string path)
        {
            try
            {
                var info = _adls_filesys_rest_client.FileSystem.GetFileStatus(this.Account, path);
                return info;
            }
            catch (Microsoft.Azure.Management.DataLake.Store.Models.AdlsErrorException ex)
            {
                if (ex.Body.RemoteException is Microsoft.Azure.Management.DataLake.Store.Models.AdlsFileNotFoundException || 
                    ex.Response.StatusCode == System.Net.HttpStatusCode.NotFound)
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
            var acl = this._adls_filesys_rest_client.FileSystem.GetAclStatus(this.Account, path);
            var acl2 = acl.AclStatus;
            return acl2;
        }

        public void ModifyACLs(string path,string perms)
        {
            this._adls_filesys_rest_client.FileSystem.ModifyAclEntries(this.Account,  path, perms);
        }

        public System.IO.Stream OpenFileForReadBinary(string path)
        {
            return this._adls_filesys_rest_client.FileSystem.Open(this.Account, path);
        }

        public System.IO.StreamReader OpenFileForReadText(string path)
        {
            var s = this._adls_filesys_rest_client.FileSystem.Open(this.Account, path);
            return new System.IO.StreamReader(s);
        }

        public System.IO.Stream OpenFileForReadBinary(string path, long offset, long bytesToRead)
        {
            return this._adls_filesys_rest_client.FileSystem.Open(this.Account, path, bytesToRead, offset);
        }
    }
}