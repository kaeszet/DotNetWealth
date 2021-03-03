using DotNetWMS.Data;
using DotNetWMS.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Controllers
{
    public class LocationsController : Controller
    {

        private readonly DotNetWMSContext _context;

        public LocationsController(DotNetWMSContext context)
        {
            _context = context;
        }

        public Location GetCoordinates(string id, string adress)
        {

            var locById = _context.Locations.Find(id);
            var locByAdress = _context.Locations.FirstOrDefault(l => l.Adress == adress);

            return locById ?? locByAdress;
        }
        public async Task AddCoordinates(string adress, string longitude, string latitude)
        {
            if (!string.IsNullOrEmpty(longitude) && !string.IsNullOrEmpty(latitude))
            {
                Location location = new Location()
                {
                    Adress = adress,
                    Longitude = longitude,
                    Latitude = latitude
                };

                _context.Add(location);
                await _context.SaveChangesAsync();
            }

        }
        public async Task UpdateCoordinates(string id, string adress, string longitude, string latitude)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var location = _context.Locations.Find(id);
                location.Adress = adress;
                location.Longitude = longitude;
                location.Latitude = latitude;
                _context.Update(location);
                await _context.SaveChangesAsync();
            }
        }
    }
}
