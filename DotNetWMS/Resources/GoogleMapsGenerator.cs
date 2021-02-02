﻿using DotNetWMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Resources
{
    public static class GoogleMapsGenerator
    {
        public static string PrepareAdressToGeoCode(Warehouse warehouse) => $"{warehouse.City}, {warehouse.Street}";

        public static string PrepareAdressToGeoCode(External external) => $"{external.City}, {external.Street}";
    }
}
