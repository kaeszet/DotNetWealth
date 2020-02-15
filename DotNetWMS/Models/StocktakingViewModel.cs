using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Models
{
    public class StocktakingViewModel
    {
        public List<Item> Items { get; set; }
        public SelectList Warehouses { get; set; }
        public string Warehouse { get; set; }
        public string SearchString { get; set; }
    }
}
