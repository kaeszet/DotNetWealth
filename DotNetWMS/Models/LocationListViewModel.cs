using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Models
{
    public class LocationListViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Adres")]
        public string Address { get; set; }
        [Display(Name = "Długość geogr.")]
        public string Longitude { get; set; }
        [Display(Name = "Szerokość geogr.")]
        public string Latitude { get; set; }
        [Display(Name = "Używany?")]
        public bool IsInUse { get; set; }
        public List<string> Records { get; set; }
    }
}
