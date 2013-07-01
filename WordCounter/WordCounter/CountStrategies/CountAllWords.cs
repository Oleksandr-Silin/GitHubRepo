using System;
using System.Collections.Generic;
using System.Linq;

namespace WordCounter.SearchStrategies
{

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CountAllWords : ICountStrategy
    {
        public IEnumerable<WordResult> CountWords(ITextSource textSource)
        {
            var results = new Dictionary<string, int>();
            if (textSource.IsReadyToUse)
            {
                foreach (var word in textSource)
                {
                    if (results.ContainsKey(word))
                    {
                        results[word]++;
                    }
                    else
                    {
                        results.Add(word, 1);
                    }
                }
            }

            return results.Select(pair => new WordResult() { Count = pair.Value, Word = pair.Key });
        }
    }
}
