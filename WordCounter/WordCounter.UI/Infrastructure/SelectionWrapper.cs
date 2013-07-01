namespace WordCounter.UI.Infrastructure
{
    /// <summary>
    /// Extends content with IsSelectedProperty
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class SelectionWrapper<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectionWrapper{T}"/> class.
        /// </summary>
        /// <param name="content">The content.</param>
        public SelectionWrapper(T content)
        {
            Content = content;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is selected.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is selected; otherwise, <c>false</c>.
        /// </value>
        public bool IsSelected { get; set; }

        /// <summary>
        /// Gets or sets the content which need to be wrapped for selection.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        public T Content { get; set; }
    }
}
