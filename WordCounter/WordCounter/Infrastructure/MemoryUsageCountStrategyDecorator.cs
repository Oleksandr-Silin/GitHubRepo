using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace WordCounter.Infrastructure
{
    internal sealed class MemoryUsageCountStrategyDecorator : CountStrategyDecoratorBase
    {
        public MemoryUsageCountStrategyDecorator(ICountStrategy decoratedCountStrategy)
            : base(decoratedCountStrategy)
        { }

        public override IEnumerable<WordResult> CountWords(ITextSource textSource)
        {
            const int toSize = 1024 * 1024;
            const string size = "MB";
            var process = Process.GetCurrentProcess();
            var result = base.CountWords(textSource).ToList();

            var peakWorkingSet = string.Format("Peak physical memory usage of the process: {0} {1}",
                 process.PeakWorkingSet64 / toSize, size);
            var peakPagedMem = string.Format("Peak paged memory usage of the process: {0} {1}",
                     process.PeakPagedMemorySize64 / toSize, size);
            var peakVirtualMem = string.Format("Peak virtual memory usage of the process: {0} {1}",
                   process.PeakVirtualMemorySize64 / toSize, size);

            result.Add(CreateLogWordResult(peakWorkingSet));
            result.Add(CreateLogWordResult(peakPagedMem));
            result.Add(CreateLogWordResult(peakVirtualMem));

            return result;
        }
    }
}
