using DotNetWMS.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Models
{
    /// <summary>
    /// Viewmodel cointains <c>Item</c>'s properties and other corresponding with checkboxes in StocktakingTable partial view
    /// </summary>
    public class StocktakingTableViewModel
    {
        /// <summary>
        /// <c>Item</c> type
        /// </summary>
        [Display(Name = "Typ")]
        public string ItemType { get; set; }
        /// <summary>
        /// <c>Item</c> name
        /// </summary>
        [Display(Name = "Nazwa")]
        public string ItemName { get; set; }
        /// <summary>
        /// <c>Item</c> model
        /// </summary>
        [Display(Name = "Model")]
        public string ItemModel { get; set; }
        /// <summary>
        /// <c>Item</c> code
        /// </summary>
        [Display(Name = "Kod przedmiotu")]
        public string ItemCode { get; set; }
        /// <summary>
        /// <c>Item</c> quantity
        /// </summary>
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        //[Display(Name = "Ilość")]
        //[Range(0.01, 10000, ErrorMessage = CustomErrorMessages.NumberRange)]
        //public decimal ItemQuantity { get; set; }
        [Display(Name = "Ilość")]
        [RegularExpression(@"^(?=.*[1-9])[0-9]{0,4}[.,]?[0-9]{1,2}$", ErrorMessage = "Liczba nie może być mniejsza od 0 i większa od 10000")]
        public string ItemQuantity { get; set; }
        /// <summary>
        /// <c>Item</c> units
        /// </summary>
        [Display(Name = "Jednostka")]
        public ItemUnits ItemUnit { get; set; }
        /// <summary>
        /// True if item details need to be corrected, otherwise - false 
        /// </summary>
        [Display(Name = "Do poprawy?")]
        public bool ToCorrect { get; set; }
        /// <summary>
        /// True if item details need to be deleted, otherwise - false 
        /// </summary>
        [Display(Name = "Do usunięcia?")]
        public bool ToDelete { get; set; }
        /// <summary>
        /// True if item is damaged, otherwise - false 
        /// </summary>
        [Display(Name = "Uszkodzony")]
        public bool IsDamaged { get; set; }
    }
}
