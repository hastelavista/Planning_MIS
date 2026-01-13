using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Setup
{
    public class ExpensesHead
    {
        public int Id { get; set; }

        [Required, Display(Name = "[[[Name]]]")]
        public string Name { get; set; }

        [Display(Name = "[[[Code]]]")]
        public string Code { get; set; }

        [Display(Name = "[[[Type]]]")]
        public int ExpensesTypeId { get; set; }

        [Display(Name = "[[[Type]]]")]
        public string ExpensesType { get; set; }
        
        
        public int CurrentUser { get; set; }

    }
}
