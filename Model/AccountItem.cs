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
        /// <summary>
        ///  item is not in depot but only on watch list
        /// should either be shown in different list or shown
        /// in different color
        /// </summary>
        /// <value><c>true</c> if is on watch list; otherwise, <c>false</c>.</value>
        public bool IsOnWatchList { get; set; } = false;
    }
}