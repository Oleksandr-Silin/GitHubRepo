using System;
using System.Collections.Generic;
using System.Linq;
using WordCounter.Models.Interfaces;

namespace WordCounter.Models.CountStrategies
{
    /// <summary>
    /// Counts all words in text using Dictionary.
    /// </summary>
    public sealed class CountAllWords : ICountStrategy
    {
        /// <summary>
        /// Counts the words.
        /// </summary>
        /// <param name="textSource">The text source.</param>
        /// <param name="limitResult">Result will be limited to N items.</param>
        /// <returns>
        /// Returns result of count.
        /// </returns>
        public IEnumerable<WordResult> CountWords(ITextSource textSource, int limitResult)
        {
            var results = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);
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
            IEnumerable<KeyValuePair<string, int>> result = results.OrderByDescending(item => item.Value);
            if (limitResult > 0)
            {
                result = result.Take(limitResult);
            }
            return result.Select(pair => new WordResult() { Count = pair.Value, Word = pair.Key });
        }

        public string Name
        {
            get
            {
                return "Dictionary use";
            }
        }

        public string Description
        {
            get
            {
                return "Uses dictionary to count all words.";
            }
        }
    }
}
