using FileExplorer.Models;
using System.Threading.Tasks;

namespace FileExplorer.Hubs
{
    public interface IFileSystemClient
    {
        Task ReceiveMessage(DirectoryModel model);
    }
}
