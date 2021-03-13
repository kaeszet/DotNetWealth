using DotNetWMS.Data;
using DotNetWMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Resources
{
    /// <summary>
    /// A class contains methods used in Geolocation and Google Maps
    /// </summary>
    public class GoogleMapsGenerator
    {
        /// <summary>
        /// Method prepares <c>Warehouse</c>'s data to make it usable by Geolocation process
        /// </summary>
        /// <param name="warehouse"><c>Warehouse</c> object</param>
        /// <returns>Interpolated string with the necessary data</returns>
        public static string PrepareAdressToGeoCode(Warehouse warehouse) => $"{warehouse.Street}, {warehouse.ZipCode} {warehouse.City}";
        /// <summary>
        /// Method prepares <c>External</c>'s data to make it usable by Geolocation process
        /// </summary>
        /// <param name="external"><c>External</c> object</param>
        /// <returns>Interpolated string with the necessary data</returns>
        public static string PrepareAdressToGeoCodeExternal(External external) => $"{external.Street}, {external.ZipCode} {external.City}";
        /// <summary>
        /// Method prepares <c>WMSIdentityUser</c>'s data to make it usable by Geolocation process
        /// </summary>
        /// <param name="user"><c>WMSIdentityUser</c> object</param>
        /// <returns>Interpolated string with the necessary data</returns>
        public static string PrepareAdressToGeoCodeEmployee(WMSIdentityUser user) => $"{user.Street}, {user.ZipCode} {user.City}";
    }
}
