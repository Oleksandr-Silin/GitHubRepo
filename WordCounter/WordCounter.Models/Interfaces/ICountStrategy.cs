using System.Collections.Generic;

namespace WordCounter.Models.Interfaces
{
    /// <summary>
    /// Strategy which counts word repeat in source.
    /// </summary>
    public interface ICountStrategy
    {
        /// <summary>
        /// Counts the words.
        /// </summary>
        /// <param name="textSource">The text source to count.</param>
        /// <param name="limitResult">Result will be limited to N items.</param>
        /// <returns>
        /// Returns result of count.
        /// </returns>
        IEnumerable<WordResult> CountWords(ITextSource textSource, int limitResult);

        /// <summary>
        /// Gets name of strategy.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        string Name { get; }

        /// <summary>
        /// Gets description of strategy.
        /// </summary>
        /// <value>
        /// Description.
        /// </value>
        string Description { get; }
    }
}