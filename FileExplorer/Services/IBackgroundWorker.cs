using System;
using System.Threading;
using System.Threading.Tasks;

namespace FileExplorer.Services
{
    public interface IBackgroundWorker
    {
        void Add(string key, Func<Task> newTask);
        void Start();
    }
}
