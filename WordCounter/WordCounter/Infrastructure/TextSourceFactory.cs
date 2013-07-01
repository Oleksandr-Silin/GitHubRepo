using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WordCounter.Readers
{
    public sealed class TextSourceFactory
    {
        private static readonly Lazy<TextSourceFactory> LazyInstance =
                        new Lazy<TextSourceFactory>(() => new TextSourceFactory());
        private readonly Dictionary<string, Func<ITextSource>> _textReaders;

        private TextSourceFactory()
        {
            _textReaders = new Dictionary<string, Func<ITextSource>>
                {
                    {TxtReader.SupportedFileFormat, () => new TxtReader()}
                };
        }

        /// <summary>
        /// Gets the instance of <see cref="TextSourceFactory"/>.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static TextSourceFactory Instance { get { return LazyInstance.Value; } }

        /// <summary>
        /// Creates the ready text source
        /// </summary>
        /// <param name="path">Path to the file.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">Unsupported file format.</exception>
        public ITextSource CreateTextSource(string path)
        {
            foreach (var pair in _textReaders.Where(pair => IsCanRead(pair.Key, path)))
            {
                var textSource = pair.Value();
                textSource.SetPathForRead(path);
                return textSource;
            }

            throw new InvalidOperationException("Unsupported file format.");
        }

        /// <summary>
        /// Determines whether is can read file by the specified path.
        /// </summary>
        /// <param name="extension">Target extension</param>
        /// <param name="path">The path to file.</param>
        /// <returns>
        /// The true if reader can read file by path.
        /// </returns>
        public bool IsCanRead(string extension, string path)
        {
            return (Path.GetExtension(path) ?? string.Empty).ToUpperInvariant() == extension.ToUpperInvariant();
        }
    }
}
