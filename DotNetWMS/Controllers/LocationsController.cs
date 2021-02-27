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

        public async Task AddCoordinates(string longitude, string latitude)
        {
            if (!string.IsNullOrEmpty(longitude) && !string.IsNullOrEmpty(latitude))
            {
                Location location = new Location()
                {
                    Longitude = longitude,
                    Latitude = latitude
                };

                _context.Add(location);
                await _context.SaveChangesAsync();
            }

        }
        public async Task UpdateCoordinates(int? id, string longitude, string latitude)
        {
            if (id != null)
            {
                var location = _context.Locations.Find(id);
                location.Longitude = longitude;
                location.Latitude = latitude;
                _context.Update(location);
                await _context.SaveChangesAsync();
            }
        }
    }
}
