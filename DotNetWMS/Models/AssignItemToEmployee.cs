using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Models
{
    public class AssignItemToEmployee
    {
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public int ItemId { get; set; }
        public decimal ItemQuantity { get; set; }
        public ItemState ItemState { get; set; }
        public Item Item { get; set; }
    }
}
