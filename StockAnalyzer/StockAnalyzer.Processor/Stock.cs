namespace StockAnalyzer.Processor;

public class Stock(string ticker) : IComparable<Stock>
{
    public IList<Trade> Trades { get; } = new List<Trade>();
    public decimal Min { get; set; } = decimal.MaxValue;
    public decimal Max { get; set; } = decimal.MinValue;
    public decimal Total { get; set; } = 0;
    public decimal Average => Total / Trades.Count;
    public string Ticker { get; set; } = ticker;

    public int CompareTo(Stock? other)
    {
        return string.Compare(Ticker, other.Ticker);
    }
}