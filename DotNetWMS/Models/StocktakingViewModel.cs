using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Models
{
    /// <summary>
    /// Viewmodel to handle the view with stocktaking a warehouse
    /// </summary>
    public class StocktakingViewModel
    {
        /// <summary>
        /// A field to capture list of items
        /// </summary>
        public List<Item> Items { get; set; }
        /// <summary>
        /// A field to capture name and adress of warehouse
        /// </summary>
        [Display(Name = "Wybierz magazyn z listy:")]
        public string WarehouseFullName { get; set; }
    }
}
