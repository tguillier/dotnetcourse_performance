using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using System.Globalization;

namespace StockAnalyzer.Benchmarks
{
    [SimpleJob(RunStrategy.ColdStart, iterationCount: 5)]
    [MemoryDiagnoser]
    public class SpanBenchmarks
    {
        private string _payload = string.Empty;

        [GlobalSetup]
        public void Setup()
        {
            _payload = File.ReadAllText(@"Data\Stocks.csv");
        }

        [Benchmark]
        public decimal Process()
        {
            decimal result = 0;

            foreach (var line in _payload.Split('\n').Skip(1))
            {
                int startIndex = 0;
                int endIndex = 0;

                // We know the CSV contains 8 columns.
                for (int column = 0; column <= 8; column++)
                {
                    endIndex = line.IndexOf(',', startIndex);

                    if (column is 6 or 7)
                    {
                        result += decimal.Parse(line[startIndex..endIndex], CultureInfo.InvariantCulture);
                    }
                    else if (column is 8)
                    {
                        result += decimal.Parse(line[startIndex..], CultureInfo.InvariantCulture);
                    }

                    startIndex = endIndex + 1;
                }
            }

            return result;
        }

        [Benchmark]
        public decimal ProcessAsSpan()
        {
            decimal result = 0;
            var payloadAsSpan = _payload.AsSpan();

            int lineStart = 0;
            int firstDataLineStart = payloadAsSpan.IndexOf('\n') + 1;

            // Slice to start at data row.
            payloadAsSpan = payloadAsSpan[firstDataLineStart..];

            while (true)
            {
                if (lineStart == -1)
                {
                    break;
                }

                var currentPayload = payloadAsSpan[lineStart..];
                var endOfLine = currentPayload.IndexOf('\n');

                ReadOnlySpan<char> currentLine;
                if (endOfLine == -1)
                {
                    currentLine = currentPayload[..];
                    lineStart = -1;
                }
                else
                {
                    lineStart += endOfLine + 1;
                    currentLine = currentPayload[..endOfLine];
                }

                int columnStartIndex = 0;
                int columnEndIndex = 0;

                // We know the CSV contains 8 columns.
                for (int column = 0; column <= 8; column++)
                {
                    var currentColumnData = currentLine[columnStartIndex..];
                    columnEndIndex = currentColumnData.IndexOf(',');

                    if (column == 8)
                    {
                        currentColumnData = currentColumnData[..];
                    }
                    else
                    {
                        currentColumnData = currentColumnData[..columnEndIndex];
                    }

                    if (column is 6 or 7 or 8)
                    {
                        result += decimal.Parse(currentColumnData, CultureInfo.InvariantCulture);
                    }

                    columnStartIndex += columnEndIndex + 1;
                }
            }

            return result;
        }
    }
}
