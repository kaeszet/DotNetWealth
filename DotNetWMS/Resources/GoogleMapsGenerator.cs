using DotNetWMS.Data;
using DotNetWMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Resources
{
    public  class GoogleMapsGenerator
    {
        public static string PrepareAdressToGeoCode(Warehouse warehouse) => $"{warehouse.Street}, {warehouse.ZipCode} {warehouse.City}";
        public static string PrepareAdressToGeoCodeExternal(External external) => $"{external.Street}, {external.ZipCode} {external.City}";
        public static string PrepareAdressToGeoCodeEmployee(WMSIdentityUser user) => $"{user.Street}, {user.ZipCode} {user.City}";
    }
}
