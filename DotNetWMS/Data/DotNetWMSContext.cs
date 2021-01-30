using DotNetWMS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Data
{
    /// <summary>
    /// <para>Class associated with the Entity Core framework, responsible for creating databases related to the appropriate classes in the Models directory.</para> 
    /// <para>The Data Annotattions method was used to pass the field requirements.</para>
    /// </summary>
    public class DotNetWMSContext : IdentityDbContext<WMSIdentityUser>
    {
        public DotNetWMSContext(DbContextOptions<DotNetWMSContext> options) : base(options)
        {

        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<External> Externals { get; set; }
        public DbSet<Infobox> Infoboxes { get; set; }

        /// <summary>
        /// The method is used to store the database creation settings
        /// </summary>
        /// <param name="modelBuilder">ModelBuilder class object</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //A code that prevents the deletion of data that are linked with foreign keys. 
            //It is necessary to remove dependencies before deletion.
            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

    }

}
