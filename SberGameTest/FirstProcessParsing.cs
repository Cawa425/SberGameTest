using System.Text;

namespace SberGameTest;

public static class FirstProcessParsing
{
    private static readonly string? DirectoryPath =
        Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName;

    private static readonly string ProcessedFilePath = DirectoryPath + @"\ProcessedFile.txt";
    private static readonly string WordsFilePath = DirectoryPath + @"\russian2.txt";

    public static List<string> Select5LengthWordsFromFile()
    {
        if (File.Exists(ProcessedFilePath)) return File.ReadLines(ProcessedFilePath, Encoding.UTF8).ToList();
        using (var fileStream = File.Open(ProcessedFilePath, FileMode.Create))
        {
            var text = File.ReadAllLines(WordsFilePath);
            var words = new List<string>();
            Parallel.ForEach(text, word =>
            {
                if (word.Length == 5) words.Add(word);
            });
            using var file = new StreamWriter(fileStream);
            words.ForEach(x => file.WriteLine(x));
            return words;
        }
    }
}