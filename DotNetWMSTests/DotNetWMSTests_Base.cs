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
            if (context.Departments.Any() || context.Locations.Any() || context.Items.Any() || context.Warehouses.Any() || context.Externals.Any() || context.Infoboxes.Any() || context.Doc_Assignments.Any() || context.Users.Any())
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

            var locs = new[]
            {
                new Location { Id = 1, Address = "Lipska 49, 30-716 Kraków, Polska", Latitude = "50.0405946", Longitude = "19.9997631"},
                new Location { Id = 2, Address = "Św. Filipa 17, 31-150 Kraków, Polska", Latitude = "50.068105", Longitude = "19.940981"},
                new Location { Id = 3, Address = "Myśliwska 61, 30-718 Kraków, Polska", Latitude = "50.046782", Longitude = "19.997600" }
            };

            var items = new[]
            {
                new Item { Id = 1, Type = "Elektronika", Name = "Komputer", Producer = "CBM", Model = "Commodore 64", ItemCode="C-64", Quantity = 3.0M, Units = ItemUnits.szt, State = ItemState.InEmployee, UserId = "1" },
                new Item { Id = 2, Type = "Elektronika", Name = "Laptop", Producer = "Hykker", Model = "Hello", ItemCode="H-H", Quantity = 10.0M, Units = ItemUnits.szt, State = ItemState.InEmployee, UserId = "2" },
                new Item { Id = 3, Type = "Elektronika", Name = "Laptop", Producer = "Acer", Model = "Swift", ItemCode="A-S", Quantity = 1.0M, Units = ItemUnits.szt, State = ItemState.InRepair, ExternalId = 2 }
            };

            var warehouses = new[]
            {
                new Warehouse { Id = 1, Name = "Magazyn testowy", Street = "św. Filipa 17", ZipCode = "31-150", City = "Kraków", LocationId = 2 }
            };

            var externals = new[]
            {
                new External { Id = 1, Type = ContractorType.Wypożyczający, Name = "Kamil", TaxId = "1112223344", Street = "św. Filipa 17", ZipCode = "31-150", City = "Kraków", LocationId = 2 },
                new External { Id = 2, Type = ContractorType.Serwis,Name = "Krzysztof", TaxId = "2223334455", Street = "św. Filipa 17", ZipCode = "31-150", City = "Kraków", LocationId = 2 },
                new External { Id = 3, Type = ContractorType.Sklep, Name = "Kamil", TaxId = "3334445566", Street = "św. Filipa 17", ZipCode = "31-150", City = "Kraków", LocationId = 2 }

            };

            var docs = new[]
            {
                new Doc_Assignment { DocumentId = "P/2021/1/01/00001/11111111", Title = "P", CreationDate = new DateTime(2021, 1, 1), UserTo = "1"},
                new Doc_Assignment { DocumentId = "P/2021/1/01/00001/ToDelete", Title = "P", CreationDate = new DateTime(2021, 1, 1), UserTo = "1"},
                new Doc_Assignment { DocumentId = "P/2021/1/01/00001/ToCheck", Title = "P", CreationDate = new DateTime(2021, 1, 1), UserTo = "1"}
            };

            var users = new[]
            {
                new WMSIdentityUser { Id = "1", Name = "Janusz", Surname = "Testowy", UserName = "TestoJan9012", NormalizedUserName = "TESTOJAN9012" , EmployeeNumber = "23456789012", Street = "Myśliwska 61", ZipCode = "30-721", City="Kraków", Email="a@a.pl", LoginCount = 0, LocationId = 3, Location = locs[2]}
            };

            var infos = new[]
            {
                new Infobox { Id = 1, Title = "Test", Message = "Test", User = users[0], IsChecked = false },
                new Infobox { Id = 2, Title = "ToDelete", Message = "ToDelete", User = users[0], IsChecked = false },
                new Infobox { Id = 3, Title = "ToDeleteChecked", Message = "ToDeleteChecked", IsChecked = true },
                new Infobox { Id = 4, Title = "ToDeleteChecked", Message = "ToDeleteChecked", User = users[0], IsChecked = true },
                new Infobox { Id = 5, Title = "ToCheck", Message = "ToCheck", IsChecked = true },
                new Infobox { Id = 6, Title = "ToCheckWithDoc", Message = "ToCheckWithDoc", User = users[0], IsChecked = false, DocumentId = "P/2021/1/01/00001/ToCheck" }
            };

            context.Departments.AddRange(departments);
            context.Items.AddRange(items);
            context.Warehouses.AddRange(warehouses);
            context.Externals.AddRange(externals);
            context.Infoboxes.AddRange(infos);
            context.Locations.AddRange(locs);
            context.Doc_Assignments.AddRange(docs);
            context.Users.AddRange(users);
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
