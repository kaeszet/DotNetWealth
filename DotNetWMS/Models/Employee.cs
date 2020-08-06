using DotNetWMS.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Models
{
    /// <summary>
    /// Employee class model used for creating DB with EFC and collecting info about employees
    /// </summary>
    public class Employee
    {
        /// <summary>
        /// Employee database PK
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Employee's Name
        /// </summary>
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [Display(Name = "Imię")]
        [StringLength(30)]
        [RegularExpression(@"[A-Za-zżźćńółęąśŻŹĆĄŚĘŁÓŃ\-\.\'\s]*", ErrorMessage = "{0} nie może zawierać cyfr")]
        public string Name { get; set; }
        /// <summary>
        /// Employee's Surname
        /// </summary>
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [Display(Name = "Nazwisko")]
        [StringLength(40)]
        [RegularExpression(@"[A-Za-zżźćńółęąśŻŹĆĄŚĘŁÓŃ\-\.\'\s]*", ErrorMessage = "{0} nie może zawierać cyfr")]
        public string Surname { get; set; }
        /// <summary>
        /// Universal Electronic System for Registration of the Population number
        /// </summary>
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [StringLength(11)]
        [RegularExpression(@"[0-9]{11}", ErrorMessage = "Nieprawidłowy format numeru PESEL - 11 cyfr")]
        public string Pesel { get; set; }
        /// <summary>
        /// Department's FK. Can be null.
        /// </summary>
        [Display(Name = "Stanowisko")]
        public int? DepartmentId { get; set; }
        /// <summary>
        /// Department's Name
        /// </summary>
        [Display(Name = "Stanowisko")]
        public Department Department { get; set; }
        /// <summary>
        /// Employee's address
        /// </summary>
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [StringLength(40)]
        [Display(Name = "Adres")]
        public string Street { get; set; }
        /// <summary>
        /// Postal Code
        /// </summary>
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [StringLength(6)]
        [Display(Name = "Kod pocztowy")]
        [RegularExpression(@"[0-9]{2}-[0-9]{3}", ErrorMessage = "Nieprawidłowy format kodu pocztowego xx-xxx")]
        public string ZipCode { get; set; }
        /// <summary>
        /// City of residence
        /// </summary>
        [Required(ErrorMessage = CustomErrorMessages.FieldIsRequired)]
        [StringLength(30)]
        [Display(Name = "Miasto")]
        public string City { get; set; }
        /// <summary>
        /// Employee's item list
        /// </summary>
        public IList<Item> Items { get; set; }
        /// <summary>
        /// Employee's full name 
        /// </summary>
        public string FullName => $"{Surname} {Name}";

        
    }
}
