using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Setup
{
    public class ItemRate
    {
        public int Id { get; set; }
        [Display(Name = "[[[Item]]]")]
        public int ItemId { get; set; }
        public string Item { get; set; }
        public string FiscalYear { get; set; }
        [Display(Name = "[[[Fiscal Year]]]")]
        public int FiscalYearId { get; set; }
        public decimal Rate { get; set; }


        public int CurrentUser { get; set; }
    }
}
