using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Models
{
    public class ItemAssignmentConfirmationViewModel
    {
        public bool IsDocumentNeeded { get; set; }
        public string UserId { get; set; }
        public int? WarehouseId { get; set; }
        public int? ExternalId { get; set; }
        public List<ItemsAssignmentViewModel> Items { get; set; }
    }
}
