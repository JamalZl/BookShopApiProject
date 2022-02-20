using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopMvc.DTOs
{
    public class ListDto<TItem>
    {
        public List<TItem> Items { get; set; }
    }
}
