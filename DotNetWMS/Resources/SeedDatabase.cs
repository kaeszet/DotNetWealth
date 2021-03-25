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
        public static async Task<bool> DeleteData(DotNetWMSContext context, string username)
        {
            try
            {
                var userStore = new UserStore<WMSIdentityUser>(context);
                //Delete FK from items and users
                var itemsArray = context.Items.ToArray();
                var empsArray = context.Users.ToArray();

                if (itemsArray.Length > 0)
                {
                    for (int i = 0; i < itemsArray.Length; i++)
                    {
                        itemsArray[i].ExternalId = null;
                        itemsArray[i].UserId = null;
                        itemsArray[i].WarehouseId = null;
                    }

                }

                if (empsArray.Length > 0)
                {
                    for (int i = 0; i < empsArray.Length; i++)
                    { 
                        var isInStandardRole = await userStore.IsInRoleAsync(empsArray[i], "Standard");
                        var isInStandardPlusRole = await userStore.IsInRoleAsync(empsArray[i], "StandardPlus");
                        var isInHRRole = await userStore.IsInRoleAsync(empsArray[i], "Kadry");
                        var isInModeratorRole = await userStore.IsInRoleAsync(empsArray[i], "Moderator");
                        var isInAdminRole = await userStore.IsInRoleAsync(empsArray[i], "Admin");

                        if (isInStandardRole)
                        {
                            await userStore.RemoveFromRoleAsync(empsArray[i], "Standard");
                        }
                        if (isInStandardPlusRole)
                        {
                            await userStore.RemoveFromRoleAsync(empsArray[i], "StandardPlus");
                        }
                        if (isInHRRole)
                        {
                            await userStore.RemoveFromRoleAsync(empsArray[i], "Kadry");
                        }
                        if (isInModeratorRole)
                        {
                            await userStore.RemoveFromRoleAsync(empsArray[i], "Moderator");
                        }
                        if (isInAdminRole && empsArray[i].Surname == "Właściciel")
                        {
                            await userStore.RemoveFromRoleAsync(empsArray[i], "Admin");
                        }

                        empsArray[i].DepartmentId = null;
                        empsArray[i].LocationId = null;
                    }
                }

                context.Items.UpdateRange(itemsArray);
                await context.SaveChangesAsync();
                context.Users.UpdateRange(empsArray);
                await context.SaveChangesAsync();

                //delete data from context
                var deps = context.Departments.Select(d => d).ToList();
                var locs = context.Locations.Select(l => l).ToList();
                var docs = context.Doc_Assignments.Select(l => l).ToList();
                var infs = context.Infoboxes.Select(l => l).ToList();
                var emps = context.Users.Select(u => u).Where(u => u.UserName != username).ToList();
                var itms = context.Items.Select(i => i).ToList();
                var wrhs = context.Warehouses.Select(w => w).ToList();
                var exts = context.Externals.Select(e => e).ToList();

                foreach (var emp in emps)
                {
                    if (await userStore.IsInRoleAsync(emp, "Admin"))
                    {
                        emps.Remove(emp);
                    }
                }

                context.RemoveRange(deps);
                context.RemoveRange(locs);
                context.RemoveRange(docs);
                context.RemoveRange(infs);
                context.RemoveRange(emps);
                context.RemoveRange(itms);
                context.RemoveRange(wrhs);
                context.RemoveRange(exts);
                await context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            
        }
        public static async Task<bool> InitializeUsers(DotNetWMSContext context)
        {

            if (context.Users.Count() < 10)
            {
                await SeedUsers(context);
                await AddSeedUsersToRole(context);
                return true;
            }
            return false;

        }

        public static async Task<bool> InitializeData(DotNetWMSContext context)
        {

            if (context.Departments.Any() || context.Locations.Any() || context.Items.Any() || context.Warehouses.Any() || context.Externals.Any())
            {
                return false;
            }

            await SeedData(context);
            return true;

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
                new Location { Address = "Myśliwska 61, 30-718 Kraków, Polska", Latitude = "50.046782", Longitude = "19.997600" },
                new Location { Address = "Cystersów 19, 31-553 Kraków, Polska", Latitude = "50.064216", Longitude = "19.969936" },
                new Location { Address = "Półłanki 30, 30-721 Kraków, Polska", Latitude = "50.029647", Longitude = "20.039791" },
                new Location { Address = "Lwowska 17, 35-301 Rzeszów, Polska", Latitude = "50.037976", Longitude = "22.026178" },
                new Location { Address = "Przewóz 34, 30-721 Kraków, Polska", Latitude = "50.042612", Longitude = "19.993209" },
                new Location { Address = "Mariusza Bojemskiego 25, Częstochowa, Polska", Latitude = "50.786742", Longitude = "19.186141" },
                new Location { Address = "Walerego Eljasza Radzikowskiego 160, 31-342 Kraków, Polska", Latitude = "50.088306", Longitude = "19.888248" },
                new Location { Address = "Lubelska 113, 32-120 Nowe Brzesko, Polska", Latitude = "50.137764", Longitude = "20.385744" }

            };

            var items = new[]
            {
                new Item { Type = "Elektronika", Name = "Komputer", Producer = "Dell", Model = "Optiplex 970", ItemCode = "TMP0", WarrantyDate = new DateTime(2021,7,30,0,0,0), Quantity = 1.0M, Units = ItemUnits.szt, State = ItemState.InEmployee},
                new Item { Type = "Elektronika", Name = "Komputer", Producer = "Dell", Model = "Optiplex 970", ItemCode = "TMP1", WarrantyDate = new DateTime(2021,1,30,0,0,0), Quantity = 1.0M, Units = ItemUnits.szt, State = ItemState.InEmployee},
                new Item { Type = "Elektronika", Name = "Komputer", Producer = "Dell", Model = "Optiplex 970", ItemCode = "TMP2", WarrantyDate = new DateTime(2021,2,28,0,0,0), Quantity = 1.0M, Units = ItemUnits.szt, State = ItemState.InEmployee},
                new Item { Type = "Elektronika", Name = "Laptop", Producer = "Lenovo", Model = "Legion 7", ItemCode = "TMP3", WarrantyDate = new DateTime(2021,6,30,0,0,0), Quantity = 1.0M, Units = ItemUnits.szt, State = ItemState.InEmployee},
                new Item { Type = "Elektronika", Name = "Laptop", Producer = "Lenovo", Model = "Legion 7", ItemCode = "TMP4", WarrantyDate = new DateTime(2021,6,30,0,0,0), Quantity = 1.0M, Units = ItemUnits.szt, State = ItemState.InEmployee},
                new Item { Type = "Narzędzia", Name = "Wiertarka", Producer = "Parkside", Model = "PSBM 750", ItemCode = "TMP5", Quantity = 5.0M, Units = ItemUnits.szt, State = ItemState.InWarehouse},
                new Item { Type = "Pojazdy", Name = "Sam. służbowy", Producer = "Fiat", Model = "Panda", ItemCode = "TMP6", Quantity = 1.0M, Units = ItemUnits.szt, State = ItemState.InRepair},
                new Item { Type = "Papiercznicze", Name = "Ryza papieru", Producer = "International Paper", Model = "Speed", ItemCode = "TMP7", Quantity = 100.0M, Units = ItemUnits.kpl, State = ItemState.New },
                new Item { Type = "Budowlane", Name = "Kostka brukowa", Producer = "Bruk-Bet", Model = "Holland", ItemCode = "TMP8", Quantity = 10.0M, Units = ItemUnits.pal, State = ItemState.InWarehouse},
                new Item { Type = "Budowlane", Name = "Przewód miedziany", Producer = "Mors", Model = "", ItemCode = "TMP9", Quantity = 200.0M, Units = ItemUnits.m, State = ItemState.InWarehouse}
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

            var deps = context.Departments.ToArray();
            var locs = context.Locations.ToArray();
            var emps = context.Users.ToArray();
            var itms = context.Items.ToArray();
            var wrhs = context.Warehouses.ToArray();
            var exts = context.Externals.ToArray();

            //emps[0].DepartmentId = deps[6].Id;
            //emps[1].DepartmentId = deps[7].Id;
            //emps[2].DepartmentId = deps[5].Id;
            //emps[3].DepartmentId = deps[0].Id;
            //emps[4].DepartmentId = deps[2].Id;
            //emps[5].DepartmentId = deps[3].Id;
            //emps[6].DepartmentId = deps[4].Id;
            //emps[7].DepartmentId = deps[0].Id;
            //emps[8].DepartmentId = deps[0].Id;
            //emps[9].DepartmentId = deps[2].Id;

            emps.FirstOrDefault(e => e.EmployeeNumber == "12345670000").DepartmentId = deps.FirstOrDefault(d => d.Name == "Konserwator")?.Id;
            emps.FirstOrDefault(e => e.EmployeeNumber == "12345670001").DepartmentId = deps.FirstOrDefault(d => d.Name == "Spec. ds. Kadr")?.Id;
            emps.FirstOrDefault(e => e.EmployeeNumber == "12345670002").DepartmentId = deps.FirstOrDefault(d => d.Name == "Informatyk")?.Id;
            emps.FirstOrDefault(e => e.EmployeeNumber == "12345670003").DepartmentId = deps.FirstOrDefault(d => d.Name == "Sprzedawca")?.Id;
            emps.FirstOrDefault(e => e.EmployeeNumber == "12345670004").DepartmentId = deps.FirstOrDefault(d => d.Name == "Magazynier")?.Id;
            emps.FirstOrDefault(e => e.EmployeeNumber == "12345670005").DepartmentId = deps.FirstOrDefault(d => d.Name == "Księgowy")?.Id;
            emps.FirstOrDefault(e => e.EmployeeNumber == "12345670006").DepartmentId = deps.FirstOrDefault(d => d.Name == "Właściciel")?.Id;
            emps.FirstOrDefault(e => e.EmployeeNumber == "12345670007").DepartmentId = deps.FirstOrDefault(d => d.Name == "Sprzedawca")?.Id;
            emps.FirstOrDefault(e => e.EmployeeNumber == "12345670008").DepartmentId = deps.FirstOrDefault(d => d.Name == "Sprzedawca")?.Id;
            emps.FirstOrDefault(e => e.EmployeeNumber == "12345670009").DepartmentId = deps.FirstOrDefault(d => d.Name == "Magazynier")?.Id;

            //emps[0].LocationId = locs[0].Id;
            //emps[1].LocationId = locs[0].Id;
            //emps[2].LocationId = locs[0].Id;
            //emps[3].LocationId = locs[0].Id;
            //emps[4].LocationId = locs[0].Id;
            //emps[5].LocationId = locs[2].Id;
            //emps[6].LocationId = locs[2].Id;
            //emps[7].LocationId = locs[2].Id;
            //emps[8].LocationId = locs[2].Id;
            //emps[9].LocationId = locs[2].Id;

            emps.FirstOrDefault(e => e.EmployeeNumber == "12345670000").LocationId = locs.FirstOrDefault(l => l.Address == "Myśliwska 61, 30-718 Kraków, Polska")?.Id;
            emps.FirstOrDefault(e => e.EmployeeNumber == "12345670001").LocationId = locs.FirstOrDefault(l => l.Address == "Myśliwska 61, 30-718 Kraków, Polska")?.Id;
            emps.FirstOrDefault(e => e.EmployeeNumber == "12345670002").LocationId = locs.FirstOrDefault(l => l.Address == "Myśliwska 61, 30-718 Kraków, Polska")?.Id;
            emps.FirstOrDefault(e => e.EmployeeNumber == "12345670003").LocationId = locs.FirstOrDefault(l => l.Address == "Myśliwska 61, 30-718 Kraków, Polska")?.Id;
            emps.FirstOrDefault(e => e.EmployeeNumber == "12345670004").LocationId = locs.FirstOrDefault(l => l.Address == "Myśliwska 61, 30-718 Kraków, Polska")?.Id;
            emps.FirstOrDefault(e => e.EmployeeNumber == "12345670005").LocationId = locs.FirstOrDefault(l => l.Address == "Lwowska 17, 35-301 Rzeszów, Polska")?.Id;
            emps.FirstOrDefault(e => e.EmployeeNumber == "12345670006").LocationId = locs.FirstOrDefault(l => l.Address == "Lwowska 17, 35-301 Rzeszów, Polska")?.Id;
            emps.FirstOrDefault(e => e.EmployeeNumber == "12345670007").LocationId = locs.FirstOrDefault(l => l.Address == "Lwowska 17, 35-301 Rzeszów, Polska")?.Id;
            emps.FirstOrDefault(e => e.EmployeeNumber == "12345670008").LocationId = locs.FirstOrDefault(l => l.Address == "Lwowska 17, 35-301 Rzeszów, Polska")?.Id;
            emps.FirstOrDefault(e => e.EmployeeNumber == "12345670009").LocationId = locs.FirstOrDefault(l => l.Address == "Lwowska 17, 35-301 Rzeszów, Polska")?.Id;

            //itms[0].UserId = emps[1].Id;
            //itms[0].ItemCode = ItemCodeGenerator.Generate(itms[0], emps[1].UserName);
            //itms[1].UserId = emps[3].Id;
            //itms[1].ItemCode = ItemCodeGenerator.Generate(itms[1], emps[3].UserName);
            //itms[2].UserId = emps[5].Id;
            //itms[2].ItemCode = ItemCodeGenerator.Generate(itms[2], emps[5].UserName);
            //itms[3].UserId = emps[2].Id;
            //itms[3].ItemCode = ItemCodeGenerator.Generate(itms[3], emps[2].UserName);
            //itms[4].UserId = emps[6].Id;
            //itms[4].ItemCode = ItemCodeGenerator.Generate(itms[4], emps[6].UserName);
            //itms[5].WarehouseId = wrhs[0].Id;
            //itms[6].UserId = emps[3].Id;
            //itms[6].ExternalId = exts[1].Id;
            //itms[6].ItemCode = ItemCodeGenerator.Generate(itms[6], emps[3].UserName);
            //itms[8].WarehouseId = wrhs[1].Id;
            //itms[9].WarehouseId = wrhs[1].Id;

            //blad
            itms.FirstOrDefault(i => i.ItemCode == "TMP0").UserId = emps.FirstOrDefault(e => e.EmployeeNumber == "12345670001")?.Id;
            itms.FirstOrDefault(i => i.ItemCode == "TMP0").ItemCode = ItemCodeGenerator.Generate(itms.FirstOrDefault(i => i.ItemCode == "TMP0"), emps.FirstOrDefault(e => e.EmployeeNumber == "12345670001")?.UserName);
            itms.FirstOrDefault(i => i.ItemCode == "TMP1").UserId = emps.FirstOrDefault(e => e.EmployeeNumber == "12345670003")?.Id;
            itms.FirstOrDefault(i => i.ItemCode == "TMP1").ItemCode = ItemCodeGenerator.Generate(itms.FirstOrDefault(i => i.ItemCode == "TMP1"), emps.FirstOrDefault(e => e.EmployeeNumber == "12345670003")?.UserName);
            itms.FirstOrDefault(i => i.ItemCode == "TMP2").UserId = emps.FirstOrDefault(e => e.EmployeeNumber == "12345670005")?.Id;
            itms.FirstOrDefault(i => i.ItemCode == "TMP2").ItemCode = ItemCodeGenerator.Generate(itms.FirstOrDefault(i => i.ItemCode == "TMP2"), emps.FirstOrDefault(e => e.EmployeeNumber == "12345670005")?.UserName);
            itms.FirstOrDefault(i => i.ItemCode == "TMP3").UserId = emps.FirstOrDefault(e => e.EmployeeNumber == "12345670002")?.Id;
            itms.FirstOrDefault(i => i.ItemCode == "TMP3").ItemCode = ItemCodeGenerator.Generate(itms.FirstOrDefault(i => i.ItemCode == "TMP3"), emps.FirstOrDefault(e => e.EmployeeNumber == "12345670002")?.UserName);
            itms.FirstOrDefault(i => i.ItemCode == "TMP4").UserId = emps.FirstOrDefault(e => e.EmployeeNumber == "12345670006")?.Id;
            itms.FirstOrDefault(i => i.ItemCode == "TMP4").ItemCode = ItemCodeGenerator.Generate(itms.FirstOrDefault(i => i.ItemCode == "TMP4"), emps.FirstOrDefault(e => e.EmployeeNumber == "12345670006")?.UserName);
            itms.FirstOrDefault(i => i.ItemCode == "TMP5").WarehouseId = wrhs.FirstOrDefault(w => w.Name == "Siedziba firmy")?.Id;
            itms.FirstOrDefault(i => i.ItemCode == "TMP6").UserId = emps.FirstOrDefault(e => e.EmployeeNumber == "12345670003")?.Id;
            itms.FirstOrDefault(i => i.ItemCode == "TMP6").ExternalId = exts.FirstOrDefault(e => e.TaxId == "6771173032")?.Id;
            itms.FirstOrDefault(i => i.ItemCode == "TMP6").ItemCode = ItemCodeGenerator.Generate(itms.FirstOrDefault(i => i.ItemCode == "TMP6"), emps.FirstOrDefault(e => e.EmployeeNumber == "12345670003")?.UserName);
            itms.FirstOrDefault(i => i.ItemCode == "TMP8").WarehouseId = wrhs.FirstOrDefault(e => e.Name == "Magazyn główny")?.Id;
            itms.FirstOrDefault(i => i.ItemCode == "TMP9").WarehouseId = wrhs.FirstOrDefault(e => e.Name == "Magazyn główny")?.Id;

            //for (int i = 0; i < itms.Length; i++)
            //{
            //    if (string.IsNullOrEmpty(itms[i].ItemCode))
            //    {
            //        itms[i].ItemCode = ItemCodeGenerator.Generate(itms[i], emps[Convert.ToInt32(itms[i].UserId)].UserName);
            //    }

            //}

            for (int i = 0; i < itms.Length; i++)
            {
                if (itms[i].ItemCode.Contains("TMP"))
                {
                    string empNumber = $"1234567000{i}";
                    itms[i].ItemCode = ItemCodeGenerator.Generate(itms[i], null);
                }
            }



            //wrhs[0].LocationId = locs[0].Id;
            //wrhs[1].LocationId = locs[0].Id;
            //wrhs[2].LocationId = locs[3].Id;
            //wrhs[3].LocationId = locs[1].Id;
            //wrhs[4].LocationId = locs[2].Id;

            wrhs.FirstOrDefault(w => w.Name == "Siedziba firmy").LocationId = locs.FirstOrDefault(l => l.Address == "Myśliwska 61, 30-718 Kraków, Polska")?.Id;
            wrhs.FirstOrDefault(w => w.Name == "Magazyn główny").LocationId = locs.FirstOrDefault(l => l.Address == "Myśliwska 61, 30-718 Kraków, Polska")?.Id;
            wrhs.FirstOrDefault(w => w.Name == "Filia - Rzeszów").LocationId = locs.FirstOrDefault(l => l.Address == "Lwowska 17, 35-301 Rzeszów, Polska")?.Id;
            wrhs.FirstOrDefault(w => w.Name == "Kontener Maersk").LocationId = locs.FirstOrDefault(l => l.Address == "Cystersów 19, 31-553 Kraków, Polska")?.Id;
            wrhs.FirstOrDefault(w => w.Name == "Kontener Magazynowy").LocationId = locs.FirstOrDefault(l => l.Address == "Półłanki 30, 30-721 Kraków, Polska")?.Id;

            //exts[0].LocationId = locs[4].Id;
            //exts[1].LocationId = locs[4].Id;
            //exts[2].LocationId = locs[5].Id;
            //exts[3].LocationId = locs[6].Id;
            //exts[4].LocationId = locs[7].Id;

            exts.FirstOrDefault(w => w.TaxId == "1112223344").LocationId = locs.FirstOrDefault(l => l.Address == "Przewóz 34, 30-721 Kraków, Polska")?.Id;
            exts.FirstOrDefault(w => w.TaxId == "2223334455").LocationId = locs.FirstOrDefault(l => l.Address == "Przewóz 34, 30-721 Kraków, Polska")?.Id;
            exts.FirstOrDefault(w => w.TaxId == "9492107026").LocationId = locs.FirstOrDefault(l => l.Address == "Mariusza Bojemskiego 25, Częstochowa, Polska")?.Id;
            exts.FirstOrDefault(w => w.TaxId == "6771173032").LocationId = locs.FirstOrDefault(l => l.Address == "Walerego Eljasza Radzikowskiego 160, 31-342 Kraków, Polska")?.Id;
            exts.FirstOrDefault(w => w.TaxId == "6821757606").LocationId = locs.FirstOrDefault(l => l.Address == "Lubelska 113, 32-120 Nowe Brzesko, Polska")?.Id;


            context.Users.UpdateRange(emps);
            //await context.SaveChangesAsync();
            context.Items.UpdateRange(itms);
            //await context.SaveChangesAsync();
            context.Warehouses.UpdateRange(wrhs);
            //await context.SaveChangesAsync();
            context.Externals.UpdateRange(exts);
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
           
            var users = context.Users.ToArray();

            for (int i = 0; i < users.Length; i++)
            {
                var userStore = new UserStore<WMSIdentityUser>(context);
                //var isInStandardRole = await userStore.IsInRoleAsync(users[i], "STANDARD");

                if (users[i].Surname == "Właściciel")
                {
                    await userStore.AddToRoleAsync(users[i], "ADMIN");
                }

                if (users[i].Surname == "Kadrowy" || users[i].Surname == "Kadrosprzedawca")
                {
                    await userStore.AddToRoleAsync(users[i], "KADRY");
                }

                if (users[i].Surname == "Sprzedawca" || users[i].Surname == "Kadrosprzedawca")
                {
                    await userStore.AddToRoleAsync(users[i], "STANDARDPLUS");
                }

                if (users[i].Surname == "Magazynier")
                {
                    await userStore.AddToRoleAsync(users[i], "STANDARD");
                }

                if (users[i].Surname == "Moderator")
                {
                    await userStore.AddToRoleAsync(users[i], "MODERATOR");
                }
            }

            await context.SaveChangesAsync();
        }
    }
}
