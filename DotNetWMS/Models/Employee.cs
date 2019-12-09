using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Pesel { get; set; }
        //foreign key
        public int? DepartmentId { get; set; }
        public Department Department { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public IList<Item> Items { get; set; }
        public string FullName => $"{Surname}, {Name}";

        
    }
}
