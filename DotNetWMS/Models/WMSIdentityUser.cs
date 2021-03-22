using DotNetWMS.Resources;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Models
{
    /// <summary>
    /// Class inheriting from IdentityUser extending its properties with data needed in the application
    /// </summary>
    public class WMSIdentityUser : IdentityUser
    {
        /// <summary>
        /// <c>WMSIdentityUser</c>'s name
        /// </summary>
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [Display(Name = "Imię")]
        [StringLength(30)]
        [RegularExpression(@"[A-Za-zżźćńółęąśŻŹĆĄŚĘŁÓŃ\-\.\'\s]*", ErrorMessage = "{0} nie może zawierać cyfr")]
        public string Name { get; set; }
        /// <summary>
        /// <c>WMSIdentityUser</c>'s surname
        /// </summary>
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [Display(Name = "Nazwisko")]
        [StringLength(40)]
        [RegularExpression(@"[A-Za-zżźćńółęąśŻŹĆĄŚĘŁÓŃ\-\.\'\s]*", ErrorMessage = "{0} nie może zawierać cyfr")]
        public string Surname { get; set; }
        /// <summary>
        /// <c>WMSIdentityUser</c>'s registration number
        /// </summary>
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [StringLength(11)]
        [Display(Name = "Identyfikator")]
        [RegularExpression(@"[0-9]{11}", ErrorMessage = "Identyfikator powinien posiadać 11 cyfr")]
        public string EmployeeNumber { get; set; }
        /// <summary>
        /// <c>Department</c>'s ID
        /// </summary>
        [Display(Name = "Stanowisko")]
        public int? DepartmentId { get; set; }
        /// <summary>
        /// <c>Department</c> property to create FK with CF method
        /// </summary>
        [Display(Name = "Stanowisko")]
        public Department Department { get; set; }
        /// <summary>
        /// <c>WMSIdentityUser</c>'s street
        /// </summary>
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [StringLength(40)]
        [Display(Name = "Adres")]
        public string Street { get; set; }
        /// <summary>
        /// <c>WMSIdentityUser</c>'s zip code
        /// </summary>
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [StringLength(6)]
        [Display(Name = "Kod pocztowy")]
        [RegularExpression(@"[0-9]{2}-[0-9]{3}", ErrorMessage = "Nieprawidłowy format kodu pocztowego xx-xxx")]
        public string ZipCode { get; set; }
        /// <summary>
        /// Employee's headquarters city
        /// </summary>
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
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
        /// Property to create one-to-many relationship with Items
        /// </summary>
        public IList<Item> Items { get; set; }
        /// <summary>
        /// Property to create one-to-many relationship with Infobox
        /// </summary>
        public IList<Infobox> Messages { get; set; }
        public int LoginCount { get; set; }
        /// <summary>
        /// Method to display values in view
        /// </summary>
        public string FullName => $"{Surname} {Name}";
        /// <summary>
        /// Method to display values in documentation view
        /// </summary>
        public string FullNameForDocumentation => $"{Surname} {Name}{Environment.NewLine}{EmployeeNumber}{Environment.NewLine}{Street}{Environment.NewLine}{ZipCode} {City}";
    }
}
