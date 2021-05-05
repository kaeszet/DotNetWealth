using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DotNetWMS.Resources;

namespace DotNetWMS.Models
{
    /// <summary>
    /// External class model used for creating DB with EFC and collecting info about external clients or services
    /// </summary>
    public class External
    {
        /// <summary>
        /// External database PK
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Enum used to catalog and select the type of client 
        /// </summary>
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [Display(Name = "Rodzaj")]
        public ContractorType Type { get; set; }
        /// <summary>
        /// External's name
        /// </summary>
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [StringLength(50)]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }
        /// <summary>
        /// External's tax identification number
        /// </summary>
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [StringLength(10)]
        [Display(Name = "NIP")]
        [RegularExpression(@"[0-9]{10}", ErrorMessage = "Nieprawidłowy format numeru NIP - 10 cyfr")]
        public string TaxId { get; set; }
        /// <summary>
        /// External's address
        /// </summary>
        [StringLength(40)]
        [Display(Name = "Ulica")]
        public string Street { get; set; }
        /// <summary>
        /// External's postal code
        /// </summary>
        [StringLength(6)]
        [Display(Name = "Kod pocztowy")]
        [RegularExpression(@"[0-9]{2}-[0-9]{3}", ErrorMessage = "Nieprawidłowy format kodu pocztowego xx-xxx")]
        public string ZipCode { get; set; }
        /// <summary>
        /// External's headquarters city
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
        /// List of items which was transferred to the client or external service
        /// </summary>
        public IList<Item> Items { get; set; }
        /// <summary>
        /// Method to display values in view
        /// </summary>
        public string FullName => $"{Name}, {TaxId}";
        public string TypeAndName => $"{Type}, {Name}";
        /// <summary>
        /// Method to display values in documentation view
        /// </summary>
        public string FullNameForDocumentation => $"{Name}{Environment.NewLine}{TaxId}{Environment.NewLine}{Street}{Environment.NewLine}{ZipCode} {City}";

    }
}
