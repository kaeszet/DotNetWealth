using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

namespace DotNetWMS.Models
{
    public class Doc_Assignment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "ID dokumentu")]
        public string DocumentId { get; set; }
        [Display(Name = "Tytuł")]
        public string Title { get; set; }
        [Display(Name = "Data stworzenia")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMMM yyyy HH:mm:ss}")]
        public DateTime CreationDate { get; set; }
        [Display(Name = "Pracownik wydający")]
        [RegularExpression(@"[({]?[a-fA-F0-9]{8}[-]?([a-fA-F0-9]{4}[-]?){3}[a-fA-F0-9]{12}[})]?", ErrorMessage = "Coś poszło nie tak")]
        public string UserFrom { get; set; }
        [Display(Name = "Pracownik wydający")]
        public string UserFromName { get; set; }
        [Display(Name = "Pracownik przyjmujący")]
        [RegularExpression(@"[({]?[a-fA-F0-9]{8}[-]?([a-fA-F0-9]{4}[-]?){3}[a-fA-F0-9]{12}[})]?", ErrorMessage = "Coś poszło nie tak")]
        public string UserTo { get; set; }
        [Display(Name = "Pracownik przyjmujący")]
        public string UserToName { get; set; }
        [Display(Name = "Magazyn wydający")]
        public int? WarehouseFrom { get; set; }
        [Display(Name = "Magazyn wydający")]
        public string WarehouseFromName { get; set; }
        [Display(Name = "Magazyn przyjmujący")]
        public int? WarehouseTo { get; set; }
        [Display(Name = "Magazyn przyjmujący")]
        public string WarehouseToName { get; set; }
        [Display(Name = "Podmiot wydający")]
        public int? ExternalFrom { get; set; }
        [Display(Name = "Podmiot wydający")]
        public string ExternalFromName { get; set; }
        [Display(Name = "Podmiot przyjmujący")]
        public int? ExternalTo { get; set; }
        [Display(Name = "Podmiot przyjmujący")]
        public string ExternalToName{ get; set; }
        [Display(Name = "Potwierdzono?")]
        public bool IsConfirmed { get; set; }
        [Display(Name = "Data potwierdzenia")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMMM yyyy HH:mm:ss}")]
        public DateTime? ConfirmationDate { get; set; }
        private List<Item> _items { get; set; }
        [NotMapped]
        public List<Item> Items 
        { 
            get { return _items; }
            set { _items = value; }
        }
        [Required]
        public string ItemsToString
        {
            get { return JsonSerializer.Serialize(_items); }
            set { _items = JsonSerializer.Deserialize<List<Item>>(value); }
        }

    }
}
