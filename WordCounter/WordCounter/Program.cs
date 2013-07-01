using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WordCounter.CountStrategies;
using WordCounter.Infrastructure;
using WordCounter.Readers;
using WordCounter.SearchStrategies;

namespace WordCounter
{
    class Program
    {
        static void Main(string[] args)
        {
            var countStrategy = new CountAllWords();
            var speedCountStrDec = new SpeedCountStrategyDecorator(countStrategy);
            var memoryCountStrDec = new MemoryUsageCountStrategyDecorator(speedCountStrDec);


            var counter = new WordCounter(memoryCountStrDec);
            counter.CountWords(args[0]);
            Console.WriteLine("File path \"{0}\"", args[0]);

            foreach (var s in counter.Results.Where(item => item.Count == CountStrategyDecoratorBase.SpecialCountNum))
            {
                Console.WriteLine(s.Word);
            }
            Console.WriteLine();

            var top = 50;
            foreach (var s in counter.Results)
            {
                Console.WriteLine("Word={0}, repeated={1}", s.Word, s.Count);
                top--;
                if (top == 0)
                {
                    break;
                }
            }


            Console.ReadKey(true);
        }
    }
}
