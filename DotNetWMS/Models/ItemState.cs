using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Models
{
    public enum ItemState
    {
        Ordered, New, Damaged, Repaired, Returned, InWarehouse, InEmployee, InRepair, InLoan, Other, Unknown
    }
}
