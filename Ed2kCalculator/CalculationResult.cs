using System;
using System.IO;

namespace Ed2kCalculator
{
    public class CalculationResult
    {
        public FileInfo File { get; protected set; }
        public string Hash { get; protected set; }

        // ReSharper disable once InconsistentNaming
        public string Ed2kLink => $"ed2k://|file|{Uri.EscapeUriString(File.Name)}|{File.Length}|{Hash}|/";

        public CalculationResult(FileInfo file, string hash)
        {
            if (file == null) throw new ArgumentNullException(nameof(file));
            if (hash == null) throw new ArgumentNullException(nameof(hash));
            File = file;
            Hash = hash;
        }

        public override string ToString()
        {
            return $"{File.FullName}\t{File.Length}\t{File.LastWriteTime:yyyy-MM-dd HH:mm:ss}\t{Hash}";
        }
    }
}