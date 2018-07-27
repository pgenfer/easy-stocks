using System;
using System.Collections.Generic;
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
      var accountItems = await accountItemRepository.ReadFromFile("stocks.json");
      var currentStockItems = await dailyStockDataService.GetDailyInformationForShareAsync(accountItems);
      return Ok(currentStockItems);
    }
  }
}
