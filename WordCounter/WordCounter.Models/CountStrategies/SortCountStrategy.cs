using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordCounter.Models.Interfaces;

namespace WordCounter.Models.CountStrategies
{
    public sealed class SortCountStrategy : ICountStrategy
    {
        /// <summary>
        /// Counts the words.
        /// </summary>
        /// <param name="textSource">The text source to count.</param>
        /// <param name="limitResult">Result will be limited to N items.</param>
        /// <returns>
        /// Returns result of count.
        /// </returns>
        public IEnumerable<WordResult> CountWords(ITextSource textSource, int limitResult)
        {
            var result = new LinkedList<WordResult>();

            var list = textSource.OrderBy(item => item, StringComparer.OrdinalIgnoreCase);
            if (limitResult == 0)
            {
                limitResult = list.Count();
            }

            var currentWord = list.First();
            var currentWordCount = 0;
            result.AddFirst(new WordResult());

            foreach (var word in list)
            {
                if (string.Equals(currentWord, word, StringComparison.OrdinalIgnoreCase))
                {
                    currentWordCount++;
                    continue;
                }

                var currentNode = result.Last;

                for (var j = 0; j <= limitResult; j++)
                {
                    if (currentWordCount <= currentNode.Value.Count && result.Count == limitResult)
                    {
                        break;
                    }

                    if (currentNode.Previous == null || currentWordCount <= currentNode.Previous.Value.Count)
                    {
                        var wordResult = new WordResult { Word = currentWord, Count = currentWordCount };
                        result.AddBefore(currentNode, wordResult);
                        if (result.Count > limitResult)
                        {
                            result.RemoveLast();
                        }
                        break;
                    }
                    currentNode = currentNode.Previous;
                }

                currentWord = word;
                currentWordCount = 1;
            }


            return result;
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
                return "Sorting strategy";
            }
        }

        /// <summary>
        /// Gets description of strategy.
        /// </summary>
        /// <value>
        /// Description.
        /// </value>
        public string Description
        {
            get
            {
                return "Sorts source first, then counts word, and if it repeated more than already added, will add new, and remove word with minimum repeat occasions.";
            }
        }
    }
}
