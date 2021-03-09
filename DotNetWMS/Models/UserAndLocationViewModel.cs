using DotNetWMS.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Models
{
    public class UserAndLocationViewModel
    {
        public string UserId { get; set; }
        /// <summary>
        /// Employee's name
        /// </summary>
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [Display(Name = "Imię")]
        [StringLength(30)]
        [RegularExpression(@"[A-Za-zżźćńółęąśŻŹĆĄŚĘŁÓŃ\-\.\'\s]*", ErrorMessage = "{0} nie może zawierać cyfr")]
        public string Name { get; set; }
        /// <summary>
        /// Employee's surname
        /// </summary>
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [Display(Name = "Nazwisko")]
        [StringLength(40)]
        [RegularExpression(@"[A-Za-zżźćńółęąśŻŹĆĄŚĘŁÓŃ\-\.\'\s]*", ErrorMessage = "{0} nie może zawierać cyfr")]
        public string Surname { get; set; }
        /// <summary>
        /// Employee's registration number
        /// </summary>
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [StringLength(11)]
        [Display(Name = "Identyfikator")]
        [RegularExpression(@"[0-9]{11}", ErrorMessage = "Identyfikator powinien posiadać 11 cyfr")]
        public string EmployeeNumber { get; set; }
        [Display(Name = "Stanowisko")]
        public int? DepartmentId { get; set; }
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [StringLength(40)]
        [Display(Name = "Adres")]
        public string Street { get; set; }
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
        public int? LocationId { get; set; }
        public string Address { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
    }
}
