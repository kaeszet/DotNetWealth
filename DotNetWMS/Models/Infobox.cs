using DotNetWMS.Resources;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Models
{
    public class Infobox
    {
        public int Id { get; set; }
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [Display(Name = "Tytuł")]
        [StringLength(30)]
        [RegularExpression(@"[A-Za-zżźćńółęąśŻŹĆĄŚĘŁÓŃ\-\.\'\s]*", ErrorMessage = "{0} nie może zawierać cyfr i znaków specjalnych")]
        public string Title { get; set; }
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [Display(Name = "Wiadomość")]
        [StringLength(200)]
        [RegularExpression(@"[A-Za-zżźćńółęąśŻŹĆĄŚĘŁÓŃ\-\.\'\s]*", ErrorMessage = "{0} nie może zawierać cyfr i znaków specjalnych")]
        public string Message { get; set; }
        [Display(Name = "Data otrzymania")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMMM yyyy HH:mm:ss}")]
        public DateTime ReceivedDate { get; set; }
        public bool IsChecked { get; set; }
        public string UserId { get; set; }
        public WMSIdentityUser User { get; set; }

    }
}
