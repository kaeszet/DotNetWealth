using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Models
{
    /// <summary>
    /// Location model class used for creating DB with EFC and collecting infos about messages
    /// </summary>
    public class Location
    {
        /// <summary>
        /// Location database PK
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Location's address
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Location's longitude gets from Google Geolocation
        /// </summary>
        public string Longitude { get; set; }
        /// <summary>
        /// Location's latitude gets from Google Geolocation
        /// </summary>
        public string Latitude { get; set; }

    }
}
