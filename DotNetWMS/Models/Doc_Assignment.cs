using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

namespace DotNetWMS.Models
{
    /// <summary>
    /// Doc_Assignment model class used for creating DB with EFC and collecting infos about documents
    /// </summary>
    public class Doc_Assignment
    {
        /// <summary>
        /// Doc_Assignment database PK
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "ID dokumentu")]
        public string DocumentId { get; set; }
        /// <summary>
        /// Doc_Assignment's name
        /// </summary>
        [Display(Name = "Tytuł")]
        public string Title { get; set; }
        /// <summary>
        /// Doc_Assignment's date of creation
        /// </summary>
        [Display(Name = "Data stworzenia")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMMM yyyy HH:mm:ss}")]
        public DateTime CreationDate { get; set; }
        /// <summary>
        /// Doc_Assignment's id of first side of document user 
        /// </summary>
        [Display(Name = "Pracownik wydający")]
        [RegularExpression(@"[({]?[a-fA-F0-9]{8}[-]?([a-fA-F0-9]{4}[-]?){3}[a-fA-F0-9]{12}[})]?", ErrorMessage = "Coś poszło nie tak")]
        public string UserFrom { get; set; }
        /// <summary>
        /// Doc_Assignment's name of first side of document user 
        /// </summary>
        [Display(Name = "Pracownik wydający")]
        public string UserFromName { get; set; }
        /// <summary>
        /// Doc_Assignment's id of second side of document user 
        /// </summary>
        [Display(Name = "Pracownik przyjmujący")]
        [RegularExpression(@"[({]?[a-fA-F0-9]{8}[-]?([a-fA-F0-9]{4}[-]?){3}[a-fA-F0-9]{12}[})]?", ErrorMessage = "Coś poszło nie tak")]
        public string UserTo { get; set; }
        /// <summary>
        /// Doc_Assignment's name of second side of document user 
        /// </summary>
        [Display(Name = "Pracownik przyjmujący")]
        public string UserToName { get; set; }
        /// <summary>
        /// Doc_Assignment's id of first side of document warehouse 
        /// </summary>
        [Display(Name = "Magazyn wydający")]
        public int? WarehouseFrom { get; set; }
        /// <summary>
        /// Doc_Assignment's name of first side of document warehouse 
        /// </summary>
        [Display(Name = "Magazyn wydający")]
        public string WarehouseFromName { get; set; }
        /// <summary>
        /// Doc_Assignment's id of second side of document warehouse 
        /// </summary>
        [Display(Name = "Magazyn przyjmujący")]
        public int? WarehouseTo { get; set; }
        /// <summary>
        /// Doc_Assignment's name of second side of document warehouse 
        /// </summary>
        [Display(Name = "Magazyn przyjmujący")]
        public string WarehouseToName { get; set; }
        /// <summary>
        /// Doc_Assignment's id of first side of document external 
        /// </summary>
        [Display(Name = "Podmiot wydający")]
        public int? ExternalFrom { get; set; }
        /// <summary>
        /// Doc_Assignment's name of first side of document external 
        /// </summary>
        [Display(Name = "Podmiot wydający")]
        public string ExternalFromName { get; set; }
        /// <summary>
        /// Doc_Assignment's id of second side of document external 
        /// </summary>
        [Display(Name = "Podmiot przyjmujący")]
        public int? ExternalTo { get; set; }
        /// <summary>
        /// Doc_Assignment's name of second side of document external 
        /// </summary>
        [Display(Name = "Podmiot przyjmujący")]
        public string ExternalToName{ get; set; }
        /// <summary>
        /// Doc_Assignment's information whether the message has been confirmed by the user or not
        /// </summary>
        [Display(Name = "Potwierdzono?")]
        public bool IsConfirmed { get; set; }
        /// <summary>
        /// Doc_Assignment's date of confirmation
        /// </summary>
        [Display(Name = "Data potwierdzenia")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMMM yyyy HH:mm:ss}")]
        public DateTime? ConfirmationDate { get; set; }
        /// <summary>
        /// List of items (not exists in DB)
        /// </summary>
        private List<Item> _items { get; set; }
        /// <summary>
        /// List of items (not exists in DB)
        /// </summary>
        [NotMapped]
        public List<Item> Items 
        { 
            get { return _items; }
            set { _items = value; }
        }
        /// <summary>
        /// List of items converted to JSON format string
        /// </summary>
        [Required]
        public string ItemsToString
        {
            get { return JsonSerializer.Serialize(_items); }
            set { _items = JsonSerializer.Deserialize<List<Item>>(value); }
        }

    }
}
