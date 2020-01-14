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
        [StringLength(40)]
        [Remote(action: "IsDepartmentExists", controller: "Departments")]
        public string Name { get; set; }
    }
}
