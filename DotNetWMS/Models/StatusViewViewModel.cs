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
        /// <c>WMSIdentityUser</c> ID
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// Fullname of <c>WMSIdentityUser</c>
        /// </summary>
        [Display(Name = "Użytkownik")]
        public string UserFullName { get; set; }
        /// <summary>
        /// <c>Warehouse</c> ID
        /// </summary>
        public int? WarehouseId { get; set; }
        /// <summary>
        /// Fullname of <c>Warehouse</c>
        /// </summary>
        [Display(Name = "Magazyn")]
        public string WarehouseFullName { get; set; }


    }
}
