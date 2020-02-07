using DotNetWMS.Resources;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Models
{
    public class Admin_EditUserViewModel
    {
        public Admin_EditUserViewModel()
        {
            Roles = new List<string>();
        }

        public string Id { get; set; }
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [StringLength(30)]
        [Display(Name = "Imię")]
        public string Name { get; set; }
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [StringLength(40)]
        [Display(Name = "Nazwisko")]
        public string Surname { get; set; }
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [Display(Name = "Identyfikator")]
        [StringLength(12)]
        [RegularExpression(@"[0-9]{12}", ErrorMessage = "Nieprawidłowy identyfikator!")]
        public string EmployeeNumber { get; set; }
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [Display(Name = "Oddział")]
        [StringLength(30)]
        public string City { get; set; }
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [EmailAddress]
        [Remote(action: "IsEmailInUse", controller: "Account")]
        public string Email { get; set; }
        public IList<string> Roles { get; set; }
    }
}
