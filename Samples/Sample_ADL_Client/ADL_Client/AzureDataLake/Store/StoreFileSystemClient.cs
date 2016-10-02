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

        public IEnumerable<FSPage> ListFilesRecursive(FSPath path, int pagesize)
        {
            var queue = new Queue<FSPath>();
            queue.Enqueue(path);

            while (queue.Count > 0)
            {
                FSPath cur_path = queue.Dequeue();

                foreach (var page in ListFiles(cur_path, pagesize))
                {
                    yield return page;

                    foreach (var item in page.Children)
                    {
                        if (item.Type.HasValue && item.Type.Value == ADL.Store.Models.FileType.DIRECTORY)
                        {
                            var new_path = cur_path.Append(item.PathSuffix);
                            queue.Enqueue(new_path);
                        }
                    }
                }
            }
        }

        public IEnumerable<FSPage> ListFiles(FSPath path, int pagesize)
        {
            string after = null;
            while (true)
            {
                var result = _adls_filesys_rest_client.FileSystem.ListFileStatus(this.Account, path.ToString(), pagesize,after);

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
        
        public void CreateDirectory(FSPath path)
        {
            var result = _adls_filesys_rest_client.FileSystem.Mkdirs(this.Account, path.ToString());
        }

        public void Delete(FSPath path)
        {
            var result = _adls_filesys_rest_client.FileSystem.Delete(this.Account, path.ToString());
        }

        public void Delete(FSPath path, bool recursive)
        {
            var result = _adls_filesys_rest_client.FileSystem.Delete(this.Account, path.ToString(), recursive );
        }

        public void CreateFile(FSPath path, byte[] bytes, bool overwrite)
        {
            var memstream = new System.IO.MemoryStream(bytes);
            _adls_filesys_rest_client.FileSystem.Create(this.Account, path.ToString(),memstream,overwrite);
        }

        public void CreateFile(FSPath path, string content, bool overwrite)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(content);
            this.CreateFile(path, bytes, overwrite);
        }

        public ADL.Store.Models.FileStatusResult GetFileInformation( FSPath path )
        {
            var info = _adls_filesys_rest_client.FileSystem.GetFileStatus(this.Account, path.ToString());
            return info;
        }

        public ADL.Store.Models.FileStatusResult TryGetFileInformation(FSPath path)
        {
            try
            {
                var info = _adls_filesys_rest_client.FileSystem.GetFileStatus(this.Account, path.ToString());
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

        public bool Exists(FSPath path)
        {
            var info = this.TryGetFileInformation(path);
            return (info != null);
        }

        public ADL.Store.Models.AclStatus GetPermissions(FSPath path)
        {
            var acl = this._adls_filesys_rest_client.FileSystem.GetAclStatus(this.Account, path.ToString());
            var acl2 = acl.AclStatus;
            return acl2;
        }

        public void ModifyACLs(FSPath path,string perms)
        {
            this._adls_filesys_rest_client.FileSystem.ModifyAclEntries(this.Account,  path.ToString(), perms);
        }

        public System.IO.Stream OpenFileForReadBinary(FSPath path)
        {
            return this._adls_filesys_rest_client.FileSystem.Open(this.Account, path.ToString());
        }

        public System.IO.StreamReader OpenFileForReadText(FSPath path)
        {
            var s = this._adls_filesys_rest_client.FileSystem.Open(this.Account, path.ToString());
            return new System.IO.StreamReader(s);
        }

        public System.IO.Stream OpenFileForReadBinary(FSPath path, long offset, long bytesToRead)
        {
            return this._adls_filesys_rest_client.FileSystem.Open(this.Account, path.ToString(), bytesToRead, offset);
        }
    }
}