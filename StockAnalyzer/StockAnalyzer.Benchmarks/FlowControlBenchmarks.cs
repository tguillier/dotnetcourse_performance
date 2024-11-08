using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

namespace StockAnalyzer.Benchmarks
{
    [SimpleJob(RunStrategy.ColdStart, iterationCount: 5)]
    [MemoryDiagnoser]
    public class FlowControlBenchmarks
    {
        [Benchmark]
        public decimal ProcessWithException()
        {
            decimal result = 0;

            for (int i = 0; i < 10000; i++)
            {
                try
                {
                    var dimensions = GetDimensionsFor(i);
                    result += dimensions.Width;
                }
                catch
                {
                    // Do nothing.
                }
            }

            return result;
        }

        [Benchmark]
        public decimal ProcessWithoutException()
        {
            decimal result = 0;

            for (int i = 0; i < 10000; i++)
            {
                var dimensions = GetDimensionsFor(i);

                if (dimensions is not null)
                {
                    result += dimensions.Width;
                }
            }

            return result;
        }

        public Dimensions GetDimensionsFor(int index)
        {
            if (index % 10 == 0)
            {
                return null!;
            }

            return new(100, 100);
        }
    }

    public record Dimensions(int Width, int Height);
}
