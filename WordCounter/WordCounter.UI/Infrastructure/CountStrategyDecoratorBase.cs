using System;
using System.Collections.Generic;
using WordCounter.Models;
using WordCounter.Models.Interfaces;

namespace WordCounter.UI.Infrastructure
{
    /// <summary>
    /// Allows to decorate count strategies.
    /// </summary>
    public abstract class CountStrategyDecoratorBase : ICountStrategy
    {
        private readonly ICountStrategy _decoratedCountStrategy;
        private readonly List<string> _logResults;

        protected CountStrategyDecoratorBase(ICountStrategy decoratedCountStrategy)
        {
            if (decoratedCountStrategy == null)
            {
                throw new ArgumentNullException("decoratedCountStrategy");
            }

            _decoratedCountStrategy = decoratedCountStrategy;
            _logResults = new List<string>();
        }

        /// <summary>
        /// Gets the decorated count strategy.
        /// </summary>
        /// <value>
        /// The decorated count strategy.
        /// </value>
        public ICountStrategy DecoratedCountStrategy
        {
            get
            {
                return _decoratedCountStrategy;
            }
        }

        /// <summary>
        /// Counts the words.
        /// </summary>
        /// <param name="textSource">The text source to count.</param>
        /// <param name="limitResult">Result will be limited to N items.</param>
        /// <returns>
        /// Returns result of count.
        /// </returns>
        public virtual IEnumerable<WordResult> CountWords(ITextSource textSource, int limitResult)
        {
            return _decoratedCountStrategy.CountWords(textSource, limitResult);
        }

        /// <summary>
        /// Gets name of strategy.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public abstract string Name { get; }

        /// <summary>
        /// Gets description of strategy.
        /// </summary>
        /// <value>
        /// Description.
        /// </value>
        public string Description
        {
            get
            {
                return string.Format("Decorates with new strategy {0}.", Name);
            }
        }

        /// <summary>
        /// Creates the log word result.
        /// </summary>
        /// <param name="logInfo">The log info.</param>
        /// <returns>
        /// Returns instance with specific count num.
        /// </returns>
        protected void CreateLogWordResult(string logInfo)
        {
            _logResults.Add(string.Format("$$$ {0} $$$", logInfo));

        }

        /// <summary>
        /// Gets the log results.
        /// </summary>
        /// <value>
        /// The log results.
        /// </value>
        public IEnumerable<string> LogResults
        {
            get
            {
                return _logResults;
            }
        }
    }
}
