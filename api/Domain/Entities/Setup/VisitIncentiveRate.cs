using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel.GeneralSetup
{
    public class VisitIncentiveRate
    {
        public int Id { get; set; }
        [Display(Name = "[[[Fiscal Year]]]")]
        public int FiscalYearId { get; set; }
        public string FiscalYearName { get; set; }
        [Display(Name = "[[[Designation]]]")]
        public int DesignationId { get; set; }
        public string DesignationName { get; set; }
        public decimal Rate { get; set; }

        public int CurrentUser { get; set; }
    }
}
