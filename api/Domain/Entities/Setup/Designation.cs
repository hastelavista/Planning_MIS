using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Setup
{
    public class Designation
    {
        public int Id { get; set; }

        [Required, Display(Name = "[[[Name]]]")]
        public String Name { get; set; }

        [Display(Name = "[[[Type]]]")]
        public string Type { get; set; }

        [Display(Name = "Display Order")]
        public int DisplayOrder { get; set; }
       
        
        public int CurrentUser { get; set; }
        
    }
}
