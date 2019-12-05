using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Models
{
    public class AssignItemToWarehouse
    {
        public int WarehouseId { get; set; }
        public Employee Warehouse { get; set; }
        public int ItemId { get; set; }
        public decimal ItemQuantity { get; set; }
        public ItemState ItemState { get; set; }
        public Item Item { get; set; }
    }
}
