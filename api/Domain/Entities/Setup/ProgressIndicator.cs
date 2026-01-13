
using System.ComponentModel.DataAnnotations;


namespace Domain.Entities.Setup
{
    public class ProgressIndicator
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        [Display(Name = "[[[Unit]]]")]
        public int UnitId { get; set; }
        public string Unit { get; set; }
        public decimal MaxPhysicalGoal { get; set; }
        public decimal MaxFinancialGoal { get; set; }
        
        
        public int CurrentUser { get; set; }

    }
}
