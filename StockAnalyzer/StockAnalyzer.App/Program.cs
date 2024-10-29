using StockAnalyzer.Processor;

var result = string.Empty;

var processor = new Processor("D:\\source\\repos\\Perf_Exercices\\StockAnalyzer\\StockAnalyzer.Processor\\Data");

Console.WriteLine("Starting..");

processor.Initialize();

foreach (var stock in processor.Stocks)
{
    var min = processor.Min(stock.Key);
    var max = processor.Max(stock.Key);
    var average = processor.Average(stock.Key);

    result += $"{stock.Key},{min},{max},{average}{Environment.NewLine}";
}

File.WriteAllText("Result.txt", $"{result}");

Console.WriteLine("Completed..");

Console.ReadLine();