using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Setup
{
    public class BudgetAllocationProject
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public decimal Amount { get; set; }
    }
}
