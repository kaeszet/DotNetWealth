using DotNetWMS.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Models
{
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
        /// Warehouse address
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
        public int? LocationId { get; set; }
        public string Address { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
    }
}
