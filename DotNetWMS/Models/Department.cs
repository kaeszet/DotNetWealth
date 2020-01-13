using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Models
{
    public class Department
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Nazwa")]
        [Remote(action: "IsDepartmentExists", controller: "Departments")]
        public string Name { get; set; }
    }
}
