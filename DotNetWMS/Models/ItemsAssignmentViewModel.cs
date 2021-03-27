using DotNetWMS.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Models
{
    public class ItemsAssignmentViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [Display(Name = "Kod przedmiotu")]
        [StringLength(100)]
        public string ItemCode { get; set; }
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [StringLength(30)]
        [Display(Name = "Rodzaj")]
        public string Type { get; set; }
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [StringLength(30)]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }
        /// <summary>
        /// Item's manufacturer
        /// </summary>
        [StringLength(30)]
        [Display(Name = "Producent")]
        public string Producer { get; set; }
        /// <summary>
        /// Item's model name or code
        /// </summary>
        [StringLength(30)]
        public string Model { get; set; }
        /// <summary>
        /// Item's individual code that can be used to work with the scanner etc.
        /// </summary>
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [Display(Name = "Ilość")]
        [Range(0.01, 100000, ErrorMessage = "Wprowadzona wartość musi być większa od zera!")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Enum used to select units of measure
        /// </summary>
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [Display(Name = "Jedn.")]
        public ItemUnits Units { get; set; }
        [Display(Name = "Wybierz przedm.")]
        public bool IsChecked { get; set; }
        [Display(Name = "Aktualnie przypisany do:")]
        public List<string> Records { get; set; }
    }
}
