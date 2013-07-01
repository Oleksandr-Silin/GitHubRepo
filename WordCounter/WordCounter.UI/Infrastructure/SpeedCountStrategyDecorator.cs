using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using WordCounter.Models;
using WordCounter.Models.Interfaces;

namespace WordCounter.UI.Infrastructure
{
    internal sealed class SpeedCountStrategyDecorator : CountStrategyDecoratorBase
    {
        public SpeedCountStrategyDecorator(ICountStrategy decoratedCountStrategy)
            : base(decoratedCountStrategy)
        { }

        public override IEnumerable<WordResult> CountWords(ITextSource textSource, int limitResult)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var result = base.CountWords(textSource,limitResult);
            stopwatch.Stop();
            var logMessage = string.Format("Total time {0:c}.", stopwatch.Elapsed);
            CreateLogWordResult(logMessage);
            return result;
        }

        public override string Name
        {
            get
            {
                return "Speed of strategy.";
            }
        }
    }
}
