using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Models
{
    /// <summary>
    /// Viewmodel to handle the view with the item status report
    /// </summary>
    public class StatusViewViewModel
    {
        /// <summary>
        /// A field to capture state of an item
        /// </summary>
        [Display(Name = "Status")]
        public ItemState State { get; set; }
        /// <summary>
        /// A field to capture list of items
        /// </summary>
        //public List<Item> Items { get; set; }
        public string UserId { get; set; }
        [Display(Name = "Użytkownik")]
        public string UserFullName { get; set; }
        public int? WarehouseId { get; set; }
        [Display(Name = "Magazyn")]
        public string WarehouseFullName { get; set; }


    }
}
