using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Models
{
    /// <summary>
    /// Class to handle the default roles in the application
    /// </summary>
    public class Admin_DefaultRoles
    {
        /// <summary>
        /// Class constructor with a list of default roles. Enables you to edit them by changing the "Roles" list
        /// </summary>
        public Admin_DefaultRoles()
        {
            Roles = new List<string>() 
            { 
                "Admin", "Moderator", "Kadry", "StandardPlus", "Standard"
            };
            
        }
        /// <summary>
        /// List of roles stored as 'string' values
        /// </summary>
        public IList<string> Roles { get; set; }
        /// <summary>
        /// A method that checks if the selected role is the default one
        /// </summary>
        /// <param name="roleName">A name of the role taken from the user in the text field of the application</param>
        /// <returns>If the role is on the default role list, returns true. Otherwise - false.</returns>
        public static bool IsDefaultRole(string roleName)
        {
            var defaultRoles = new Admin_DefaultRoles().Roles;
            foreach (string role in defaultRoles)
            {
                if (roleName == role)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
