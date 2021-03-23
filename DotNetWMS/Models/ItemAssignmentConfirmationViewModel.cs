using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Models
{
    public class ItemAssignmentConfirmationViewModel
    {
        [Display(Name = "Czy wygenerować dokument?")]
        public bool IsDocumentNeeded { get; set; }
        [Display(Name = "Wybierz użytkownika:")]
        public string UserId { get; set; }
        [Display(Name = "Wybierz magazyn:")]
        public int? WarehouseId { get; set; }
        [Display(Name = "Wybierz kontrahenta:")]
        public int? ExternalId { get; set; }
        public List<ItemsAssignmentViewModel> Items { get; set; }
    }
}
