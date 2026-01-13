using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Project
{
    public class ProjectClosing
    {
        public List<ClosingProject> ClosingProject { get; set; }
        public int SourceFyId { get; set; }
        public int TargetFyId { get; set; }
    }

    public class ClosingProject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int WardId { get; set; }
        public string WardNo { get; set; }
        public decimal EstimatedBudget { get; set; }
        public decimal EstimatedCost { get; set; }
        public bool IsSelected { get; set; }
    }
}
