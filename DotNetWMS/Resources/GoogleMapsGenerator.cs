using DotNetWMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Resources
{
    public static class GoogleMapsGenerator
    {
        public static string PrepareAdressToGeoCode(Warehouse warehouse) => $"{warehouse.Street}, {warehouse.ZipCode} {warehouse.City}";

        public static string PrepareAdressToGeoCodeExternal(External external) => $"{external.Street}, {external.ZipCode} {external.City}";
    }
}
