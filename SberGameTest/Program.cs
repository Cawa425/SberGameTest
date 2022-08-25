namespace SberGameTest;

internal static class Program
{
    private static void Main(string[] args)
    {
        var words = FirstProcessParsing.Select5LengthWordsFromFile();
        ParallelQuery<string> remainedWords;
        while (true)
        {
            Console.WriteLine("Введите слово");
            var inputWord = Console.ReadLine();
            if (string.IsNullOrEmpty(inputWord))
            {
                Console.WriteLine("Неправильный формат ввода");
                continue;
            }

            Console.WriteLine("Какие буквы зеленые? (номер через пробел)");
            var rightPlacesCharsIdsInput = Console.ReadLine();
            var rightPlacesCharsIds = string.IsNullOrEmpty(rightPlacesCharsIdsInput)
                ? new List<int>()
                : rightPlacesCharsIdsInput.Split(' ').Select(int.Parse).ToList();
            Console.WriteLine("Какие буквы синие? (номер букв в слове через пробел )");
            var wrongPlacesCharsIdsInput = Console.ReadLine();
            var wrongPlacesCharsIds = string.IsNullOrEmpty(wrongPlacesCharsIdsInput)
                ? new List<int>()
                : wrongPlacesCharsIdsInput.Split(' ').Select(int.Parse).ToList();
            var totalyWrongChars = inputWord.ToList().Where(x =>
                !rightPlacesCharsIds.Contains(inputWord.IndexOf(x) + 1) &&
                !wrongPlacesCharsIds.Contains(inputWord.IndexOf(x) + 1)).ToList();

            Console.WriteLine(
                "Какие буквы повторяются? (номер буквы и количество в слове через пробел, например 1 2. Несколько букв размечать запятой)");
            var samePlacesCharsIdsInputIDS = Console.ReadLine();
            var samePlacesCharsIdsInput = string.IsNullOrEmpty(samePlacesCharsIdsInputIDS)
                ? new List<string>()
                : samePlacesCharsIdsInputIDS.Split(',').ToList();
            var dict = new Dictionary<char, int>();
            //добавляем те что записали что несколько раз
            //все остальные 1 раз по дефолту
            foreach (var str in samePlacesCharsIdsInput)
            {
                var s = str.Split(' ');
                dict.Add(inputWord[int.Parse(s[0])-1], int.Parse(s[1]));
            }

            remainedWords = from word in words.AsParallel()
                where rightPlacesCharsIds.All(id => inputWord[id - 1] == word[id - 1]) &&
                      wrongPlacesCharsIds.All(id => inputWord[id - 1] != word[id - 1] &&
                                                    word.Contains(inputWord[id - 1])) &&
                      totalyWrongChars.All(charing => !word.Contains(charing)) &&
                      dict.All(x => word.Count(z => z == x.Key) == x.Value)
                select word;
            Console.WriteLine("Найдено слов: " + remainedWords.Count());
            foreach (var remainedWord in remainedWords)
                Console.WriteLine(remainedWord);
            words = remainedWords.ToList();
        }
    }
}