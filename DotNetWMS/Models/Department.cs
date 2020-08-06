using DotNetWMS.Resources;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Models
{
    /// <summary>
    /// Department class model used for creating DB with EFC and collecting info about departments
    /// </summary>
    public class Department
    {
        /// <summary>
        /// Department database PK
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Department's name
        /// </summary>
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [Display(Name = "Nazwa")]
        [StringLength(40)]
        [Remote(action: "IsDepartmentExists", controller: "Departments")]
        public string Name { get; set; }
    }
}
