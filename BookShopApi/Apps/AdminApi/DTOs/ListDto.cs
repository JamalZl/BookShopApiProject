using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopApi.Apps.AdminApi.DTOs
{
    public class ListDto<TItem>
    {
        public List<TItem> Items { get; set; }
        public int TotalCount { get; set; }
    }
}
