using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using StockAnalyzer.Processor;
using System.Collections.Immutable;

namespace StockAnalyzer.Benchmarks
{
    [SimpleJob(RunStrategy.ColdStart, iterationCount: 5)]
    [MemoryDiagnoser]
    public class BigOBenchmarks
    {
        private List<Stock> _stocks;
        private ImmutableSortedSet<Stock> _sortedStocks;
        private Dictionary<string, Stock> _dictionaryStocks;

        [GlobalSetup]
        public void Setup()
        {
            _stocks = Enumerable.Range(0, 100000)
                .Select(x => new Stock(x.ToString()))
                .OrderBy(x => Guid.NewGuid())
                .ToList();

            //_sortedStocks = _stocks
            //    .OrderBy(x => x)
            //    .ToImmutableSortedSet();
        }

        [Benchmark]
        public Stock? NotSorted()
        {
            Stock? element = null;

            for (int i = 0; i < 1000; i++)
            {
                element = _stocks.FirstOrDefault(x => x.Ticker == i.ToString());
            }

            return element;
        }

        [Benchmark]
        public Stock? Sorted()
        {
            Stock? element = null;
            _sortedStocks = _stocks
                .OrderBy(x => x)
                .ToImmutableSortedSet();

            for (int i = 0; i < 1000; i++)
            {
                _sortedStocks.TryGetValue(new(i.ToString()), out element);
            }

            return element;
        }

        [Benchmark]
        public Stock? Dictionary()
        {
            Stock? element = null;
            _dictionaryStocks = _stocks.ToDictionary(x => x.Ticker);

            for (int i = 0; i < 1000; i++)
            {
                element = _dictionaryStocks[i.ToString()];
            }

            return element;
        }
    }
}
