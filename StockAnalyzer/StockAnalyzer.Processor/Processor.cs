using System.Globalization;

namespace StockAnalyzer.Processor;

public class Processor(string dataPath)
{
    public Dictionary<string, Stock> Stocks { get; set; } = new Dictionary<string, Stock>();

    public void Initialize()
    {
        foreach (var file in Directory.GetFiles(dataPath))
        {
            var content = File.ReadAllText(file);
            var lines = content.Split('\n');

            foreach (var line in lines[1..]) // Skip the first line
            {
                var csv = line.Split(',');

                if (csv.Length < 8)
                {
                    continue;
                }

                var trade = new Trade(
                    DateTime.Parse(csv[1], CultureInfo.InvariantCulture),
                    decimal.Parse(csv[6], CultureInfo.InvariantCulture),
                    decimal.Parse(csv[7], CultureInfo.InvariantCulture),
                    decimal.Parse(csv[8], CultureInfo.InvariantCulture));

                if (!Stocks.ContainsKey(csv[0]))
                {
                    Stocks[csv[0]] = new Stock(csv[0]);
                }

                Stocks[csv[0]].Trades.Add(trade);
            }
        }
    }

    public decimal Min(string ticker)
    {
        decimal min = decimal.MaxValue;

        foreach (var trade in Stocks[ticker].Trades)
        {
            if (trade.Change < min) min = trade.Change;
        }

        return min;
    }

    public decimal Max(string ticker)
    {
        decimal max = decimal.MinValue;

        foreach (var trade in Stocks[ticker].Trades)
        {
            if (trade.Change > max) max = trade.Change;
        }

        return max;
    }

    public decimal Average(string ticker)
    {
        decimal total = 0;

        foreach (var trade in Stocks[ticker].Trades)
        {
            total += trade.Change;
        }

        return total / Stocks[ticker].Trades.Count;
    }
}
