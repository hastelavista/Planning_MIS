using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Setup
{
    public class CommitteeType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Name_En { get; set; }
        public int Code { get; set; }
        public int CurrentUser { get; set; }
    }
}
