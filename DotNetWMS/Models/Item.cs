using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Models
{
    public class Item
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Rodzaj")]
        public string Type { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }
        [StringLength(50)]
        [Display(Name = "Producent")]
        public string Producer { get; set; }
        [StringLength(50)]
        public string Model { get; set; }
        [Display(Name = "Kod przedmiotu")]
        public string ItemCode { get; set; }
        [Required]
        [Display(Name = "Ilość")]
        public decimal Quantity { get; set; }
        [Required]
        [Display(Name = "Jedn.")]
        public ItemUnits Units { get; set; }
        [Display(Name = "Data gwarancji")]
        [DataType(DataType.Date)]
        public DateTime WarrantyDate { get; set; }
        [Required]
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

    }
}
