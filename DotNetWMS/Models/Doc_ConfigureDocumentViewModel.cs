using DotNetWMS.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Models
{
    public class Doc_ConfigureDocumentViewModel
    {
        /// <summary>
        /// Doc_Assignment's id of first side of document 
        /// </summary>
        [Display(Name = "Od")]
        public string From { get; set; }
        /// <summary>
        /// Doc_Assignment's id of second side of document 
        /// </summary>
        [Display(Name = "Do")]
        public string To { get; set; }
        /// <summary>
        /// Type of first side of document
        /// </summary>
        public int? FromIndex { get; set; }
        /// <summary>
        /// Type of second side of document
        /// </summary>
        public int? ToIndex { get; set; }
        /// <summary>
        /// Item's ID
        /// </summary>
        public int? Id { get; set; }
        /// <summary>
        /// Item's type e.g. tools, electronics, clothes etc.
        /// </summary>
        [Display(Name = "Typ")]
        public string Type { get; set; }
        /// <summary>
        /// Item's name
        /// </summary>
        [Display(Name = "Nazwa")]
        public string Name { get; set; }
        /// <summary>
        /// Item's model name or code
        /// </summary>
        [Display(Name = "Model")]
        public string Model { get; set; }
        /// <summary>
        /// Item's manufacturer
        /// </summary>
        [Display(Name = "Producent")]
        public string Producer { get; set; }
        /// <summary>
        /// Item's model name or code
        /// </summary>
        [Display(Name = "Kod przedmiotu")]
        public string Code { get; set; }
        /// <summary>
        /// Number of items
        /// </summary>
        [Display(Name = "Ilość")]
        public string Quantity { get; set; }
        /// <summary>
        /// Enum used to select units of measure
        /// </summary>
        [Display(Name = "Jednostka")]
        public ItemUnits Unit { get; set; }
        /// <summary>
        /// Contains information if user check corresponding checkbox and mark item add to document
        /// </summary>
        [Display(Name = "Dodaj")]
        public bool AddToDocument { get; set; }
    }
}
