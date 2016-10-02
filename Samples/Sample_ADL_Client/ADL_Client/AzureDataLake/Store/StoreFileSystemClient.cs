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

        public IEnumerable<FsPage> ListFilesRecursive(FsPath path, int pagesize)
        {
            var queue = new Queue<FsPath>();
            queue.Enqueue(path);

            while (queue.Count > 0)
            {
                FsPath cur_path = queue.Dequeue();

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

        public IEnumerable<FsPage> ListFiles(FsPath path, int pagesize)
        {
            string after = null;
            while (true)
            {
                var result = _adls_filesys_rest_client.FileSystem.ListFileStatus(this.Account, path.ToString(), pagesize,after);

                if (result.FileStatuses.FileStatus.Count > 0)
                {
                    var page = new FsPage();
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
        
        public void CreateDirectory(FsPath path)
        {
            var result = _adls_filesys_rest_client.FileSystem.Mkdirs(this.Account, path.ToString());
        }

        public void Delete(FsPath path)
        {
            var result = _adls_filesys_rest_client.FileSystem.Delete(this.Account, path.ToString());
        }

        public void Delete(FsPath path, bool recursive)
        {
            var result = _adls_filesys_rest_client.FileSystem.Delete(this.Account, path.ToString(), recursive );
        }

        public void CreateFile(FsPath path, byte[] bytes, bool overwrite)
        {
            var memstream = new System.IO.MemoryStream(bytes);
            _adls_filesys_rest_client.FileSystem.Create(this.Account, path.ToString(),memstream,overwrite);
        }

        public void CreateFile(FsPath path, string content, bool overwrite)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(content);
            this.CreateFile(path, bytes, overwrite);
        }

        public ADL.Store.Models.FileStatusResult GetFileInformation( FsPath path )
        {
            var info = _adls_filesys_rest_client.FileSystem.GetFileStatus(this.Account, path.ToString());
            return info;
        }

        public ADL.Store.Models.FileStatusResult TryGetFileInformation(FsPath path)
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

        public bool Exists(FsPath path)
        {
            var info = this.TryGetFileInformation(path);
            return (info != null);
        }

        public AzureDataLake.Store.FsAcl GetPermissions(FsPath path)
        {
            var acl = this._adls_filesys_rest_client.FileSystem.GetAclStatus(this.Account, path.ToString());
            var acl2 = acl.AclStatus;

            var y = new AzureDataLake.Store.FsAcl(acl2);

            return y;
        }

        public void ModifyACLs(FsPath path,string perms)
        {
            this._adls_filesys_rest_client.FileSystem.ModifyAclEntries(this.Account,  path.ToString(), perms);
        }

        public System.IO.Stream OpenFileForReadBinary(FsPath path)
        {
            return this._adls_filesys_rest_client.FileSystem.Open(this.Account, path.ToString());
        }

        public System.IO.StreamReader OpenFileForReadText(FsPath path)
        {
            var s = this._adls_filesys_rest_client.FileSystem.Open(this.Account, path.ToString());
            return new System.IO.StreamReader(s);
        }

        public System.IO.Stream OpenFileForReadBinary(FsPath path, long offset, long bytesToRead)
        {
            return this._adls_filesys_rest_client.FileSystem.Open(this.Account, path.ToString(), bytesToRead, offset);
        }
    }
}