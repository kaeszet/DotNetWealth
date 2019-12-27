using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Models
{
    public class External
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Rodzaj")]
        public string Type { get; set; }
        [Required]
        [StringLength(100)]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }
        [Required]
        [StringLength(10)]
        [Display(Name = "NIP")]
        [RegularExpression(@"[0-9]{10}", ErrorMessage = "Nieprawidłowy format numeru NIP - 10 cyfr")]
        public string TaxId { get; set; }
        [Display(Name = "Ulica")]
        public string Street { get; set; }
        [StringLength(6)]
        [Display(Name = "Kod pocztowy")]
        [RegularExpression(@"[0-9]{2}-[0-9]{3}", ErrorMessage = "Nieprawidłowy format kodu pocztowego xx-xxx")]
        public string ZipCode { get; set; }
        [Display(Name = "Miasto")]
        public string City { get; set; }
        public IList<Item> Items { get; set; }

    }
}
