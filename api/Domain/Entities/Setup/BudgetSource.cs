using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Setup
{
    public class BudgetSource
    {
        public int Id { get; set; }
        [Display(Name = "[[[Level]]]")]
        public int BudgetSourceLevelId { get; set; }
        public string BudgetSourceLevelName { get; set; }

        [Required, Display(Name = "[[[Name]]]")]
        public string Name { get; set; }

        [Required, Display(Name = "[[[Code Number]]]")]
        public string CodeNo { get; set; }
        public int UserId { get; set; }

        public decimal Budget { get; set; }
        public string DisplayName
        {
            get
            {
                return Name + " [बजेटः " + Budget.ToString("n") + "]";
            }
        }
    }
}
