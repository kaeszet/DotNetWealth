using DotNetWMS.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Resources
{
    public class DotNetWMSContextSeed
    {
        private readonly DotNetWMSContext _context;
        private readonly RoleManager<IdentityRole> roleManager;

        public DotNetWMSContextSeed(DotNetWMSContext _context, RoleManager<IdentityRole> roleManager)
        {
            this._context = _context;
            this.roleManager = roleManager;
        }
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
