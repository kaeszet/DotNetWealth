using DotNetWMS.Resources;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Models
{
    /// <summary>
    /// Viewmodel to handle the list of roles assigned to a given user, and for editing user's data in corresponding view
    /// </summary>
    public class Admin_EditUserViewModel
    {
        /// <summary>
        /// Constructor for creating a new roles list
        /// </summary>
        public Admin_EditUserViewModel()
        {
            Roles = new List<string>();
        }
        /// <summary>
        /// Stores the user ID in IFC database
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// A field to capture the user's name
        /// </summary>
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [StringLength(30)]
        [Display(Name = "Imię")]
        public string Name { get; set; }
        /// <summary>
        /// A field to capture the user's surname
        /// </summary>
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [StringLength(40)]
        [Display(Name = "Nazwisko")]
        public string Surname { get; set; }
        /// <summary>
        /// A field to capture the user's employee number. 12 digits from 0 to 9 are required.
        /// </summary>
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [Display(Name = "Identyfikator")]
        [StringLength(12)]
        [RegularExpression(@"[0-9]{12}", ErrorMessage = "Nieprawidłowy identyfikator!")]
        public string EmployeeNumber { get; set; }
        /// <summary>
        /// A field to capture the user's home city
        /// </summary>
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [Display(Name = "Oddział")]
        [StringLength(30)]
        public string City { get; set; }
        /// <summary>
        /// A field to capture the user's email
        /// </summary>
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [EmailAddress]
        [Remote(action: "IsEmailInUse", controller: "Account")]
        public string Email { get; set; }
        /// <summary>
        /// List of roles stored as 'string' values
        /// </summary>
        public IList<string> Roles { get; set; }
    }
}
