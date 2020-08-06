using DotNetWMS.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Models
{
    /// <summary>
    /// Viewmodel class for creating a role by a administrator privileges user
    /// </summary>
    public class Admin_CreateRoleViewModel
    {
        /// <summary>
        /// The role's name for an application user
        /// </summary>
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [Display(Name = "Podaj nazwę roli:")]
        public string RoleName { get; set; }
    }
}
