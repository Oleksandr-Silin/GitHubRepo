using System;
using System.Collections.Generic;

namespace WordCounter.Infrastructure
{
    abstract class CountStrategyDecoratorBase : ICountStrategy
    {
        private readonly ICountStrategy _decoratedCountStrategy;

        protected CountStrategyDecoratorBase(ICountStrategy decoratedCountStrategy)
        {
            if (decoratedCountStrategy == null)
            {
                throw new ArgumentNullException("decoratedCountStrategy");
            }

            _decoratedCountStrategy = decoratedCountStrategy;
        }

        public virtual IEnumerable<WordResult> CountWords(ITextSource textSource)
        {
            return _decoratedCountStrategy.CountWords(textSource);
        }

        protected WordResult CreateLogWordResult(string logInfo)
        {
            var logMessage = string.Format("$$$ {0} $$$", logInfo);
            return new WordResult() { Word = logMessage, Count = SpecialCountNum };
        }

        public static int SpecialCountNum { get { return -999; } }
    }
}
