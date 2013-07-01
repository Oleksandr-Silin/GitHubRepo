using System;
using System.Collections.Generic;
using System.Linq;
using WordCounter.Models.Interfaces;
using WordCounter.Models.Readers;

namespace WordCounter.Models
{
    /// <summary>
    /// Counts word repeat occasions.
    /// </summary>
    public sealed class WordCounter
    {
        private ICountStrategy _searcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="WordCounter"/> class.
        /// </summary>
        /// <param name="searcher">The count strategy.</param>
        public WordCounter(ICountStrategy searcher)
        {
            SetSearchStrategy(searcher);
        }

        /// <summary>
        /// Sets the search strategy.
        /// </summary>
        /// <param name="strategy">The count strategy.</param>
        /// <exception cref="System.ArgumentNullException">strategy;Strategy cannot be null.</exception>
        public void SetSearchStrategy(ICountStrategy strategy)
        {
            if (strategy == null)
            {
                throw new ArgumentNullException("strategy", "Strategy cannot be null.");
            }

            _searcher = strategy;
        }

        /// <summary>
        /// Counts words in the file.
        /// </summary>
        /// <param name="path">Path to the file.</param>
        /// <param name="limitResult">Result will be limited to N items.</param>
        /// <returns></returns>
        public IEnumerable<WordResult> CountWords(string path, int limitResult)
        {
            var textSource = TextSourceFactory.Instance.CreateTextSource(path);
            return _searcher.CountWords(textSource, limitResult);
        }
    }
}
