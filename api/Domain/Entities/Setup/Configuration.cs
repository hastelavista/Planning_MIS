using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Setup
{
    public class Configuration
    {

        public int Id { get; set; }
        [Display(Name = "नाम")]
        public string Name { get; set; }
        public string Code { get; set; }
        [Display(Name = "दर")]
        public decimal Value { get; set; }
        [Display(Name = "प्रकार")]
        public string Type { get; set; }
        [Display(Name = "लागत अनुमानमा प्रयोग")]
        public bool IsForEstimation { get; set; }
        [Display(Name = "भुक्तानीमा प्रयोग")]
        public bool IsForEvaluation { get; set; }
        
        
        public int CurrentUser { get; set; }

    }
}
