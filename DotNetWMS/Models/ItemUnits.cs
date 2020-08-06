using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Models
{
    /// <summary>
    /// Enum that stores item units
    /// </summary>
    public enum ItemUnits
    {
        [Display(Name = "szt.")]
        szt,
        [Display(Name = "kg")]
        kg,
        [Display(Name = "m.")]
        m,
        [Display(Name = "pal.")]
        pal,
        [Display(Name = "kpl.")]
        kpl
    }
}
