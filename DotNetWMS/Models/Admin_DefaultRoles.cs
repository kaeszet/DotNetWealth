using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Models
{
    public class Admin_DefaultRoles
    {
        public Admin_DefaultRoles()
        {
            Roles = new List<string>() 
            { 
                "Admin", "Moderator", "Kadry", "StandardPlus", "Standard"
            };
            
        }
        public IList<string> Roles { get; set; }
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
