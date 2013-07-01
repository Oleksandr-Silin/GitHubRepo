using System;
using System.Collections.Generic;
using WordCounter.Readers;
using System.Linq;

namespace WordCounter
{
    /// <summary>
    /// Counts word repeat occasions.
    /// </summary>
    public class WordCounter
    {
        private ICountStrategy _searcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="WordCounter"/> class.
        /// </summary>
        /// <param name="searcher">The count strategy.</param>
        public WordCounter(ICountStrategy searcher)
        {
            SetSearchStrategy(searcher);
            Results = new List<WordResult>();
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

        public IEnumerable<WordResult> Results { get; private set; }

        public void CountWords(string path)
        {
            var textSource = TextSourceFactory.Instance.CreateTextSource(path);
            Results = _searcher.CountWords(textSource).OrderByDescending(item => item.Count);
        }
    }
}
