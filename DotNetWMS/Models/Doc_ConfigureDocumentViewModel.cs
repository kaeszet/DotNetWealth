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
        [Display(Name = "Od")]
        public string From { get; set; }
        [Display(Name = "Do")]
        public string To { get; set; }
        [Display(Name = "Typ")]
        public int? FromIndex { get; set; }
        public int? ToIndex { get; set; }
        public int? Id { get; set; }
        [Display(Name = "Typ")]
        public string Type { get; set; }
        [Display(Name = "Nazwa")]
        public string Name { get; set; }
        [Display(Name = "Model")]
        public string Model { get; set; }
        [Display(Name = "Producent")]
        public string Producer { get; set; }
        [Display(Name = "Kod przedmiotu")]
        public string Code { get; set; }
        [Display(Name = "Ilość")]
        public string Quantity { get; set; }
        [Display(Name = "Jednostka")]
        public ItemUnits Unit { get; set; }
        [Display(Name = "Dodaj")]
        public bool AddToDocument { get; set; }
    }
}
