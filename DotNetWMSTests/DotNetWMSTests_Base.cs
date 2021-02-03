using DotNetWMS.Data;
using DotNetWMS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
            if (context.Departments.Any() || context.Employees.Any() || context.Items.Any() || context.Externals.Any())
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
            var items = new[]
            {
                new Item { Id = 1, Type = "Elektronika", Name = "Komputer", Producer = "CBM", Model = "Commodore 64", ItemCode="C-64", Quantity = 3.0M, Units = ItemUnits.szt, State = ItemState.InEmployee, UserId = "1" },
                new Item { Id = 2, Type = "Elektronika", Name = "Laptop", Producer = "Hykker", Model = "Hello", ItemCode="H-H", Quantity = 10.0M, Units = ItemUnits.szt, State = ItemState.InEmployee, UserId = "2" }
            };

            var externals = new[]
            {
                new External { Id = 1, Type = ContractorType.Wypożyczający, Name = "Adam", TaxId = "1112223344",Street="św. Filipa 17", ZipCode="30-000", City = "Kraków"},
                new External { Id = 2, Type = ContractorType.Serwis,Name = "Krzysztof", TaxId = "2223334455",Street="św. Filipa 17", ZipCode="30-000", City = "Kraków"},
                new External { Id = 3, Type = ContractorType.Sklep, Name = "Kamil", TaxId = "3334445566",Street="św. Filipa 17", ZipCode="30-000", City = "Kraków"}

            };

            context.Departments.AddRange(departments);
            context.Employees.AddRange(employees);
            context.Items.AddRange(items);
            context.Externals.AddRange(externals);
            context.SaveChanges();
        }
        public bool TryValidate(object model, out ICollection<ValidationResult> results)
        {
            var context = new ValidationContext(model, serviceProvider: null, items: null);
            results = new List<ValidationResult>();
            return Validator.TryValidateObject(model, context, results, validateAllProperties: true);
        }
        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
            
        }
    }
}
