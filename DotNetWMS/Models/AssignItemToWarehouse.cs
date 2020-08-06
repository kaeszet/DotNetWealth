using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Models
{
    /// <summary>
    /// Viewmodel to handle a view where an user assigns items to an warehouse
    /// </summary>
    public class AssignItemToWarehouse
    {
        /// <summary>
        /// Stores the warehouse ID in DB
        /// </summary>
        public int WarehouseId { get; set; }
        /// <summary>
        /// Warehouse DB data
        /// </summary>
        public Employee Warehouse { get; set; }
        /// <summary>
        /// Stores the item ID in DB
        /// </summary>
        public int ItemId { get; set; }
        /// <summary>
        /// Number of items
        /// </summary>
        public decimal ItemQuantity { get; set; }
        /// <summary>
        /// Enum to choose the condition of the item. Correlated with the foreign keys of the Employee, Warehouse and External classes for reporting purposes
        /// </summary>
        public ItemState ItemState { get; set; }
        /// <summary>
        /// Item DB data
        /// </summary>
        public Item Item { get; set; }
    }
}
