using FileExplorer.Hubs;
using FileExplorer.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FileExplorer.Services
{
    public class FileSystemService : IFileSystemService
    {
        private const string CacheKey = "FileExplorer";

        private readonly IOptions<AppSettings> options;
        private readonly IHubContext<FileSystemHub, IFileSystemClient> fileSystemHub;
        private readonly IMemoryCache memoryCache;
        private readonly IBackgroundWorker worker;

        public FileSystemService(IOptions<AppSettings> options, 
            IHubContext<FileSystemHub,IFileSystemClient > fileSystemHub,
            IMemoryCache memoryCache,
            IBackgroundWorker worker)
        {

            this.options = options;
            this.fileSystemHub = fileSystemHub;
            this.memoryCache = memoryCache;
            this.worker = worker;
        }

        public DirectoryModel GetFileSystem()
        {
            
            return memoryCache.Get<DirectoryModel>(CacheKey);
        }

        public async Task ReadFileSystem()
        {
            var root = options.Value.RootDirectoryPath;
            var cashedFileSystem = memoryCache.Get<DirectoryModel>(CacheKey);
            var updatedFileSystem = GetDirectoryContent(root);
            if (cashedFileSystem == null || !updatedFileSystem.Equals(cashedFileSystem))
                await fileSystemHub.Clients.All.ReceiveMessage(updatedFileSystem);
            memoryCache.Set(CacheKey, updatedFileSystem);
            await Task.Delay(1000);
            worker.Add("read", ReadFileSystem);
            System.Diagnostics.Debug.WriteLine(DateTime.Now);
        }

        private DirectoryModel GetDirectoryContent(string path)
        {
            var dirInfo = new DirectoryInfo(path);
            if (!dirInfo.Exists)
                throw new DirectoryNotFoundException();
            return new DirectoryModel
            {
                Name = dirInfo.Name,
                Directories = dirInfo.GetDirectories().Select(x => GetDirectoryContent(x.FullName)).ToList(),
                Files = dirInfo.GetFiles().Select(fileInfo => new FileModel{Name = fileInfo.Name}).ToList(),
            };
        }
    }
}
