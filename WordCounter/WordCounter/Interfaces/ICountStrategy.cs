using System.Collections.Generic;

namespace WordCounter
{
    public interface ICountStrategy
    {
        /// <summary>
        /// Counts the words.
        /// </summary>
        /// <param name="textSource">The text source.</param>
        /// <returns></returns>
        IEnumerable<WordResult> CountWords(ITextSource textSource);
    }
}