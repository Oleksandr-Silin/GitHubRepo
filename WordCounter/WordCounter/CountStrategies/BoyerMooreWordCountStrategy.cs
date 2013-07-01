using System.Collections.Generic;

namespace WordCounter.CountStrategies
{
    class BoyerMooreWordCountStrategy : ICountStrategy
    {
        public IEnumerable<WordResult> CountWords(ITextSource textSource)
        {
            var confidence = 0;
            var candidate = string.Empty;

            foreach (var word in textSource)
            {
                if (confidence == 0)
                {
                    candidate = word;
                    confidence++;
                }
                else if (candidate == word)
                {
                    confidence++;
                }
                else
                {
                    confidence--;
                }
            }

            return new[] { new WordResult() { Count = 1, Word = confidence > 0 ? candidate : string.Empty } };
        }
    }
}
