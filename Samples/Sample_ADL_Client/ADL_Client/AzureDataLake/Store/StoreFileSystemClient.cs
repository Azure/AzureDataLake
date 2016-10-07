using System.Collections.Generic;
using System.Linq;
using ADL=Microsoft.Azure.Management.DataLake;
using Microsoft.Azure.Management.DataLake.Store;
using Microsoft.Azure.Management.DataLake.Store.Models;

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

                    foreach (var item in page.FileItems)
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

                    page.FileItems = result.FileStatuses.FileStatus;
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

        public void CreateFile(FsPath path, byte[] bytes, CreateFileOptions options)
        {
            var memstream = new System.IO.MemoryStream(bytes);
            _adls_filesys_rest_client.FileSystem.Create(this.Account, path.ToString(),memstream,options.Overwrite);
        }

        public void CreateFile(FsPath path, string content, CreateFileOptions options)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(content);
            this.CreateFile(path, bytes, options);
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

        public bool ExistsFile(FsPath path)
        {
            var info = this.TryGetFileInformation(path);
            if (info == null)
            {
                return false;
            }

            if (!info.FileStatus.Type.HasValue)
            {
                throw new System.ArgumentException();
            }

            if (info.FileStatus.Type.Value == FileType.DIRECTORY)
            {
                return false;
            }

            return true;

        }

        public bool ExistsFolder(FsPath path)
        {
            var info = this.TryGetFileInformation(path);
            if (info == null)
            {
                return false;
            }

            if (!info.FileStatus.Type.HasValue)
            {
                throw new System.ArgumentException();
            }

            if (info.FileStatus.Type.Value == FileType.FILE)
            {
                return false;
            }

            return true;

        }


        public AzureDataLake.Store.FsAcl GetPermissions(FsPath path)
        {
            var acl_result = this._adls_filesys_rest_client.FileSystem.GetAclStatus(this.Account, path.ToString());
            var acl_status = acl_result.AclStatus;

            var fs_acl = new AzureDataLake.Store.FsAcl(acl_status);

            return fs_acl;
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

        public void Upload(string src_local_filename, FsPath dest_remote_filename, UploadOptions options)
        {
            var parameters = new ADL.StoreUploader.UploadParameters(src_local_filename, dest_remote_filename.ToString(), this.Account, isOverwrite: options.Force);
            var frontend = new ADL.StoreUploader.DataLakeStoreFrontEndAdapter(this.Account, this._adls_filesys_rest_client);
            var uploader = new ADL.StoreUploader.DataLakeStoreUploader(parameters, frontend);
            uploader.Execute();
        }

        public void Download(FsPath src_remote_filename, string destPath, DownloadOptions options)
        {
            using (var stream = this._adls_filesys_rest_client.FileSystem.Open(this.Account, src_remote_filename.ToString()))
            {
                var filemode = options.Append ? System.IO.FileMode.Append : System.IO.FileMode.Create;
                using (var fileStream = new System.IO.FileStream(destPath, filemode))
                {
                    stream.CopyTo(fileStream);
                }
            }
        }

        public void Append(FsPage file,string content)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(content);
            using (var stream = new System.IO.MemoryStream(bytes))
            {
                this._adls_filesys_rest_client.FileSystem.Append(this.Account, file.ToString(), stream);
            }
        }

        public void Append(FsPage file, byte[] bytes)
        {
            using (var stream = new System.IO.MemoryStream(bytes))
            {
                this._adls_filesys_rest_client.FileSystem.Append(this.Account, file.ToString(), stream);
            }
        }

        public void Concatenate(IEnumerable<FsPage> src_files, FsPath dest_path)
        {
            var src_file_strings = src_files.Select(i => i.ToString()).ToList();
            this._adls_filesys_rest_client.FileSystem.Concat(this.Account, dest_path.ToString(), src_file_strings);

        }
    }
}