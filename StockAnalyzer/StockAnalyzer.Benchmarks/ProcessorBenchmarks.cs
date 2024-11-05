using BenchmarkDotNet.Attributes;

namespace StockAnalyzer.Benchmarks
{
    [SimpleJob]
    [MemoryDiagnoser]
    public class ProcessorBenchmarks
    {
        Processor.Processor _processor;

        [GlobalSetup]
        public void Setup()
        {
            _processor = new Processor.Processor();
            _processor.Initialize();
        }

        [Benchmark]
        public List<string> Processor()
        {
            var result = new List<string>();

            foreach (var stock in _processor.Stocks)
            {
                var min = _processor.Min(stock.Key);
                var max = _processor.Max(stock.Key);
                var average = _processor.Average(stock.Key);

                result.Add($"{min} {max} {average}");
            }

            return result;
        }


        [Benchmark]
        public List<string> ProcessorFaster()
        {
            var result = new List<string>();

            foreach (var stock in _processor.Stocks)
            {
                (decimal min, decimal max, decimal average) = _processor.GetReport(stock.Key);
                result.Add($"{min} {max} {average}");
            }

            return result;
        }
    }
}
