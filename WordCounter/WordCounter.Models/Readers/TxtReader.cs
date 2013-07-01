using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;
using System.Text;
using WordCounter.Models.Interfaces;

namespace WordCounter.Models.Readers
{
    /// <summary>
    /// Provides read of txt file thru enumeration.
    /// </summary>
    public sealed class TxtReader : ITextSource
    {
        private string _path;

        /// <summary>
        /// Gets a value indicating whether this instance is ready to use.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is ready to use; otherwise, <c>false</c>.
        /// </value>
        public bool IsReadyToUse
        {
            get
            {
                return !string.IsNullOrEmpty(_path);
            }
        }

        /// <summary>
        /// Gets the file format which supported by reader.
        /// </summary>
        /// <value>
        /// The file format.
        /// </value>
        string ITextSource.SupportedFileFormat
        {
            get { return SupportedFileFormat; }
        }

        /// <summary>
        /// Gets the file format which supported by reader.
        /// </summary>
        /// <value>
        /// The file format.
        /// </value>
        public static string SupportedFileFormat
        {
            get
            {
                return ".txt";
            }
        }

        /// <summary>
        /// Sets the path for reader to read from.
        /// </summary>
        /// <param name="path">The path to file.</param>
        /// <exception cref="System.InvalidOperationException">This file cannot be read by txt reader.</exception>
        public void SetPathForRead(string path)
        {
            if (!File.Exists(path))
            {
                throw new InvalidOperationException("File does not exist.");
            }
            _path = path;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<string> GetEnumerator()
        {
            if (!File.Exists(_path))
            {
                throw new InvalidOperationException("File does not exist.");
            }

            var fs = new FileStream(_path, FileMode.Open, FileAccess.Read, FileShare.Read);
            return new TxtReaderEnumerator(new StreamReader(fs, Encoding.UTF8));
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private class TxtReaderEnumerator : IEnumerator<string>
        {
            private readonly StreamReader _streamReader;

            public TxtReaderEnumerator(StreamReader streamReader)
            {
                _streamReader = streamReader;
            }

            /// <summary>
            /// Gets the element in the collection at the current position of the enumerator.
            /// </summary>
            /// <returns>
            /// The element in the collection at the current position of the enumerator.
            ///   </returns>
            object IEnumerator.Current
            {
                get
                {
                    return Current;
                }
            }

            /// <summary>
            /// Gets the element in the collection at the current position of the enumerator.
            /// </summary>
            /// <returns>
            /// The element in the collection at the current position of the enumerator.
            /// </returns>
            public string Current { get; private set; }

            public bool MoveNext()
            {
                var isMovedToNext = false;
                var stringBuilder = new StringBuilder();

                while (_streamReader.Peek() >= 0)
                {
                    var c = (char)_streamReader.Read();

                    if (!char.IsLetter(c))
                    {
                        if (stringBuilder.Length == 0)
                        {
                            continue;
                        }

                        break;
                    }
                    isMovedToNext = true;
                    stringBuilder.Append(c);
                }

                Current = stringBuilder.ToString();

                return isMovedToNext;
            }

            /// <summary>
            /// Sets the enumerator to its initial position, which is before the first element in the collection.
            /// </summary>
            /// <exception cref="System.InvalidOperationException">Reader is disposed</exception>
            public void Reset()
            {
                _streamReader.BaseStream.Position = 0;
                _streamReader.DiscardBufferedData();
            }

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                _streamReader.Dispose();
            }
        }
    }
}
