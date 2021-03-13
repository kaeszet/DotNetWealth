using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Models
{
    /// <summary>
    /// Viewmodel contains Location properties, info whether location is in use by other data or not and list of those data
    /// </summary>
    public class LocationListViewModel
    {
        /// <summary>
        /// Location ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Location's address
        /// </summary>
        [Display(Name = "Adres")]
        public string Address { get; set; }
        /// <summary>
        /// Location's longitude gets from Google Geolocation
        /// </summary>
        [Display(Name = "Długość geogr.")]
        public string Longitude { get; set; }
        /// <summary>
        /// Location's latitude gets from Google Geolocation
        /// </summary>
        [Display(Name = "Szerokość geogr.")]
        public string Latitude { get; set; }
        /// <summary>
        /// Refer if <c>Location</c> is in use or not
        /// </summary>
        [Display(Name = "Używany?")]
        public bool IsInUse { get; set; }
        /// <summary>
        /// List of objects names which using location
        /// </summary>
        public List<string> Records { get; set; }
    }
}
