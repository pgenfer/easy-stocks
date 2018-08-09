using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using easystocks.Model;
using EasyStocks.Model;
using EasyStocks.Service;
using Microsoft.AspNetCore.Mvc;

namespace EasyStocks.Controllers
{
  [Route("api/[controller]")]
  public class StocksController : Controller
  {
    [HttpGet("[action]")]
    public async Task<ActionResult> CurrentAccountItems()
    {
            var accountItemRepository = new AccountItemRepository();
            var dailyStockDataService = new CurrentStockDataService();
            var stopQuoteCalculationService = new StopQuoteCalcuationService();
            var accountItems = await accountItemRepository.ReadFromFile("stocks.json");
            var currentStockItems = await dailyStockDataService.GetDailyInformationForShareAsync(accountItems);
            stopQuoteCalculationService.UpdateStopQuotes(currentStockItems);
            if(currentStockItems.SelectMany(x => x.AccountItems).Any(x => x.HasChanged))
            {
                await accountItemRepository.WriteToFileAsync(currentStockItems.SelectMany(x => x.AccountItems), "stocks.json");
            }
               
            return Ok(currentStockItems.OrderByDescending(x => x.DailyChangeInPercent));
    }

    [HttpGet("[action]")]
    public async Task<ActionResult> AccountItemBySymbol([FromQuery]string symbol)
    {
      var accountItemRepository = new AccountItemRepository();
      var dailyStockDataService = new CurrentStockDataService();
      var accountItems = await accountItemRepository.ReadFromFile("stocks.json");
      var accountItemsForSymbol = accountItems
        .Where(x => string.Compare(x.Symbol,symbol,CultureInfo.CurrentCulture,CompareOptions.IgnoreCase) == 0);
      // there can actually be only one stock item since we have used only one symbol here
      var currentStockItems = await dailyStockDataService
        .GetDailyInformationForShareAsync(accountItemsForSymbol);
      // TODO: should we also update the stop rate here? maybe...
      return Ok(currentStockItems.FirstOrDefault());
    }
  }
}
