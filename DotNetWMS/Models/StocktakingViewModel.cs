using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Models
{
    public class StocktakingViewModel
    {
        public List<Item> Items { get; set; }
        [Display(Name = "Wybierz magazyn z listy:")]
        public string WarehouseFullName { get; set; }
    }
}
