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
    public class Item
    {
        public int Id { get; set; }
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [StringLength(30)]
        [Display(Name = "Rodzaj")]
        public string Type { get; set; }
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [StringLength(30)]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }
        [StringLength(30)]
        [Display(Name = "Producent")]
        public string Producer { get; set; }
        [StringLength(30)]
        public string Model { get; set; }
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [Display(Name = "Kod przedmiotu")]
        [StringLength(30)]
        public string ItemCode { get; set; }
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [Display(Name = "Ilość")]
        public decimal Quantity { get; set; }
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [Display(Name = "Jedn.")]
        public ItemUnits Units { get; set; }
        [Display(Name = "Data gwarancji")]
        [DataType(DataType.Date)]
        public DateTime? WarrantyDate { get; set; }
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [Display(Name = "Stan")]
        public ItemState State { get; set; }
        [Display(Name = "Przypisz do pracownika:")]
        public int? EmployeeId { get; set; }
        [Display(Name = "Pracownik")]
        public Employee Employee { get; set; }
        [Display(Name = "Przypisz do magazynu:")]
        public int? WarehouseId { get; set; }
        [Display(Name = "Magazyn")]
        public Warehouse Warehouse { get; set; }
        [Display(Name = "Przypisz do kl. zewn.:")]
        public int? ExternalId { get; set; }
        [Display(Name = "Kl. zewn.")]
        public External External { get; set; }
        public string Assign => $"{ItemCode} {Name} {Producer} {Model}";

    }
}
