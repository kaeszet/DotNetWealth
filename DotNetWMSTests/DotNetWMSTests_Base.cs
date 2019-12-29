using DotNetWMS.Data;
using DotNetWMS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetWMSTests
{
    public class DotNetWMSTests_Base : IDisposable
    {
        protected readonly DotNetWMSContext _context;

        public DotNetWMSTests_Base()
        {
            var _options = new DbContextOptionsBuilder<DotNetWMSContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new DotNetWMSContext(_options);
            _context.Database.EnsureCreated();
            Initialize(_context);


        }
        public static void Initialize(DotNetWMSContext context)
        {
            if (context.Departments.Any())
            {
                return;
            }

            Seed(context);
        }

        private static void Seed(DotNetWMSContext context)
        {
            var deparments = new[]
            {
                new Department { Id = 1, Name = "Sprzedawca"},
                new Department { Id = 2, Name = "Kierownik"},
                new Department { Id = 3, Name = "Magazynier"},
                new Department { Id = 4, Name = "Księgowy"}

            };

            context.Departments.AddRange(deparments);
            context.SaveChanges();
        }
        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
            
        }
    }
}
