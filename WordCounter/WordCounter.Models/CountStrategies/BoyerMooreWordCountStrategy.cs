using System.Collections.Generic;
using WordCounter.Models.Interfaces;

namespace WordCounter.Models.CountStrategies
{
    /// <summary>
    /// Search word which repeated in N/2 times using Boyer-Moore algorithm.
    /// </summary>
    public sealed class BoyerMooreWordCountStrategy : ICountStrategy
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

            return new[] { new WordResult() { Word = confidence > 0 ? candidate : "There is no candidate." } };
        }

        /// <summary>
        /// Gets name of strategy.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get
            {
                return "Boyer-Moore";
            }
        }

        public string Description
        {
            get
            {
                return "Uses Boyer-Moore substring search algorithm modification to search word which repeated in N/2 times.";
            }
        }
    }
}
