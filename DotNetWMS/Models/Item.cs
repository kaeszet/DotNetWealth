using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Producer { get; set; }
        public string Model { get; set; }
        public string ItemCode { get; set; }
        public decimal Quantity { get; set; }
        public DateTime WarrantyDate { get; set; }
        public ItemState State { get; set; }
        public int? EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public int? WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }
        public int? ExternalId { get; set; }
        public External External { get; set; }

    }
}
