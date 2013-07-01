namespace WordCounter.Models
{
    /// <summary>
    /// Keeps info about word and how many times is was repeated.
    /// </summary>
    public struct WordResult
    {
        /// <summary>
        /// Gets or sets the word.
        /// </summary>
        /// <value>
        /// The word.
        /// </value>
        public string Word { get; set; }

        /// <summary>
        /// Gets or sets the count of repeat times of the word.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public int Count { get; set; }
    }
}