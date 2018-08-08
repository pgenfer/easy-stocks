using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace easystocks.Model
{
    public class AccountItem
    {
      public string Name { get; set; }
      public string Symbol { get; set; }
      public float BuyingRate { get; set; }
      public DateTime BuyingDate { get; set; }
      public float StopRate { get; set; }
      public bool HasChanged { get; set; }
    }
}