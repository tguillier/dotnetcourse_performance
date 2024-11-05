using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace StockAnalyzer.Processor;

public class ProcessorFaster(string dataPath = "Data")
{
    public Dictionary<string, Stock> Stocks { get; set; } = new Dictionary<string, Stock>();

    public void Initialize()
    {
        foreach (var file in Directory.GetFiles(dataPath))
        {
            var content = File.ReadAllText(file);
            var lines = content.Split('\n');

            //foreach (var line in lines[1..]) // Skip the first line
            for (int i = 1; i < lines.Length; i++)
            {
                var line = lines[i];
                //var csv = line.Split(',');

                //if (csv.Length < 8)
                //{
                //    continue;
                //}

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

                var trade = new Trade(
                    DateTime.MinValue,
                    decimal.MinValue,
                    decimal.Parse(value, CultureInfo.InvariantCulture),
                    decimal.MinValue);

                if (!Stocks.ContainsKey(name))
                {
                    Stocks[name] = new Stock(name);
                }

                Stocks[name].Trades.Add(trade);
            }
        }
    }

    public (decimal min, decimal max, decimal average) GetReport(string ticker)
    {
        var min = decimal.MaxValue;
        var max = decimal.MinValue;
        var total = 0M;
        var count = 0;

        foreach (var trade in Stocks[ticker].Trades)
        {
            if (trade.Change < min) min = trade.Change;
            if (trade.Change > max) max = trade.Change;

            total += trade.Change;
            count++;
        }

        var average = total / count;

        return (min, max, average);
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
