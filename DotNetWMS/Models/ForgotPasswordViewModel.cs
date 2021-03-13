using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Models
{
    /// <summary>
    /// Viewmodel to handle the view with the application password recovery form
    /// </summary>
    public class ForgotPasswordViewModel
    {
        /// <summary>
        /// Email adress connected with user account
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
