using System;

namespace EasyStocks.Controllers
{
  public class NewAccountItem
  {
    public string StockName;
    public string StockSymbol;
    public float BuyingRate;
    public DateTime BuyingDate;
    public float StopRate;
        public bool IsOnWatchList;
  }
}