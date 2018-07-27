using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using easystocks.Model;
using Newtonsoft.Json.Linq;

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
  }


  public class CurrentStockDataService
  {
    /// <summary>
    /// service is needed to retrieve the data for a stock symbol
    /// </summary>
    private readonly HttpClient _yahooFinanceClient = new HttpClient { BaseAddress = new Uri("https://query1.finance.yahoo.com/v8/finance/chart/") };


    public async Task<IEnumerable<ActualStockItem>> GetDailyInformationForShareAsync(
      IEnumerable<AccountItem> accountItems)
    {
      var random = new Random();

      var actualItems = accountItems
        .GroupBy(x => x.Symbol).Select(x => new ActualStockItem(x))
        .OrderBy(x => random.Next())
        .ToList();

      try
      {
        foreach (var item in actualItems)
        {
          var response = await _yahooFinanceClient.GetAsync($"{item.Symbol}?range=2d&interval=1d");
          if (!response.IsSuccessStatusCode)
            continue;
          response.EnsureSuccessStatusCode();
          if (response.IsSuccessStatusCode)
          {
            // get the content from the response message
            var content = response.Content.ReadAsStringAsync();
            // read the result information from the response
            var contentResult = content.Result;
            // convert to json object
            dynamic json = JObject.Parse(contentResult);
            // information we need is in chart -> results -> indicators
            var chart = json.chart;
            var result = chart.result;

            foreach (var resultEntry in result)
            {
              // in case we do not have data for two days, take the last closing quote from meta data
              var meta = resultEntry.meta;
              var chartPreviousClose = float.Parse(meta.chartPreviousClose.ToString(), CultureInfo.CurrentCulture);

              var timestampEntries = resultEntry.timestamp;
              var indicatorEntries = resultEntry.indicators;
              var unadjclosEntries = ((JArray) indicatorEntries.adjclose).Single()["adjclose"];

              var timestamps = new List<float>();
              var unadjcloses = new List<float>();

              foreach (var timestamp in timestampEntries)
                timestamps.Add(float.Parse(timestamp.ToString(), CultureInfo.CurrentCulture));
              foreach (var unadjclose in unadjclosEntries)
                unadjcloses.Add(float.Parse(unadjclose.ToString(), CultureInfo.CurrentCulture));
              // we need at least two timestamp data to calculate the difference
              // if we have only one timestamp, we assume there was no change since the last time
              if (timestamps.Count < 1)
                continue;

              var timeStampOfCurrentValue = 0f;
              var oldValue = 0f;
              var currentValue = 0f;
              var diff = 0f;
              var diffInPercent = 0f;

              if (timestamps.Count == 1 && unadjcloses.Count == 1)
              {
                timeStampOfCurrentValue = timestamps[0];
                currentValue = unadjcloses[0];
                oldValue = chartPreviousClose;
              }

              // ensure that number of entries is correct, otherwise, we have to skip
              if (timestamps.Count >= 2 && unadjcloses.Count >= 2)
              {
                timeStampOfCurrentValue = timestamps[1];
                oldValue = unadjcloses[0];
                currentValue = unadjcloses[1];
              }

              diff = currentValue - oldValue;
              diffInPercent = 100.0f / oldValue * diff;

              var timeOfCurrentValue = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(timeStampOfCurrentValue).ToLocalTime();

              item.CurrentRate = currentValue;
              item.LastTradingDate = timeOfCurrentValue;
              item.DailyChange = diff;
              item.DailyChangeInPercent = diffInPercent;
            }
          }
        }

        return actualItems;
      }
      catch (Exception ex)
      {

      }
      return Enumerable.Empty<ActualStockItem>();
    }
  }
}
