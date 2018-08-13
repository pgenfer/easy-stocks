using System;
using System.Collections.Generic;
using System.Linq;
using easystocks.Model;

namespace EasyStocks.Service
{
  public class ActualStockItem
  {
    public ActualStockItem(IEnumerable<AccountItem> accountItems)
    {
      AccountItems = new List<AccountItem>(accountItems);
      Name = AccountItems.FirstOrDefault()?.Name ?? "no name";
      Symbol = AccountItems.FirstOrDefault()?.Symbol ?? "no symbol";
    }

    public string Name { get; }
    public string Symbol { get; }
    public List<AccountItem> AccountItems { get; } // there can be more than one account item per stock
    public float CurrentRate { get; set; }
    public float DailyChange { get; set; }
    public float DailyChangeInPercent { get; set; }
    public DateTime LastTradingDate { get; set; }
    public bool IsStopRateReached => !AccountItems.All(x => x.StopRate < CurrentRate);
    public bool IsPositive => DailyChange >= 0;
    public float DailyChangeInPercentAbsolute => Math.Abs(DailyChangeInPercent);

    public override string ToString() => $"{Name} {Symbol}";
  }
}