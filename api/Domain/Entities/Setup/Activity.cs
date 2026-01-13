using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Setup
{
    public class Activity
    {
        public int Id { get; set; }
        [Display(Name = "[[[Activity Group]]]")]
        public int ActivityGroupId { get; set; }
        public string ActivityGroupCode { get; set; }
        public string ActivityGroup { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        [Display(Name = "[[[Reference Code]]]")]
        public string ReferenceCode { get; set; }
        [Display(Name = "[[[Unit]]]")]
        public int UnitId { get; set; }
        public string Unit { get; set; }
        [Display(Name = "[[[Is Usable]]]")]
        public bool IsUsable { get; set; }
        [Display(Name = "[[[Norms Type]]]")]
        public string NormsType { get; set; }
        public int QtyFor { get; set; }
        public decimal Rate { get; set; }


        public int CurrentUser { get; set; }
    }


    public class ActivityGroup
    {
        public int Id { get; set; }

        [Display(Name = "[[[Group]]]")]
        public int? ParentId { get; set; }
        public string Parent { get; set; }
        public string ParentCode { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Code { get; set; }
        [Display(Name = "[[[Norms Type]]]")]
        public int? NormsTypeId { get; set; }
        public string NormsType { get; set; }
        public int CurrentUser { get; set; }
    }



    public class ActivityRate
    {
        public int Id { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal RatePerUnit { get; set; }
        public int FiscalYearId { get; set; }
        public string FiscalYear { get; set; }
    }

}
