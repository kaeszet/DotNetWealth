using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Models
{
    /// <summary>
    /// Viewmodel to handle the view with the new password selection form
    /// </summary>
    public class ResetPasswordViewModel
    {
        /// <summary>
        /// A field to capture an email address of the account which password will be changed
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        /// <summary>
        /// A field to capture a new password
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }
        /// <summary>
        /// A field to capture a new password confirmation
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź hasło")]
        [Compare("Password", ErrorMessage = "Hasła muszą pasować do siebie!")]
        public string ConfirmPassword { get; set; }
        /// <summary>
        /// A field to capture security token
        /// </summary>
        public string Token { get; set; }
    }
}
