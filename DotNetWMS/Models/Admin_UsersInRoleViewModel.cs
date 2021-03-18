using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Models
{
    /// <summary>
    /// Viewmodel that supports the view to check if the checkbox has been checked by the user
    /// </summary>
    public class Admin_UsersInRoleViewModel
    {
        /// <summary>
        /// Stores the user ID in DB
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// Stores name and surname of user
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// Stores employee number
        /// </summary>
        public string EmployeeNumber { get; set; }
        /// <summary>
        /// A field to capture the user's login
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// True, if a checkbox was selected, otherwise - false
        /// </summary>
        public bool IsSelected { get; set; }
    }
}
