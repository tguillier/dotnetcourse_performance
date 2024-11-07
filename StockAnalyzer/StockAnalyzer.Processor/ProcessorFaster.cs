using System.Globalization;

namespace StockAnalyzer.Processor;

public class ProcessorFaster(string dataPath = "Data")
{
    public Dictionary<string, (int Count, decimal Min, decimal Max, decimal Average, decimal Total)> Stocks { get; set; } = [];

    public void Initialize()
    {
        foreach (var file in Directory.GetFiles(dataPath))
        {
            using var reader = new StreamReader(File.Open(file, FileMode.Open, FileAccess.Read));

            // Skip the first line.
            reader.ReadLine();
            while (reader.ReadLine() is string line)
            {
                int startIndex = 0;
                int endIndex = 0;
                string name = string.Empty;
                string value = string.Empty;

                // We know the CSV contains 8 columns.
                for (int column = 0; column < 8; column++)
                {
                    // Find the end of the current column.
                    endIndex = line.IndexOf(',', startIndex);

                    if (column == 0) // The stock name
                    {
                        name = line[startIndex..endIndex];
                    }
                    else if (column == 7)
                    {
                        value = line[startIndex..endIndex];
                    }

                    startIndex = endIndex + 1;
                }

                var change = decimal.Parse(value, CultureInfo.InvariantCulture);

                if (Stocks.ContainsKey(name))
                {
                    var trade = Stocks[name];
                    var min = change < trade.Min ? change : trade.Min;
                    var max = change > trade.Max ? change : trade.Max;
                    var total = trade.Total + change;
                    var count = trade.Count + 1;
                    var average = total / count;

                    Stocks[name] = (count, min, max, average, total);
                }
                else
                {
                    Stocks[name] = (1, change, change, change, change);
                }
            }
        }
    }

    public async Task InitializeAsync()
    {
        foreach (var file in Directory.GetFiles(dataPath))
        {
            using var reader = new StreamReader(File.Open(file, FileMode.Open, FileAccess.Read));

            // Skip the first line.
            await reader.ReadLineAsync();
            while (await reader.ReadLineAsync() is string line)
            {
                int startIndex = 0;
                int endIndex = 0;
                string name = string.Empty;
                string value = string.Empty;

                // We know the CSV contains 8 columns.
                for (int column = 0; column < 8; column++)
                {
                    // Find the end of the current column.
                    endIndex = line.IndexOf(',', startIndex);

                    if (column == 0) // The stock name
                    {
                        name = line[startIndex..endIndex];
                    }
                    else if (column == 7)
                    {
                        value = line[startIndex..endIndex];
                    }

                    startIndex = endIndex + 1;
                }

                var change = decimal.Parse(value, CultureInfo.InvariantCulture);

                if (Stocks.ContainsKey(name))
                {
                    var trade = Stocks[name];
                    var min = change < trade.Min ? change : trade.Min;
                    var max = change > trade.Max ? change : trade.Max;
                    var total = trade.Total + change;
                    var count = trade.Count + 1;
                    var average = total / count;

                    Stocks[name] = (count, min, max, average, total);
                }
                else
                {
                    Stocks[name] = (1, change, change, change, change);
                }
            }
        }
    }

    public async Task InitializeAsync_V2()
    {
        foreach (var file in Directory.GetFiles(dataPath))
        {
            await foreach (var line in File.ReadLinesAsync(file))
            {
                if (line.StartsWith("Ticker"))
                {
                    continue;
                }

                int startIndex = 0;
                int endIndex = 0;
                string name = string.Empty;
                string value = string.Empty;

                // We know the CSV contains 8 columns.
                for (int column = 0; column < 8; column++)
                {
                    // Find the end of the current column.
                    endIndex = line.IndexOf(',', startIndex);

                    if (column == 0) // The stock name
                    {
                        name = line[startIndex..endIndex];
                    }
                    else if (column == 7)
                    {
                        value = line[startIndex..endIndex];
                    }

                    startIndex = endIndex + 1;
                }

                var change = decimal.Parse(value, CultureInfo.InvariantCulture);

                if (Stocks.ContainsKey(name))
                {
                    var trade = Stocks[name];
                    var min = change < trade.Min ? change : trade.Min;
                    var max = change > trade.Max ? change : trade.Max;
                    var total = trade.Total + change;
                    var count = trade.Count + 1;
                    var average = total / count;

                    Stocks[name] = (count, min, max, average, total);
                }
                else
                {
                    Stocks[name] = (1, change, change, change, change);
                }
            }
        }
    }
}
