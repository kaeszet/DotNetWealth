using DotNetWMS.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Resources
{
    /// <summary>
    /// The class is responsible for injecting the records into the created database. These records are used for the proper functionality of the application 
    /// </summary>
    public class DotNetWMSContextSeed
    {
        /// <summary>
        /// A field for handling the delivery of information to the DB associated with the Entity Core framework
        /// </summary>
        private readonly DotNetWMSContext _context;
        /// <summary>
        /// Implementation of the WMSIdentityUser class in the RoleManager class to support assigning and editing user-assigned roles
        /// </summary>
        private readonly RoleManager<IdentityRole> roleManager;

        public DotNetWMSContextSeed(DotNetWMSContext _context, RoleManager<IdentityRole> roleManager)
        {
            this._context = _context;
            this.roleManager = roleManager;
        }
        /// <summary>
        /// The method is responsible for injecting data about roles in the application
        /// </summary>
        public async void SeedRoles()
        {
            
            if (!_context.Roles.Any(r => r.Name == "admin"))
            {
                await roleManager.CreateAsync(new IdentityRole { Name = "admin", NormalizedName = "ADMIN" });
            }
            if (!_context.Roles.Any(r => r.Name == "moderator"))
            {
                await roleManager.CreateAsync(new IdentityRole { Name = "moderator", NormalizedName = "MODERATOR" });
            }
            if (!_context.Roles.Any(r => r.Name == "standardplus"))
            {
                await roleManager.CreateAsync(new IdentityRole { Name = "standardplus", NormalizedName = "STANDARDPLUS" });
            }
            if (!_context.Roles.Any(r => r.Name == "standard"))
            {
                await roleManager.CreateAsync(new IdentityRole { Name = "standard", NormalizedName = "STANDARD" });
            }
            if (!_context.Roles.Any(r => r.Name == "kadry"))
            {
                await roleManager.CreateAsync(new IdentityRole { Name = "kadry", NormalizedName = "KADRY" });
            }

            await _context.SaveChangesAsync();
        }
    }
    
}
