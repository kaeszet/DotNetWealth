using DotNetWMS.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Models
{
    /// <summary>
    /// Viewmodel contains <c>WMSIdentityUser</c> and <c>Location</c> properties
    /// </summary>
    public class UserAndLocationViewModel
    {
        /// <summary>
        /// <c>WMSIdentityUser</c>'s ID
        /// </summary>
        public string UserId { get; set; }
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
        /// <c>WMSIdentityUser</c>'s street
        /// </summary>
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [StringLength(40)]
        [Display(Name = "Adres")]
        public string Street { get; set; }
        /// <summary>
        /// <c>WMSIdentityUser</c>'s zipcode
        /// </summary>
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [StringLength(6)]
        [Display(Name = "Kod pocztowy")]
        [RegularExpression(@"[0-9]{2}-[0-9]{3}", ErrorMessage = "Nieprawidłowy format kodu pocztowego xx-xxx")]
        public string ZipCode { get; set; }
        /// <summary>
        /// <c>WMSIdentityUser</c>'s headquarters city
        /// </summary>
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
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
