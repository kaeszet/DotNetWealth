using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Models
{
    public class StatusViewViewModel
    {
        [Display(Name = "Status")]
        public ItemState State { get; set; }
        public List<Item> Items { get; set; }
    }
}
