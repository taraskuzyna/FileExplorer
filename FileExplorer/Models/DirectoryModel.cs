using System;
using System.Collections.Generic;
using System.Linq;

namespace FileExplorer.Models
{
    public class DirectoryModel: IEquatable<DirectoryModel>
    {
        public string Name { get; set; }
        public List<DirectoryModel> Directories { get; set; }
        public List<FileModel> Files { get; set; }

        public bool Equals(DirectoryModel other)
        {
            return this.Name == other.Name &&
                Enumerable.SequenceEqual(this.Directories, other.Directories) &&
                Enumerable.SequenceEqual(this.Files, other.Files);
        }
    }
}
