using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Setup
{
    public class Item
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        [Display(Name = "[[[Item Type]]]")]
        public int ItemTypeId { get; set; }
        public string ItemType { get; set; }
        public string Code { get; set; }
        [Display(Name = "[[[Unit]]]")]
        public int UnitId { get; set; }
        public string Unit { get; set; }
        public string Remarks { get; set; }
        public decimal Rate { get; set; }


        public int CurrentUser { get; set; }

    }
}
