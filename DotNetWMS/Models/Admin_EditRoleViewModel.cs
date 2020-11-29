using DotNetWMS.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Models
{
    /// <summary>
    /// Viewmodel to handle the list of users assigned to a given role, and for editing name of the role in corresponding view
    /// </summary>
    public class Admin_EditRoleViewModel
    {
        /// <summary>
        /// Constructor for creating a new users list
        /// </summary>
        public Admin_EditRoleViewModel()
        {
            Users = new List<string>();
        }
        /// <summary>
        /// Stores the role ID in IFC database
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// A field to capture the role name
        /// </summary>
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [Display(Name = "Nazwa roli")]
        public string RoleName { get; set; }
        /// <summary>
        /// List of users stored as 'string' values
        /// </summary>
        public List<string> Users { get; set; }
    }
}
