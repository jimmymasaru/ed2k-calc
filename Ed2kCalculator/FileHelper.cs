using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Ed2kCalculator
{
    public class FileHelper
    {
        public List<FileInfo> AllFiles { get; protected set; }
        private readonly List<string> _excludedFilename = new List<string>
        {
            "desktop.ini", "thumbs.db", ".DS_Store"
        };
        private IEnumerable<string> _allFilenames = new List<string>();

        public FileHelper(string path)
        {
            var dir = new DirectoryInfo(path);
            var files = new List<FileInfo>();
            if (dir.Exists)
            {
                files.AddRange(dir.GetFiles("*", SearchOption.AllDirectories));
            }
            AllFiles = files.Where(f => f.Length > 0 &&
                                        !f.Name.StartsWith(".") &&
                                        !f.Name.EndsWith(".lnk", StringComparison.OrdinalIgnoreCase) &&
                                        !_excludedFilename.Contains(f.Name, StringComparer.OrdinalIgnoreCase))
                .OrderBy(f => f.DirectoryName + Path.PathSeparator)
                .ThenBy(f => f.Name)
                .ToList();
        }
    }
}