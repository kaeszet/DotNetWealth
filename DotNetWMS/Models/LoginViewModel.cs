using DotNetWMS.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Models
{
    /// <summary>
    /// Viewmodel to handle the view with with the login form
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>
        /// A field to capture user's login
        /// </summary>
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        public string Login { get; set; }
        /// <summary>
        /// A field to capture user's password
        /// </summary>
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [Display(Name = "Hasło")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        /// <summary>
        /// A field to capture checkbox status whether the logged user should be remembered
        /// </summary>
        [Display(Name = "Zapamiętaj mnie")]
        public bool RememberMe { get; set; }
    }
}
