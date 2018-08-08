using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using easystocks.Model;
using Newtonsoft.Json;

namespace EasyStocks.Model
{
    public class AccountItemRepository
    {

      public async Task WriteToFileAsync(IEnumerable<AccountItem> accountItems, string fileName)
      {
        var jsonContent = JsonConvert.SerializeObject(accountItems,
          Formatting.Indented,
          new JsonSerializerSettings
          {
            DefaultValueHandling = DefaultValueHandling.Ignore
          });
        using (var writer = File.CreateText(fileName))
        {
          await writer.WriteAsync(jsonContent);
        }
      }

      public async Task<IEnumerable<AccountItem>> ReadFromFile(string fileName)
      {
      if (File.Exists(fileName))
        {
          try
          {
            using (var reader = File.OpenText(fileName))
            {
              var content = await reader.ReadToEndAsync();
              var accountItems = JsonConvert.DeserializeObject<AccountItemList>(content);
              return accountItems?.Items ?? Enumerable.Empty<AccountItem>(); ;
            }
          }
          catch (Exception exception)
          {
            // TODO: throw error message here
          }
        }

        return Enumerable.Empty<AccountItem>();
      }
    }
}
