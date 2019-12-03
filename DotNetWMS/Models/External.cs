using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Models
{
    public class External
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string TaxId { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
    }
}
