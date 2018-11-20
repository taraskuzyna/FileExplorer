using FileExplorer.Models;
using FileExplorer.Services;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace FileExplorer.Hubs
{
    public class FileSystemHub : Hub<IFileSystemClient>
    {
        private readonly IFileSystemService fileSystemService;

        public FileSystemHub(IFileSystemService fileSystemService)
        {
            this.fileSystemService = fileSystemService;
        }

        public override Task OnConnectedAsync()
        {
            base.OnConnectedAsync();
            var FileExplorer = fileSystemService.GetFileSystem();
            return SendMessageToCaller(FileExplorer);
        }

        public Task SendMessageToCaller(DirectoryModel message)
        {
            return Clients.Caller.ReceiveMessage(message);
        }
    }
}
