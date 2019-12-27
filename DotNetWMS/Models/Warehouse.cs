﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Models
{
    public class Warehouse
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }
        [StringLength(100)]
        [Display(Name = "Ulica")]
        public string Street { get; set; }
        [StringLength(6)]
        [Display(Name = "Kod pocztowy")]
        [RegularExpression(@"[0-9]{2}-[0-9]{3}", ErrorMessage = "Nieprawidłowy format kodu pocztowego xx-xxx")]
        public string ZipCode { get; set; }
        [StringLength(50)]
        [Display(Name = "Miasto")]
        public string City { get; set; }
        public IList<Item> Items { get; set; }

    }
}
