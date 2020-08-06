using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Models
{
    /// <summary>
    /// Class inheriting from IdentityUser extending its properties with data needed in the application
    /// </summary>
    public class WMSIdentityUser : IdentityUser
    {
        /// <summary>
        /// Employee's name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Employee's surname
        /// </summary>
        public string Surname { get; set; }
        /// <summary>
        /// Employee's registration number
        /// </summary>
        public string EmployeeNumber { get; set; }
        /// <summary>
        /// Employee's headquarters city
        /// </summary>
        public string City { get; set; }
    }
}
