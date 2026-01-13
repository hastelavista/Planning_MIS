using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Setup
{
    public class Beneficiary
    {
        public int Id { get; set; }
        [Required, Display(Name = "[[[Name]]]")]
        public string Name { get; set; }

        [Display(Name = "[[[Is Population]]]")]
        public bool IsForPopulation { get; set; }


        public int CurrentUser { get; set; }
    }
}
