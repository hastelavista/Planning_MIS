using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Setup
{
    public class ImplementationModel
    {
        public int Id { get; set; }

        [Display(Name = "[[[Model]]]")]
        public int ProjectModelId { get; set; }
        public string ProjectModel { get; set; }
        public bool AddBidInformation { get; set; }
        public bool AddContractInformation { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int TotalProject { get; set; }
        public int OnGoingProject { get; set; }
        public int CompletedProject { get; set; }
       
        
        public int CurrentUser { get; set; }

    }
}
