using DotNetWMS.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Models
{
    public class Admin_EditRoleViewModel
    {
        public Admin_EditRoleViewModel()
        {
            Users = new List<string>();
        }

        public string Id { get; set; }

        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [Display(Name = "Nazwa roli:")]
        public string RoleName { get; set; }

        public List<string> Users { get; set; }
    }
}
