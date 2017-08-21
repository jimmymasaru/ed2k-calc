using System;
using System.IO;

namespace Ed2kCalculator
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var flag = true;
            Console.WriteLine("ed2k Hash Calculator");
            string path = null;
            string outputFile = null;
            if (args.Length >= 1)
            {
                path = args[0];
                if (!path.EndsWith(new string(Path.DirectorySeparatorChar, 1)))
                    path = path + Path.DirectorySeparatorChar;
                Console.WriteLine($"Directory: {path}");
                var outputDir = path;
                if (args.Length >= 2)
                {
                    outputDir = args[1];
                }
                outputFile = Path.Combine(outputDir,
                    $"Ed2k Hash Result {DateTime.Now:yyyyMMdd HHmmss}.txt");
            }
            else
            {
                Console.WriteLine("Usage: dotnet Ed2kCalculator.dll folder [outputfolder]");
                Console.WriteLine("       dotnet Ed2kCalculator.dll ~/Downloads/");
                Console.WriteLine("       dotnet Ed2kCalculator.dll ~/Downloads/ ~/Documents/ed2k/");
                flag = false;
            }
            if (flag)
            {
                try
                {
                    Execute(path, outputFile);
                }
                catch (Exception e)
                {
                    Console.WriteLine("An error has occurred while processing.");
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.StackTrace);
                }
            }
        }

        public static void Execute(string destination, string output)
        {
            Console.Write("Retrieving Files ...");
            ClearLine();
            var helper = new FileHelper(destination);
            var allFiles = helper.AllFiles;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{allFiles.Count} file(s) in total.");
            Console.ResetColor();

            Console.WriteLine($"Results will be written to ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(output);
            Console.ResetColor();

            Console.WriteLine();
            var header = $"{destination}{Environment.NewLine}{new string('-', 80)}{Environment.NewLine}";
            File.WriteAllText(output, header);

            var calculator = new ed2kCalculator();
            foreach (var result in calculator.Calc(allFiles))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                var line = TabDelimittedResultFormatter.Format(result, destination);
                Console.WriteLine(line);
                Console.ResetColor();
                File.AppendAllLines(output, new[] { line });
            }
        }

        public static void ClearLine()
        {
            // http://stackoverflow.com/a/6774395
            // 033 = x1b
            Console.Write("\r\x1b[K");
        }
    }
}