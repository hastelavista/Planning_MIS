
using System.ComponentModel.DataAnnotations;


namespace Domain.Entities.Setup
{
    public class Norms
    {
        public int Id { get; set; }


        [Display(Name = "[[[Activity]]]")]
        public int ActivityId { get; set; }
        public string Activity { get; set; }


        [Display(Name = "[[[Activity Group]]]")]
        public int ActivityGroupId { get; set; }
        public string ActivityGroup { get; set; }


        [Display(Name = "[[[Item]]]")]
        public int ItemId { get; set; }
        public string Item { get; set; }


        [Display(Name = "[[[Item Type]]]")]
        public int ItemTypeId { get; set; }
        public string ItemType { get; set; }
        public string Unit { get; set; }
        public int Qty { get; set; }
        public decimal Rate { get; set; }
        public int FiscalYearId { get; set; }
        public int ActivityRateId { get; set; }


        public int CurrentUser { get; set; }

    }
}
