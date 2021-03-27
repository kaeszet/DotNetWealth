using DotNetWMS.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Models
{
    public class Admin_ChangePasswordViewModel
    {
        [Display(Name = "Użytkownik")]
        public string FullName { get; set; }
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        public string Login { get; set; }
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [Display(Name = "Stare hasło")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [Display(Name = "Nowe hasło")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [Display(Name = "Potwierdź nowe hasło")]
        [DataType(DataType.Password)]
        public string NewPasswordConfirm { get; set; }
    }
}
