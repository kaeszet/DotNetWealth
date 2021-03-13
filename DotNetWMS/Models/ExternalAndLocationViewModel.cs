using DotNetWMS.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Models
{
    /// <summary>
    /// Viewmodel contains <c>WMSIdentityUser</c> and <c>External</c> properties
    /// </summary>
    public class ExternalAndLocationViewModel
    {
        public int ExternalId { get; set; }
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
        /// Location's ID
        /// </summary>
        public int? LocationId { get; set; }
        /// <summary>
        /// Location's Adress
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Location's longitude
        /// </summary>
        public string Longitude { get; set; }
        /// <summary>
        /// Location's latitude
        /// </summary>
        public string Latitude { get; set; }
       
    }
}
