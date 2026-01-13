using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Setup
{
    public class BudgetAllocation
    {
        public int Id { get; set; }
        public int FiscalYearId { get; set; }
        public string FiscalYear { get; set; }
        [Display(Name = "[[[Activities Project]]]")]
        [Required]
        public string Name { get; set; }
        [Display(Name = "रकम")]
        public decimal Amount { get; set; }
        public decimal AssignedBudget { get; set; }
        public decimal Balance { get; set; }

        [Display(Name = "वडा")]
        public int WardId { get; set; }
        public string WardName { get; set; }

        public string DisplayName
        {
            get
            {
                return (ParentId > 0 ? ParentName + " > " : "") + Name + " " + "[बजेटः " + Balance.ToString("n") + "]";
            }
        }
        [Display(Name = "[[[Activities Project]]]")]
        public int? ParentId { get; set; }
        public string ParentName { get; set; }

        public string CreatedBy { get; set; }
    }
}
