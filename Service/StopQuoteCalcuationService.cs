using System;
using System.Collections.Generic;

namespace EasyStocks.Service
{
    public class StopQuoteCalcuationService
    {
        private const float StopRate = 0.9f;

        public void UpdateStopQuotes(IEnumerable<ActualStockItem> actualItems)
        {
            foreach(var item in actualItems)
            {
                if(item.DailyChange > 0)
                {
                    var newStopRate = item.CurrentRate * StopRate;
                    foreach(var accountItem in item.AccountItems)
                    {
                        // stop rate has increased, update it for every item
                        if(newStopRate > accountItem.StopRate)
                        {
                            accountItem.StopRate = newStopRate;
                            accountItem.HasChanged = true;
                        }
                    }
                }
            }
        }
    }
}
