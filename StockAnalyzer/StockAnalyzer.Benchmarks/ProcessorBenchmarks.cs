using BenchmarkDotNet.Attributes;

namespace StockAnalyzer.Benchmarks
{
    [SimpleJob]
    [MemoryDiagnoser]
    public class ProcessorBenchmarks
    {
        [GlobalSetup]
        public void Setup() { }

        [Benchmark]
        public List<string> Processor()
        {
            var result = new List<string>();

            var processor = new Processor.Processor("D:\\source\\repos\\Perf_Exercices\\StockAnalyzer\\StockAnalyzer.Processor\\Data");

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
    }
}
