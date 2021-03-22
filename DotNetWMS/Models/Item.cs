using DotNetWMS.Resources;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Models
{
    /// <summary>
    /// Item class model used for creating DB with EFC and collecting info about items
    /// </summary>
    public class Item
    {
        /// <summary>
        /// Item database PK
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Item's type e.g. tools, electronics, clothes etc.
        /// </summary>
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [StringLength(30, ErrorMessage = CustomErrorMessages.MaxLength)]
        [Display(Name = "Rodzaj")]
        public string Type { get; set; }
        /// <summary>
        /// Item's name
        /// </summary>
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [StringLength(30, ErrorMessage = CustomErrorMessages.MaxLength)]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }
        /// <summary>
        /// Item's manufacturer
        /// </summary>
        [StringLength(30, ErrorMessage = CustomErrorMessages.MaxLength)]
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
        [Display(Name = "Kod przedmiotu")]
        [StringLength(100, ErrorMessage = CustomErrorMessages.MaxLength)]
        public string ItemCode { get; set; }
        /// <summary>
        /// Number of items
        /// </summary>
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [Display(Name = "Ilość")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Enum used to select units of measure
        /// </summary>
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [Display(Name = "Jedn.")]
        public ItemUnits Units { get; set; }
        /// <summary>
        /// Item's warranty end date. Can be null.
        /// </summary>
        [Display(Name = "Data gwarancji")]
        [DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime? WarrantyDate { get; set; }
        /// <summary>
        /// Enum to choose the condition of the item. Correlated with the foreign keys of the Employee, Warehouse and External classes for reporting purposes
        /// </summary>
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [Display(Name = "Stan")]
        public ItemState State { get; set; }
        /// <summary>
        /// Employee's FK. Can be null.
        /// </summary>
        [Display(Name = "Przypisz do pracownika:")]
        public string UserId { get; set; }
        /// <summary>
        /// Employee DB data
        /// </summary>
        [Display(Name = "Pracownik")]
        public WMSIdentityUser User { get; set; }
        /// <summary>
        /// Warehouse's FK. Can be null.
        /// </summary>
        [Display(Name = "Przypisz do magazynu:")]
        public int? WarehouseId { get; set; }
        /// <summary>
        /// Warehouse DB data
        /// </summary>
        [Display(Name = "Magazyn")]
        public Warehouse Warehouse { get; set; }
        /// <summary>
        /// External's FK. Can be null.
        /// </summary>
        [Display(Name = "Przypisz do kl. zewn.:")]
        public int? ExternalId { get; set; }
        /// <summary>
        /// External DB data
        /// </summary>
        [Display(Name = "Kl. zewn.")]
        public External External { get; set; }
        /// <summary>
        /// A property to assign Item to Employee, Warehouse or External DB.
        /// </summary>
        public string Assign => $"{ItemCode} {Name} {Producer} {Model}";

    }
}
