using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Models
{
    public enum ItemState
    {
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
        [Display(Name = "Inny")]
        Other,
        [Display(Name = "Nieznany")]
        Unknown
    }
}
