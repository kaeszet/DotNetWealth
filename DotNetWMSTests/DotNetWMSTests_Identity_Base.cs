﻿using DotNetWMS.Data;
using DotNetWMS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace DotNetWMSTests
{
    public class DotNetWMSTests_Identity_Base : IDisposable
    {
        protected readonly UserManager<WMSIdentityUser> userManager;
        protected readonly SignInManager<WMSIdentityUser> signInManager;
        protected readonly UserStore<WMSIdentityUser> _store;
        protected readonly DotNetWMSContext _context;

        public DotNetWMSTests_Identity_Base()
        {
            var _options = new DbContextOptionsBuilder<DotNetWMSContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new DotNetWMSContext(_options);
            _store = new UserStore<WMSIdentityUser>(_context);
            _context.Database.EnsureCreated();
            Initialize(_context);


        }
        public static void Initialize(DotNetWMSContext context)
        {
            if (context.Users.Any() || context.Roles.Any())
            {
                return;
            }

            Seed(context);
        }

        private static void Seed(DotNetWMSContext context)
        {
            var users = new[]
            {
                new WMSIdentityUser { Id = "1", Name = "Janusz", Surname = "Testowy", UserName = "TestoJan9012", EmployeeNumber = "23456789012", City="Kraków", Email="a@a.pl", LoginCount = 0 },
                new WMSIdentityUser { Id = "2", Name = "Janusz", Surname = "Testowy2", UserName = "TestoJan9013", EmployeeNumber = "23456789013", City="Kraków", Email="a@a.pl", LoginCount = 0 }

            };

            var roles = new[]
            {
                new IdentityRole { Id = "1", Name = "Standard", NormalizedName = "STANDARD", ConcurrencyStamp = "test" },
                new IdentityRole { Id = "Admin", Name = "Admin", NormalizedName = "ADMIN", ConcurrencyStamp = "test2" },
                new IdentityRole { Id = "Kadry", Name = "Kadry", NormalizedName = "KADRY", ConcurrencyStamp = "test3" }
            };


            context.Users.AddRange(users);
            context.Roles.AddRange(roles);
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

