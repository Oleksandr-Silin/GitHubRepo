using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using WordCounter.Models;
using WordCounter.Models.Interfaces;

namespace WordCounter.UI.Infrastructure
{
    public sealed class MemoryUsageCountStrategyDecorator : CountStrategyDecoratorBase
    {
        public MemoryUsageCountStrategyDecorator(ICountStrategy decoratedCountStrategy)
            : base(decoratedCountStrategy)
        { }

        public override IEnumerable<WordResult> CountWords(ITextSource textSource, int limitResult)
        {
            const int toSize = 1024 ;
            const string size = "KB";
            var process = Process.GetCurrentProcess();
            var result = base.CountWords(textSource,limitResult);

            CreateLogWordResult( string.Format("Private memory usage of the process: {0} {1}",
                 process.PrivateMemorySize64 / toSize, size));

            CreateLogWordResult (string.Format("Peak physical memory usage of the process: {0} {1}",
                 process.PeakWorkingSet64 / toSize, size));

            CreateLogWordResult(string.Format("Peak paged memory usage of the process: {0} {1}",
                     process.PeakPagedMemorySize64 / toSize, size));

            CreateLogWordResult(string.Format("Peak virtual memory usage of the process: {0} {1}",
                                              process.PeakVirtualMemorySize64/toSize, size));

            return result;
        }

        public override string Name
        {
            get
            {
                return "Memory usage.";
            }
        }
    }
}
