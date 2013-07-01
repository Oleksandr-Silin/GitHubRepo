using System.Collections.Generic;

namespace WordCounter.Models.Interfaces
{
    /// <summary>
    /// Provides enumerable access to file.
    /// </summary>
    public interface ITextSource : IEnumerable<string>
    {
        /// <summary>
        /// Gets a value indicating whether this instance is ready to use.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is ready to use; otherwise, <c>false</c>.
        /// </value>
        bool IsReadyToUse { get; }

        /// <summary>
        /// Gets the file format which supported by reader.
        /// </summary>
        /// <value>
        /// The file format.
        /// </value>
        string SupportedFileFormat { get; }

        /// <summary>
        /// Sets the path for reader to read from.
        /// </summary>
        /// <param name="path">The path to file.</param>
        /// <exception cref="System.InvalidOperationException">This file cannot be read by txt reader.</exception>
        void SetPathForRead(string path);
    }
}