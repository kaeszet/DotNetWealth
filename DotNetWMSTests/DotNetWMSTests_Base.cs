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
            if (context.Departments.Any() || context.Employees.Any())
            {
                return;
            }

            Seed(context);
        }

        private static void Seed(DotNetWMSContext context)
        {
            var departments = new[]
            {
                new Department { Id = 1, Name = "Sprzedawca"},
                new Department { Id = 2, Name = "Kierownik"},
                new Department { Id = 3, Name = "Magazynier"},
                new Department { Id = 4, Name = "Księgowy"}

            };
            var employees = new[]
            {
                new Employee { Id = 1, Name = "Janusz", Surname = "Testowy", City = "Kraków", DepartmentId = 1, Pesel = "12345678901", Street = "św. Filipa 17", ZipCode = "30-000"},
                new Employee { Id = 2, Name = "Grażyna", Surname = "Testowa", City = "Kraków", DepartmentId = 4, Pesel = "12345678902", Street = "św. Filipa 17", ZipCode = "30-000"},
                new Employee { Id = 3, Name = "Brajan", Surname = "Testowy", City = "Kraków", DepartmentId = 3, Pesel = "12345678903", Street = "św. Filipa 17", ZipCode = "30-000"}
            };

            context.Departments.AddRange(departments);
            context.Employees.AddRange(employees);
            context.SaveChanges();
        }
        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
            
        }
    }
}
