using MediatR;

namespace Autowriter.Features.WritingGeneration
{
    public partial class GenerateHandler
    {
        public class Command : IRequest<Response>
        {
            public int WordCount { get; set; }
        }

        public class Response
        {
            public int NumberOfSources { get; set; }

            public int RequestedNumberOfWords { get; set; }

            public bool WordCountOutOfRange { get; set; }

            public GeneratedWriting? Writing { get; set; }

            public class GeneratedWriting
            {
                public int Id { get; set; }

                public DateTime Created { get; set; }

                public string Content { get; set; } = string.Empty;
            }
        }

        public class Handler : RequestHandler<Command, Response>
        {
            private const int MinimumWords = 3;
            private const int MaximumWords = 10000;
            private readonly Random _random;
            private readonly IReadSourceMaterial _sourceReader;
            private readonly Dictionary<string, Dictionary<string, int>> _lexicon;

            public Handler(IReadSourceMaterial sourceReader)
            {
                _sourceReader = sourceReader;
                _random = new Random();
                _lexicon = new Dictionary<string, Dictionary<string, int>>();
            }

            protected override Response Handle(Command command)
            {
                var sources = _sourceReader.GetSources();

                if (command.WordCount < MinimumWords || command.WordCount > MaximumWords)
                {
                    return new Response
                    {
                        NumberOfSources = sources.Count(),
                        RequestedNumberOfWords = command.WordCount,
                        WordCountOutOfRange = true,
                    };
                }

                BuildLexicon(sources);
                var generatedWriting = GenerateWriting(command.WordCount);

                return new Response
                {
                    NumberOfSources = sources.Count(),
                    RequestedNumberOfWords = command.WordCount,
                    WordCountOutOfRange = false,
                    Writing = new Response.GeneratedWriting
                    {
                        Id = 1,
                        Created = DateTime.UtcNow,
                        Content = generatedWriting,
                    },
                };
            }

            private void BuildLexicon(IEnumerable<string> sources)
            {
                foreach (var source in sources)
                {
                    var words = source
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

            private static string RemoveIgnoredCharacters(string word)
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
                if (_lexicon.Count == 0)
                {
                    return string.Empty;
                }

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
                var desiredWordCount = _random.Next(MinimumWords, 20);

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

            private static string CapitaliseFirstChar(string word)
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
