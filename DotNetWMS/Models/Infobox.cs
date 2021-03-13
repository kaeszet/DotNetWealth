using DotNetWMS.Resources;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Models
{
    /// <summary>
    /// Infobox model class used for creating DB with EFC and collecting infos about messages
    /// </summary>
    public class Infobox
    {
        /// <summary>
        /// Infobox database PK
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Infobox message title
        /// </summary>
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [Display(Name = "Tytuł")]
        [StringLength(30)]
        [RegularExpression(@"[A-Za-zżźćńółęąśŻŹĆĄŚĘŁÓŃ\-\.\'\s]*", ErrorMessage = "{0} nie może zawierać cyfr i znaków specjalnych")]
        public string Title { get; set; }
        /// <summary>
        /// Infobox message body
        /// </summary>
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [Display(Name = "Wiadomość")]
        [StringLength(200)]
        [RegularExpression(@"[A-Za-zżźćńółęąśŻŹĆĄŚĘŁÓŃ\-\.\'\s]*", ErrorMessage = "{0} nie może zawierać cyfr i znaków specjalnych")]
        public string Message { get; set; }
        /// <summary>
        /// Infobox corresponding document ID
        /// </summary>
        public string DocumentId { get; set; }
        /// <summary>
        /// Infobox message date of creation
        /// </summary>
        [Display(Name = "Data otrzymania")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMMM yyyy HH:mm:ss}")]
        public DateTime ReceivedDate { get; set; }
        /// <summary>
        /// Contains information if user check message as read
        /// </summary>
        public bool IsChecked { get; set; }
        /// <summary>
        /// WMSIdentityUser FK
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// WMSIdentityUser property to create FK with CF method
        /// </summary>
        public WMSIdentityUser User { get; set; }

    }
}
