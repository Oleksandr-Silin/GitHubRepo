using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WordCounter.Models.Interfaces;

namespace WordCounter.Models.Readers
{
    /// <summary>
    /// Factory which used by WordCounter to get readers for file as source.
    /// </summary>
    public sealed class TextSourceFactory
    {
        private static readonly Lazy<TextSourceFactory> LazyInstance =
                        new Lazy<TextSourceFactory>(() => new TextSourceFactory());
        private readonly Dictionary<string, Func<ITextSource>> _textReaders;
        private readonly IEnumerable<string> _supportedTextFormats;

        private TextSourceFactory()
        {
            _textReaders = new Dictionary<string, Func<ITextSource>>
                {
                    {TxtReader.SupportedFileFormat, () => new TxtReader()}
                };
            _supportedTextFormats = _textReaders.Keys.ToList();
        }

        /// <summary>
        /// Registers the reader for the file extension, will change already registered reader for type to new.
        /// </summary>
        /// <param name="fileExtension">The file extension started from dot.</param>
        /// <param name="sourceActivator">The source activator, must return reader which can be used as source.</param>
        public void RegisterReader(string fileExtension, Func<ITextSource> sourceActivator)
        {
            if (fileExtension.StartsWith("."))
            {
                throw new ArgumentException("The file extension must be started from dot.");
            }

            if (_textReaders.ContainsKey(fileExtension))
            {
                _textReaders[fileExtension] = sourceActivator;
                return;
            }
            _textReaders.Add(fileExtension, sourceActivator);
        }

        /// <summary>
        /// Gets the instance of <see cref="TextSourceFactory"/>.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static TextSourceFactory Instance { get { return LazyInstance.Value; } }

        /// <summary>
        /// Gets the supported text formats.
        /// </summary>
        /// <value>
        /// The supported text formats.
        /// </value>
        public IEnumerable<string> SupportedTextFormats
        {
            get
            {
                return _supportedTextFormats;
            }
        }

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

        public bool IsFileSupported(string path)
        {
            return _textReaders.Any(pair => IsCanRead(pair.Key, path));
        }

        /// <summary>
        /// Determines whether is can read file by the specified path.
        /// </summary>
        /// <param name="extension">Target extension</param>
        /// <param name="path">The path to file.</param>
        /// <returns>
        /// The true if reader can read file by path.
        /// </returns>
        private static bool IsCanRead(string extension, string path)
        {
            return File.Exists(path) && (Path.GetExtension(path) ?? string.Empty).ToUpperInvariant() == extension.ToUpperInvariant();
        }
    }
}
