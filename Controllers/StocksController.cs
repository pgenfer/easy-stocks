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

    [HttpGet("[action]")]
    public async Task<ActionResult> FindStockBySymbol([FromQuery]string symbol)
    {
      var dailyStockDataService = new CurrentStockDataService();
      // there can actually be only one stock item since we have used only one symbol here
      var newAccountItem = await dailyStockDataService.FindDailyInformationForShareAsync(symbol);
      return Ok(newAccountItem);
    }

    [HttpPost("[action]")]
    public async Task<ActionResult> NewAccountItem([FromBody]NewAccountItem newAccountItem)
    {
      var accountItemRepository = new AccountItemRepository();
      var accountItems = await accountItemRepository.ReadFromFile("stocks.json");
      var accountItem = new AccountItem
      {
        BuyingDate = newAccountItem.BuyingDate,
        BuyingRate = newAccountItem.BuyingRate,
        Name = newAccountItem.StockName,
        Symbol = newAccountItem.StockSymbol,
        StopRate = newAccountItem.StopRate,
                IsOnWatchList = newAccountItem.IsOnWatchList
      };
      var accountItemList = accountItems.ToList();
      accountItemList.Add(accountItem);
      await accountItemRepository.WriteToFileAsync(accountItemList, "stocks.json");
      return Ok(true);
    }

        [HttpPost("[action]")]
        public async Task<ActionResult> SetOnWatchList([FromBody]SetOnWatchList setOnWatchList)
        {
            var accountItemRepository = new AccountItemRepository();
            var accountItems = await accountItemRepository.ReadFromFile("stocks.json");
            foreach( var accountItem in accountItems.Where(x => x.Symbol == setOnWatchList.Symbol))
            {
                accountItem.IsOnWatchList = setOnWatchList.IsOnWatchList;
            }
            await accountItemRepository.WriteToFileAsync(accountItems, "stocks.json");
            return Ok(true);
        }
  }
}


