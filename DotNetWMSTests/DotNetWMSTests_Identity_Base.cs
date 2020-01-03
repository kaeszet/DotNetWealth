using DotNetWMS.Data;
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
            //Initialize(_context);


        }
        public static void Initialize(DotNetWMSContext context)
        {
            if (context.Users.Any())
            {
                return;
            }

            Seed(context);
        }

        private static void Seed(DotNetWMSContext context)
        {
            var users = new[]
            {
                new WMSIdentityUser { Name = "Janusz", Surname = "Testowy", EmployeeNumber = "123456789012", City="Kraków", Email="a@a.pl"}

            };


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

