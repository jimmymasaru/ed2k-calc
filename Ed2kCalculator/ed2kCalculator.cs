using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace Ed2kCalculator
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class ed2kCalculator
    {
        private readonly MD4 _md4 = new MD4();

        public const int ED2K_CHUNK_SIZE = 9728000;

        public IEnumerable<CalculationResult> Calc(IEnumerable<FileInfo> files)
        {
            return files.Select(Calc);
        }

        public CalculationResult Calc(FileInfo file)
        {
            CalculationResult result = null;
            using (var stream = file.OpenRead())
            {
                var totalChunkCount = Math.Ceiling(file.Length * 1.0 / ED2K_CHUNK_SIZE);
                var chunkCount = 0;
                var bufferLength = 0;
                var buffer = new byte[ED2K_CHUNK_SIZE];
                var md4HashedBytes = new List<byte>();
                string hash = null;
                while ((bufferLength = stream.Read(buffer, 0, ED2K_CHUNK_SIZE)) > 0)
                {
                    ++chunkCount;
                    var chunkMd4HashedBytes = _md4.GetByteHashFromBytes(buffer.Take(bufferLength).ToArray());
                    md4HashedBytes.AddRange(chunkMd4HashedBytes);
                    Program.ClearLine();
                    Console.Write($"{chunkCount}/{totalChunkCount}: {file.Name} ");
                    buffer = new byte[ED2K_CHUNK_SIZE];
                }
                Program.ClearLine();
                hash = chunkCount > 1
                    ? _md4.GetHexHashFromBytes(md4HashedBytes.ToArray())
                    : MD4.BytesToHex(md4HashedBytes.ToArray(), md4HashedBytes.Count);
                result = new CalculationResult(file, hash);
            }
            return result;
        }
    }
}