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
        public string PrepareAdressToGeoCode(Warehouse warehouse) => $"{warehouse.City}, {warehouse.Street}";

        public string PrepareAdressToGeoCode(External external) => $"{external.City}, {external.Street}";
    }
}
