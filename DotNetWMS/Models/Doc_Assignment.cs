using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Models
{
    public class Doc_Assignment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string DocumentId { get; set; }
        public string Title { get; set; }
        [Display(Name = "Data stworzenia")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMMM yyyy HH:mm:ss}")]
        public DateTime CreationDate { get; set; }
        [Display(Name = "Pracownik wydający")]
        [RegularExpression(@"[({]?[a-fA-F0-9]{8}[-]?([a-fA-F0-9]{4}[-]?){3}[a-fA-F0-9]{12}[})]?")]
        public string UserFrom { get; set; }
        [Display(Name = "Pracownik przyjmujący")]
        [RegularExpression(@"[({]?[a-fA-F0-9]{8}[-]?([a-fA-F0-9]{4}[-]?){3}[a-fA-F0-9]{12}[})]?")]
        public string UserTo { get; set; }
        [Display(Name = "Magazyn wydający")]
        public int? WarehouseFrom { get; set; }
        [Display(Name = "Magazyn przyjmujący")]
        public int? WarehouseTo { get; set; }
        [Display(Name = "Podmiot wydający")]
        public int? ExternalFrom { get; set; }
        [Display(Name = "Podmiot przyjmujący")]
        public int? ExternalTo { get; set; }
        public bool IsConfirmed { get; set; }
        [Display(Name = "Data potwierdzenia")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMMM yyyy HH:mm:ss}")]
        public DateTime? ConfirmationDate { get; set; }
        private List<String> _items { get; set; }
        [NotMapped]
        public List<string> Items 
        { 
            get { return _items; }
            set { _items = value; }
        }
        [Required]
        public string ItemsToString
        {
            get { return String.Join(',', _items); }
            set { _items = value.Split(',').ToList(); }
        }



        //public int? EmployeeId { get; set; }
        //public Employee Employee { get; set; }
        //public int? WarehouseId { get; set; }
        //public Warehouse Warehouse { get; set; }
        //public int? ExternalId { get; set; }
        //public External External { get; set; }
    }
}
