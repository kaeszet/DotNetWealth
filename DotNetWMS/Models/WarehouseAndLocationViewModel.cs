using DotNetWMS.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Models
{
    /// <summary>
    /// Viewmodel contains <c>Warehouse</c> and <c>Location</c> properties
    /// </summary>
    public class WarehouseAndLocationViewModel
    {
        public int WarehouseId { get; set; }
        /// <summary>
        /// Warehouse's name
        /// </summary>
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [StringLength(40)]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }
        /// <summary>
        /// Warehouse's address
        /// </summary>
        [StringLength(40)]
        [Display(Name = "Ulica")]
        public string Street { get; set; }
        /// <summary>
        /// Warehouse's postal code
        /// </summary>
        [StringLength(6)]
        [Display(Name = "Kod pocztowy")]
        [RegularExpression(@"[0-9]{2}-[0-9]{3}", ErrorMessage = "Nieprawidłowy format kodu pocztowego xx-xxx")]
        public string ZipCode { get; set; }
        /// <summary>
        /// The city where the warehouse was built
        /// </summary>
        [StringLength(30)]
        [Display(Name = "Miasto")]
        public string City { get; set; }
        /// <summary>
        /// <c>Location</c>'s ID
        /// </summary>
        public int? LocationId { get; set; }
        /// <summary>
        /// <c>Location</c>'s address
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// <c>Location</c>'s longitude
        /// </summary>
        public string Longitude { get; set; }
        /// <summary>
        /// <c>Location</c>'s latitude
        /// </summary>
        public string Latitude { get; set; }
    }
}
