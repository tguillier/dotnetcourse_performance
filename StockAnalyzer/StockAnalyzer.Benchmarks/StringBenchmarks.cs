using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using System.Text;

namespace StockAnalyzer.Benchmarks
{
    [SimpleJob(RunStrategy.ColdStart, iterationCount: 5)]
    [MemoryDiagnoser]
    public class StringBenchmarks
    {
        [Benchmark]
        public string BuildString()
        {
            string result = "";

            for (int i = 0; i < 50000; i++)
            {
                result += i;
                result += Environment.NewLine;
            }

            return result;
        }

        [Benchmark]
        public string BuildStringInterpolation()
        {
            string result = "";

            for (int i = 0; i < 50000; i++)
            {
                result = $"{result}{i}{Environment.NewLine}";
            }

            return result;
        }

        [Benchmark]
        public string BuildStringBuilder()
        {
            var builder = new StringBuilder();

            for (int i = 0; i < 50000; i++)
            {
                builder.Append(i);
                builder.Append(Environment.NewLine);
            }

            return builder.ToString();
        }
    }
}
