using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Models
{
    /// <summary>
    /// Enum that stores item statuses
    /// </summary>
    public enum ItemState
    {
        [Display(Name = "Nieznany")]
        Unknown,
        [Display(Name = "Zamówiony")]
        Ordered,
        [Display(Name = "Nowy")]
        New,
        [Display(Name = "Uszkodzony")]
        Damaged,
        [Display(Name = "Naprawiony")]
        Repaired,
        [Display(Name = "Zwrócony")]
        Returned,
        [Display(Name = "W magazynie")]
        InWarehouse,
        [Display(Name = "U pracownika")]
        InEmployee,
        [Display(Name = "W naprawie")]
        InRepair,
        [Display(Name = "Wypożyczony")]
        InLoan,
        [Display(Name = "Po gwarancji")]
        OutOfWarranty,
        [Display(Name = "Inny")]
        Other,
        
    }
}
