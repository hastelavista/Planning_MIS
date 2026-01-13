using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Setup
{
    public class BudgetSubTitle
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int ExpensesTypeId { get; set; }
        public string ExpensesType { get; set; }
        public int UserId { get; set; }
    }
}
