using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Setup
{
    public class Checklist
    {
        public int Id { get; set; }
        [Required, Display(Name = "[[[Name]]]")]
        public string Name { get; set; }
        [Required, Display(Name = "[[[Type]]]")]
        public int Type { get; set; }
        public bool IsForAgreement { get; set; }


        public int CurrentUser { get; set; }

    }
}
