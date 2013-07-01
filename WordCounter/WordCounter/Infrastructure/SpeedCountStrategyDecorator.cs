using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace WordCounter.Infrastructure
{
    internal sealed class SpeedCountStrategyDecorator : CountStrategyDecoratorBase
    {
        public SpeedCountStrategyDecorator(ICountStrategy decoratedCountStrategy)
            : base(decoratedCountStrategy)
        { }

        public override IEnumerable<WordResult> CountWords(ITextSource textSource)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var result = base.CountWords(textSource).ToList();
            stopwatch.Stop();
            var logMessage = string.Format("Total time {0:c}.", stopwatch.Elapsed);
            result.Add(CreateLogWordResult(logMessage));
            return result;
        }
    }
}
