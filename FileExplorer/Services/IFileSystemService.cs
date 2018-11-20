using FileExplorer.Models;
using System.Threading.Tasks;

namespace FileExplorer.Services
{
    public interface IFileSystemService
    {
        DirectoryModel GetFileSystem();
        Task ReadFileSystem();
    }
}
