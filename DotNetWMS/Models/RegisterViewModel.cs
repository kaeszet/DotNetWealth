using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Models
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(30)]
        [Display(Name = "Imię")]
        public string Name { get; set; }
        [Required]
        [StringLength(40)]
        [Display(Name = "Nazwisko")]
        public string Surname { get; set; }
        [Required]
        [Display(Name = "Identyfikator")]
        [StringLength(12)]
        [RegularExpression(@"[0-9]{12}", ErrorMessage = "Nieprawidłowy identyfikator!")]
        public string EmployeeNumber { get; set; }
        [Required]
        [Display(Name = "Oddział")]
        [StringLength(30)]
        public string City { get; set; }
        [Required]
        [EmailAddress]
        [Remote(action: "IsEmailInUse", controller: "Account")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Hasło")]
        [StringLength(30)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Potwierdź hasło")]
        [DataType(DataType.Password)]
        [StringLength(30)]
        [Compare("Password", ErrorMessage = "Hasła nie pasują do siebie")]
        public string ConfirmPassword { get; set; }
        

    }
}
