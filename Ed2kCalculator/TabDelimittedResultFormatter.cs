namespace Ed2kCalculator
{
    public class TabDelimittedResultFormatter
    {
        public static string Format(CalculationResult result, string dir)
        {
            var relativeFilename = result.File.FullName;
            if (relativeFilename.StartsWith(dir))
                relativeFilename = relativeFilename.Substring(dir.Length);
            return
                $"{relativeFilename}\t{result.File.Length}\t{result.File.LastWriteTime:yyyy-MM-dd HH:mm:ss}\t{result.Ed2kLink}";
        }
    }
}