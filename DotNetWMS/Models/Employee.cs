using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Models
{
    public class Employee
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Imię")]
        [StringLength(30)]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Nazwisko")]
        [StringLength(40)]
        public string Surname { get; set; }
        [Required]
        [StringLength(11)]
        [RegularExpression(@"[0-9]{11}", ErrorMessage = "Nieprawidłowy format numeru PESEL - 11 cyfr")]
        public string Pesel { get; set; }
        [Display(Name = "Stanowisko")]
        public int? DepartmentId { get; set; }
        [Display(Name = "Stanowisko")]
        public Department Department { get; set; }
        [Required]
        [StringLength(40)]
        [Display(Name = "Adres")]
        public string Street { get; set; }
        [Required]
        [StringLength(6)]
        [Display(Name = "Kod pocztowy")]
        [RegularExpression(@"[0-9]{2}-[0-9]{3}", ErrorMessage = "Nieprawidłowy format kodu pocztowego xx-xxx")]
        public string ZipCode { get; set; }
        [Required]
        [StringLength(30)]
        [Display(Name = "Miasto")]
        public string City { get; set; }
        public IList<Item> Items { get; set; }
        public string FullName => $"{Surname} {Name}";

        
    }
}
