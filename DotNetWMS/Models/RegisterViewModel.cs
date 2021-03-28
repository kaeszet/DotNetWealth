using DotNetWMS.Resources;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Models
{
    /// <summary>
    /// Viewmodel to handle the view with the new user register form
    /// </summary>
    public class RegisterViewModel
    {
        /// <summary>
        /// A field to capture new user's name
        /// </summary>
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [StringLength(30)]
        [Display(Name = "Imię")]
        [RegularExpression(@"[A-Za-zżźćńółęąśŻŹĆĄŚĘŁÓŃ\-\.\'\s]*", ErrorMessage = "{0} nie może zawierać cyfr")]
        public string Name { get; set; }
        /// <summary>
        /// A field to capture new user's surname
        /// </summary>
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [StringLength(40)]
        [Display(Name = "Nazwisko")]
        [RegularExpression(@"[A-Za-zżźćńółęąśŻŹĆĄŚĘŁÓŃ\-\.\'\s]*", ErrorMessage = "{0} nie może zawierać cyfr")]
        public string Surname { get; set; }
        /// <summary>
        /// A field to capture new user's employee number
        /// </summary>
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [Display(Name = "Identyfikator")]
        [StringLength(11)]
        [RegularExpression(@"[0-9]{11}", ErrorMessage = "Nieprawidłowy identyfikator!")]
        public string EmployeeNumber { get; set; }
        /// <summary>
        /// A field to capture new user's employee address
        /// </summary>
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [StringLength(40)]
        [Display(Name = "Adres")]
        public string Street { get; set; }
        /// <summary>
        /// A field to capture new user's employee zip-code
        /// </summary>
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [StringLength(6)]
        [Display(Name = "Kod pocztowy")]
        [RegularExpression(@"[0-9]{2}-[0-9]{3}", ErrorMessage = "Nieprawidłowy format kodu pocztowego xx-xxx")]
        public string ZipCode { get; set; }
        /// <summary>
        /// A field to capture new user's headquarters city
        /// </summary>
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [Display(Name = "Oddział")]
        [StringLength(30)]
        public string City { get; set; }
        /// <summary>
        /// A field to capture new user's email adress
        /// </summary>
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [EmailAddress(ErrorMessage = CustomErrorMessages.IncorrectEmailAdress)]
        [Remote(action: "IsEmailInUse", controller: "Account")]
        public string Email { get; set; }
        /// <summary>
        /// A field to capture new user's password
        /// </summary>
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [Display(Name = "Hasło")]
        [StringLength(30)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        /// <summary>
        /// A field to capture password confirmation
        /// </summary>
        [Display(Name = "Potwierdź hasło")]
        [DataType(DataType.Password)]
        [StringLength(30)]
        [Compare("Password", ErrorMessage = "Hasła nie pasują do siebie")]
        public string ConfirmPassword { get; set; }
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
