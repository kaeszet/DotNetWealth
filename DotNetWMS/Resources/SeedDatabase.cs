using DotNetWMS.Data;
using DotNetWMS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetWMS.Resources
{
    public static class SeedDatabase
    {
        public static async Task InitializeUsers(DotNetWMSContext context)
        {

            if (context.Users.Count() < 10)
            {
                await SeedUsers(context);
                await AddSeedUsersToRole(context);
            }

        }

        public static async Task InitializeData(DotNetWMSContext context)
        {

            if (context.Departments.Any() || context.Locations.Any() || context.Items.Any() || context.Warehouses.Any() || context.Externals.Any())
            {
                return;
            }

            await SeedData(context);

        }

        private static async Task SeedData(DotNetWMSContext context)
        {

            var departments = new[]
            {
                new Department { Name = "Sprzedawca"},
                new Department { Name = "Kierownik"},
                new Department { Name = "Magazynier"},
                new Department { Name = "Księgowy"},
                new Department { Name = "Właściciel"},
                new Department { Name = "Informatyk"},
                new Department { Name = "Konserwator"},
                new Department { Name = "Spec. ds. Kadr"},
                new Department { Name = "St. Magazynier"},
                new Department { Name = "Dyrektor"}
            };

            var locations = new[]
            {
                new Location { Address = "Myśliwska 61, 30-718 Kraków", Latitude = "50.046782", Longitude = "19.997600" },
                new Location { Address = "Cystersów 19, 31-553 Kraków", Latitude = "50.064216", Longitude = "19.969936" },
                new Location { Address = "Półłanki 30, 30-721 Kraków", Latitude = "50.029647", Longitude = "20.039791" },
                new Location { Address = "Lwowska 17, 35-301 Rzeszów", Latitude = "50.037976", Longitude = "22.026178" },
                new Location { Address = "Przewóz 34, 30-721 Kraków", Latitude = "50.042612", Longitude = "19.993209" },

            };

            var items = new[]
            {
                new Item { Type = "Elektronika", Name = "Komputer", Producer = "Dell", Model = "Optiplex 970", ItemCode = "TMP", WarrantyDate = new DateTime(2021,7,30,0,0,0), Quantity = 1.0M, Units = ItemUnits.szt, State = ItemState.InEmployee},
                new Item { Type = "Elektronika", Name = "Komputer", Producer = "Dell", Model = "Optiplex 970", ItemCode = "TMP", WarrantyDate = new DateTime(2021,1,30,0,0,0), Quantity = 1.0M, Units = ItemUnits.szt, State = ItemState.InEmployee},
                new Item { Type = "Elektronika", Name = "Komputer", Producer = "Dell", Model = "Optiplex 970", ItemCode = "TMP", WarrantyDate = new DateTime(2021,2,28,0,0,0), Quantity = 1.0M, Units = ItemUnits.szt, State = ItemState.InEmployee},
                new Item { Type = "Elektronika", Name = "Laptop", Producer = "Lenovo", Model = "Legion 7", ItemCode = "TMP", WarrantyDate = new DateTime(2021,6,30,0,0,0), Quantity = 1.0M, Units = ItemUnits.szt, State = ItemState.InEmployee},
                new Item { Type = "Elektronika", Name = "Laptop", Producer = "Lenovo", Model = "Legion 7", ItemCode = "TMP", WarrantyDate = new DateTime(2021,6,30,0,0,0), Quantity = 1.0M, Units = ItemUnits.szt, State = ItemState.InEmployee},
                new Item { Type = "Narzędzia", Name = "Wiertarka", Producer = "Parkside", Model = "PSBM 750", ItemCode = "TMP", Quantity = 5.0M, Units = ItemUnits.szt, State = ItemState.InWarehouse},
                new Item { Type = "Pojazdy", Name = "Sam. służbowy", Producer = "Fiat", Model = "Panda", ItemCode = "TMP", Quantity = 1.0M, Units = ItemUnits.szt, State = ItemState.InRepair},
                new Item { Type = "Papiercznicze", Name = "Ryza papieru", Producer = "International Paper", Model = "Speed", ItemCode = "TMP", Quantity = 100.0M, Units = ItemUnits.kpl, State = ItemState.New },
                new Item { Type = "Budowlane", Name = "Kostka brukowa", Producer = "Bruk-Bet", Model = "Holland", ItemCode = "TMP", Quantity = 10.0M, Units = ItemUnits.pal, State = ItemState.InWarehouse},
                new Item { Type = "Budowlane", Name = "Przewód miedziany", Producer = "Mors", Model = "", ItemCode = "TMP", Quantity = 200.0M, Units = ItemUnits.m, State = ItemState.InWarehouse}
            };



            var warehouses = new[]
            {
                new Warehouse { Name = "Siedziba firmy", Street = "Myśliwska 61", ZipCode="30-721", City = "Kraków"},
                new Warehouse { Name = "Magazyn główny", Street = "Myśliwska 61", ZipCode="30-721", City = "Kraków"},
                new Warehouse { Name = "Filia - Rzeszów", Street = "Lwowska 17", ZipCode="35-301", City = "Rzeszów"},
                new Warehouse { Name = "Kontener Maersk", Street = "Cystersów 19", ZipCode="31-553", City = "Kraków"},
                new Warehouse { Name = "Kontener Magazynowy", Street = "Półłanki 30", ZipCode="30-740", City = "Kraków"}
            };

            var externals = new[]
            {
                new External { Type = ContractorType.Wypożyczający, Name = "F.R.B. Budex", TaxId = "1112223344",Street="Przewóz 34", ZipCode="30-716", City = "Kraków"},
                new External { Type = ContractorType.Serwis, Name = "Serwis samochodowy", TaxId = "2223334455", Street="Przewóz 34B", ZipCode="30-716", City = "Kraków"},
                new External { Type = ContractorType.Sklep, Name = "X-Kom", TaxId = "9492107026", Street="Mariusza Bojemskiego 25", ZipCode="42-202", City = "Częstochowa"},
                new External { Type = ContractorType.Sklep, Name = "FIAT AUTO KRAK", TaxId = "6771173032", Street="Walerego Eljasza Radzikowskiego 160", ZipCode="31-342", City = "Kraków"},
                new External { Type = ContractorType.Podwykonawca, Name = "Brukarstwo M-ska", TaxId = "6821757606", Street="Lubelska 113", ZipCode="32-120", City = "Nowe Brzesko"},

            };


            context.Departments.AddRange(departments);
            await context.SaveChangesAsync();
            context.Locations.AddRange(locations);
            await context.SaveChangesAsync();
            context.Items.AddRange(items);
            await context.SaveChangesAsync();
            context.Warehouses.AddRange(warehouses);
            await context.SaveChangesAsync();
            context.Externals.AddRange(externals);
            await context.SaveChangesAsync();

            var deps = context.Departments.Select(d => d).ToArray();
            var locs = context.Locations.Select(l => l).ToArray();
            var emps = context.Users.Select(u => u).ToArray();
            var itms = context.Items.Select(i => i).ToArray();
            var wrhs = context.Warehouses.Select(w => w).ToArray();
            var exts = context.Externals.Select(e => e).ToArray();

            emps[0].DepartmentId = deps[6].Id;
            emps[1].DepartmentId = deps[7].Id;
            emps[2].DepartmentId = deps[5].Id;
            emps[3].DepartmentId = deps[0].Id;
            emps[4].DepartmentId = deps[2].Id;
            emps[5].DepartmentId = deps[3].Id;
            emps[6].DepartmentId = deps[4].Id;
            emps[7].DepartmentId = deps[0].Id;
            emps[8].DepartmentId = deps[0].Id;
            emps[9].DepartmentId = deps[2].Id;

            itms[0].UserId = emps[1].Id;
            itms[0].ItemCode = ItemCodeGenerator.Generate(itms[0], emps[1].UserName);
            itms[1].UserId = emps[3].Id;
            itms[1].ItemCode = ItemCodeGenerator.Generate(itms[1], emps[3].UserName);
            itms[2].UserId = emps[5].Id;
            itms[2].ItemCode = ItemCodeGenerator.Generate(itms[2], emps[5].UserName);
            itms[3].UserId = emps[2].Id;
            itms[3].ItemCode = ItemCodeGenerator.Generate(itms[3], emps[2].UserName);
            itms[4].UserId = emps[6].Id;
            itms[4].ItemCode = ItemCodeGenerator.Generate(itms[4], emps[6].UserName);
            itms[5].WarehouseId = wrhs[0].Id;
            itms[6].UserId = emps[3].Id;
            itms[6].ExternalId = exts[1].Id;
            itms[6].ItemCode = ItemCodeGenerator.Generate(itms[6], emps[3].UserName);
            itms[8].WarehouseId = wrhs[1].Id;
            itms[9].WarehouseId = wrhs[1].Id;

            for (int i = 0; i < items.Length; i++)
            {
                if (string.IsNullOrEmpty(itms[i].ItemCode))
                {
                    itms[i].ItemCode = ItemCodeGenerator.Generate(itms[i], emps[Convert.ToInt32(itms[i].UserId)].UserName);
                }

            }

            wrhs[0].LocationId = locs[0].Id;
            wrhs[1].LocationId = locs[0].Id;
            wrhs[2].LocationId = locs[3].Id;
            wrhs[3].LocationId = locs[1].Id;
            wrhs[4].LocationId = locs[2].Id;


            context.Users.UpdateRange(emps);
            await context.SaveChangesAsync();
            context.Items.UpdateRange(itms);
            await context.SaveChangesAsync();
            context.Warehouses.UpdateRange(wrhs);
            await context.SaveChangesAsync();

        }

        private static async Task SeedUsers(DotNetWMSContext context)
        {

            var users = new[]
            {
                new WMSIdentityUser { Name = "Adam", Surname = "Nowy", EmployeeNumber = "12345670000", Street = "Myśliwska 61", ZipCode="30-721", City="Kraków", Email="a.nowy@firma.pl", EmailConfirmed=true},
                new WMSIdentityUser { Name = "Andrzej", Surname = "Kadrowy", EmployeeNumber = "12345670001", Street = "Myśliwska 61", ZipCode="30-721", City="Kraków", Email="a.kadrowy@firma.pl", EmailConfirmed=true},
                new WMSIdentityUser { Name = "Bartłomiej", Surname = "Moderator", EmployeeNumber = "12345670002", Street = "Myśliwska 61", ZipCode="30-721", City="Kraków", Email="b.moderator@firma.pl", EmailConfirmed=true},
                new WMSIdentityUser { Name = "Bogdan", Surname = "Sprzedawca", EmployeeNumber = "12345670003", Street = "Myśliwska 61", ZipCode="30-721", City="Kraków", Email="b.sprzedawca@firma.pl", EmailConfirmed=true},
                new WMSIdentityUser { Name = "Bernard", Surname = "Magazynier", EmployeeNumber = "12345670004", Street = "Myśliwska 61", ZipCode="30-721", City="Kraków", Email="b.magazynier@firma.pl", EmailConfirmed=true},
                new WMSIdentityUser { Name = "Damian", Surname = "Kadrosprzedawca", EmployeeNumber = "12345670005", Street = "Lwowska 17", ZipCode="35-301", City="Rzeszów", Email="d.kadrosprzedawca@firma.pl", EmailConfirmed=true},
                new WMSIdentityUser { Name = "Eugeniusz", Surname = "Właściciel", EmployeeNumber = "12345670006", Street = "Lwowska 17", ZipCode="35-301", City="Rzeszów", Email="e.wlasciciel@firma.pl", EmailConfirmed=true},
                new WMSIdentityUser { Name = "Józef", Surname = "Nowy", EmployeeNumber = "12345670007", Street = "Lwowska 17", ZipCode="35-301", City="Rzeszów", Email="j.nowy@firma.pl"},
                new WMSIdentityUser { Name = "Janusz", Surname = "Nowy", EmployeeNumber = "12345670008", Street = "Lwowska 17", ZipCode="35-301", City="Rzeszów", Email="j.nowy2@firma.pl"},
                new WMSIdentityUser { Name = "Krzysztof", Surname = "Nowy", EmployeeNumber = "12345670009", Street = "Lwowska 17", ZipCode="35-301", City="Rzeszów", Email="k.nowy@firma.pl"}

            };

            for (int i = 0; i < users.Length; i++)
            {
                users[i].UserName = UserLoginGenerator.GenerateUserLogin(users[i].Name, users[i].Surname, users[i].EmployeeNumber);
                users[i].NormalizedUserName = users[i].UserName.ToUpper();
                users[i].NormalizedEmail = users[i].Email.ToUpper();

                if (!context.Users.Any(u => u.UserName == users[i].UserName))
                {
                    var password = new PasswordHasher<WMSIdentityUser>();
                    var hashed = password.HashPassword(users[i], "Test123!");
                    users[i].PasswordHash = hashed;
                    var userStore = new UserStore<WMSIdentityUser>(context);
                    await userStore.CreateAsync(users[i]);
                }

            }

            await context.SaveChangesAsync();
        }

        private static async Task AddSeedUsersToRole(DotNetWMSContext context)
        {
           
            var users = context.Users.Select(u => u).ToArray();

            for (int i = 0; i < users.Length; i++)
            {
                var userStore = new UserStore<WMSIdentityUser>(context);
                var isInStandardRole = await userStore.IsInRoleAsync(users[i], "STANDARD");

                if (!isInStandardRole)
                {
                    await userStore.AddToRoleAsync(users[i], "Standard");
                }
            }

            await context.SaveChangesAsync();
        }
    }
}
