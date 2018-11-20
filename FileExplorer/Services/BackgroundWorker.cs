using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileExplorer.Services
{
    public class BackgroundWorker : IBackgroundWorker
    {
        private readonly Dictionary<string, Func<Task>> tasks = new Dictionary<string, Func<Task>>();

        public void Add(string key, Func<Task> newTask)
        {
            lock (tasks)
            {
                if (tasks.All(x => x.Key != key))
                {
                    tasks.Add(key, newTask);
                }
            }
            Start();
        }

        public void Start()
        {
            lock (tasks)
            {
                while (tasks.Any())
                {
                    KeyValuePair<string, Func<Task>> nextTask = tasks.FirstOrDefault();
                    new Task(async () =>
                    {
                        try
                        {
                            await nextTask.Value();
                        }
                        catch (OperationCanceledException)
                        {
                            return;
                        }
                        catch (Exception)
                        {

                        }
                    }).Start();
                    tasks.Remove(nextTask.Key);
                }
            }
        }
    }
}
