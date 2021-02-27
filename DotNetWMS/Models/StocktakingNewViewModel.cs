using DotNetWMS.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Models
{
    public class StocktakingNewViewModel
    {
        [Display(Name = "Typ")]
        public string ItemType { get; set; }
        [Display(Name = "Nazwa")]
        public string ItemName { get; set; }
        [Display(Name = "Model")]
        public string ItemModel { get; set; }
        [Display(Name = "Kod przedmiotu")]
        public string ItemCode { get; set; }
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        //[Display(Name = "Ilość")]
        //[Range(0.01, 10000, ErrorMessage = CustomErrorMessages.NumberRange)]
        //public decimal ItemQuantity { get; set; }
        [Display(Name = "Ilość")]
        [RegularExpression(@"^(?=.*[1-9])[0-9]{0,4}[.,]?[0-9]{1,2}$", ErrorMessage = "Liczba nie może być mniejsza od 0 i większa od 10000")]
        public string ItemQuantity { get; set; }
        [Display(Name = "Jednostka")]
        public ItemUnits ItemUnit { get; set; }
        [Display(Name = "Do poprawy?")]
        public bool ToCorrect { get; set; }
        [Display(Name = "Do usunięcia?")]
        public bool ToDelete { get; set; }
        [Display(Name = "Uszkodzony")]
        public bool IsDamaged { get; set; }
    }
}
