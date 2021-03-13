using DotNetWMS.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Models
{
    /// <summary>
    /// Warehouse class model used for creating DB with EFC and collecting info about warehouses
    /// </summary>
    public class Warehouse
    {
        /// <summary>
        /// Warehouse database PK
        /// </summary>
        public int Id { get; set; }
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
        /// <summary>
        /// <c>Location</c> FK
        /// </summary>
        public int? LocationId { get; set; }
        /// <summary>
        /// <c>Location</c> property to create FK with CF method
        /// </summary>
        public Location Location { get; set; }
        /// <summary>
        /// A list of items stored in warehouse
        /// </summary>
        public IList<Item> Items { get; set; }
        /// <summary>
        /// Method to display values in view
        /// </summary>
        public string AssignFullName => $"{Name}, {Street}";
        /// <summary>
        /// Method to display values in documentation view
        /// </summary>
        public string FullNameForDocumentation => $"{Name}{Environment.NewLine}{Street}{Environment.NewLine}{ZipCode} {City}";

    }
}
