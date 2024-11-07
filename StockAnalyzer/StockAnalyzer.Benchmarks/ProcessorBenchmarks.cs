using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

namespace StockAnalyzer.Benchmarks
{
    [SimpleJob(RunStrategy.ColdStart, iterationCount: 5)]
    [MemoryDiagnoser]
    public class ProcessorBenchmarks
    {
        [Benchmark]
        public List<string> Processor()
        {
            var result = new List<string>();

            var processor = new Processor.Processor();
            processor.Initialize();

            foreach (var stock in processor.Stocks)
            {
                var min = processor.Min(stock.Key);
                var max = processor.Max(stock.Key);
                var average = processor.Average(stock.Key);

                result.Add($"{min} {max} {average}");
            }

            return result;
        }

        [Benchmark]
        public List<string> ProcessorImproved()
        {
            var result = new List<string>();

            var processor = new Processor.Processor();
            processor.Initialize();

            foreach (var stock in processor.Stocks)
            {
                (decimal min, decimal max, decimal average) = processor.GetReport(stock.Key);
                result.Add($"{min} {max} {average}");
            }

            return result;
        }

        [Benchmark]
        public List<string> ProcessorFaster()
        {
            var result = new List<string>();
            var processor = new Processor.ProcessorFaster();
            processor.Initialize();

            foreach (var stock in processor.Stocks)
            {
                result.Add($"{stock.Value.Min} {stock.Value.Max} {stock.Value.Average}");
            }

            return result;
        }

        [Benchmark]
        public async Task<List<string>> ProcessorFasterAsync()
        {
            var result = new List<string>();
            var processor = new Processor.ProcessorFaster();
            await processor.InitializeAsync();

            foreach (var stock in processor.Stocks)
            {
                result.Add($"{stock.Value.Min} {stock.Value.Max} {stock.Value.Average}");
            }

            return result;
        }

        [Benchmark]
        public async Task<List<string>> ProcessorFasterAsync_V2()
        {
            var result = new List<string>();
            var processor = new Processor.ProcessorFaster();
            await processor.InitializeAsync_V2();

            foreach (var stock in processor.Stocks)
            {
                result.Add($"{stock.Value.Min} {stock.Value.Max} {stock.Value.Average}");
            }

            return result;
        }
    }
}
