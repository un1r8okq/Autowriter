using Autowriter.Data;
using MediatR;

namespace Autowriter.Pages.Generate
{
    public class GenerateHandler
    {
        public class Model
        {
            public bool WordCountTooLarge { get; set; }

            public int SourceCount { get; set; }

            public GeneratedWriting? Writing { get; set; }

            public class GeneratedWriting
            {
                public int Id { get; set; }

                public DateTime Created { get; set; }

                public string Content { get; set; } = String.Empty;
            }
        }

        public class Command : IRequest<Model>
        {
            public int WordCount { get; set; }
        }

        public class Handler : RequestHandler<Command, Model>
        {
            private readonly Random _random;
            private readonly ISourceMaterialRepository _repository;
            private Dictionary<string, Dictionary<string, int>> _lexicon;

            public Handler(ISourceMaterialRepository repository)
            {
                _random = new Random();
                _repository = repository;
            }

            protected override Model Handle(Command command)
            {
                var sources = _repository.GetSources();

                if (command.WordCount > 1000)
                {
                    return new Model
                    {
                        SourceCount = sources.Count(),
                        WordCountTooLarge = true
                    };
                }

                BuildLexicon(sources);
                var generatedWriting = GenerateWriting(command.WordCount);

                return new Model
                {
                    SourceCount = sources.Count(),
                    WordCountTooLarge = false,
                    Writing = new Model.GeneratedWriting
                    {
                        Id = 1,
                        Created = DateTime.UtcNow,
                        Content = generatedWriting,
                    },
                };
            }

            private void BuildLexicon(IEnumerable<SourceMaterialRepository.Source> sources)
            {
                _lexicon = new Dictionary<string, Dictionary<string, int>>();

                foreach (var source in sources)
                {
                    var words = source.Content
                        .Split(' ')
                        .SelectMany(word => word.Split("\r\n"))
                        .SelectMany(word => word.Split("\r"))
                        .SelectMany(word => word.Split("\n"))
                        .Select(word => RemoveIgnoredCharacters(word))
                        .Where(word => word != null && word != string.Empty)
                        .Select(word => word.ToLower())
                        .ToList();

                    for (var i = 0; i < words.Count; i++)
                    {
                        var currentWord = words[i];

                        if (!_lexicon.ContainsKey(currentWord))
                        {
                            _lexicon.Add(currentWord, new Dictionary<string, int>());
                        }

                        if (i == words.Count - 1)
                        {
                            break;
                        }

                        var nextWord = words[i + 1];

                        if (!_lexicon[currentWord].ContainsKey(nextWord))
                        {
                            _lexicon[currentWord].Add(nextWord, 0);
                        }

                        _lexicon[currentWord][nextWord]++;
                    }
                }
            }

            private string RemoveIgnoredCharacters(string word)
            {
                var charsToRemove = new string[] { ".", "!", "?", "\"", "“", "”" };

                foreach (var character in charsToRemove)
                {
                    word = word.Replace(character, string.Empty);
                }

                return word;
            }

            private string GenerateWriting(int wordCount)
            {
                var words = new List<string>();

                while (words.Count < wordCount)
                {
                    words.AddRange(GenerateSentence());
                }

                return string.Join(" ", words);
            } 

            private List<string> GenerateSentence()
            {
                var words = new List<string>();
                var desiredWordCount = _random.Next(3, 20);

                var previousWord = RandomWord();

                while (words.Count < desiredWordCount)
                {
                    words.Add(previousWord);
                    previousWord = PickNextWord(previousWord);
                }

                words[0] = CapitaliseFirstChar(words[0]);
                words[words.Count - 1] += ".";

                return words;
            }

            private string PickNextWord(string previousWord)
            {
                if (_random.Next(4) < 1)
                {
                    return RandomWord();
                }

                var mostCommonNextWord = _lexicon[previousWord].OrderBy(x => x.Value).FirstOrDefault().Key;

                return mostCommonNextWord ?? RandomWord();
            }

            private string CapitaliseFirstChar(string word)
            {
                var capitalisedFirstChar = char.ToUpper(word[0]);
                return capitalisedFirstChar + word.Remove(0, 1);
            }

            private string RandomWord()
            {
                var index = _random.Next(_lexicon.Count);
                return _lexicon.ElementAt(index).Key;
            }
        }
    }
}
