using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Models
{
    public enum Doc_Titles
    {
        [Display(Name = "P - Potwierdzenie")]
        P,
        [Display(Name = "PZ - Przyjęcie zewnętrzne")]
        PZ,
        [Display(Name = "PW - Przyjęcie wewnętrzne")]
        PW,
        [Display(Name = "ZW - Zwrot wewnętrzny")]
        ZW,
        [Display(Name = "MM - Przesunięcie międzymagazynowe")]
        MM,
        [Display(Name = "WZ - Wydanie zewnętrzne")]
        WZ,
        [Display(Name = "RZ - Rozchód wewnętrzny")]
        RW,

    }
}
