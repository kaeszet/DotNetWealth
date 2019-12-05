using DotNetWMS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Data
{
    public class DotNetWMSContext : DbContext
    {
        public DotNetWMSContext(DbContextOptions<DotNetWMSContext> options) : base(options)
        {

        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<External> Externals { get; set; }
    }
}
