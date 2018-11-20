using System;

namespace FileExplorer.Models
{
    public class FileModel : IEquatable<FileModel>
    {
        public string Name { get; set; }

        public bool Equals(FileModel other)
        {
            return this.Name == other.Name;
        }
    }
}
