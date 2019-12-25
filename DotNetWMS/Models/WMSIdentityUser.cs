using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Models
{
    public class WMSIdentityUser : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string EmployeeNumber { get; set; }
        public string City { get; set; }
    }
}
