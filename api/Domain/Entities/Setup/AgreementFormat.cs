using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Setup
{
    public class AgreementFormat
    {
        public int Id { get; set; }
        public int ImplementationModelId { get; set; }
        public string ImplementationModel { get; set; }
        [Display(Name = "नाम")]
        public string Name { get; set; }
        [Display(Name = "चिठीको नमुना")]
        public string Body { get; set; }

        public int CurrentUser { get; set; }
    }
}
