using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Setup
{
    public class Department
    {
        public int Id { get; set; }

        [Display(Name = "शाखाको नाम")]
        public string Name { get; set; }

        [Display(Name = "उपनाम")]
        public string Alias { get; set; }

        public int CurrentUser { get; set; }
    }
}
