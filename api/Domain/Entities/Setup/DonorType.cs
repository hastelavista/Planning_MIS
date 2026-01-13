using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Setup
{
    public class DonorType
    {
        public int Id { get; set; }

        [Required, Display(Name = "[[[Name]]]")]
        public string Name { get; set; }

        [Required, Display(Name = "[[[Code]]]")]
        public string Code { get; set; }
    }
}
