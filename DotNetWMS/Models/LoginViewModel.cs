using DotNetWMS.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        public string Login { get; set; }

        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [Display(Name = "Hasło")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Zapamiętaj mnie")]
        public bool RememberMe { get; set; }
    }
}
