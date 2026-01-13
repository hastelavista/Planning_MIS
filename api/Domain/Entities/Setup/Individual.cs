using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Setup
{
    public class Individual
    {

        public int Id { get; set; }

        [Display(Name = "अमानतको/निवेदनको नाम")]
        public string Name { get; set; }

        [Display(Name = "[[[Address]]]")]
        public string Address { get; set; }

        [Display(Name = "[[[Mobile No]]]")]
        public string MobileNo { get; set; }
        [Display(Name = "[[[Bank]]]")]
        public int? BankId { get; set; }
        public string Bank { get; set; }
        [Display(Name = "Account No")]
        public string AccountNo { get; set; }

        [Display(Name = "कर्मचारी हो ?")]
        public bool IsEmployee { get; set; }


        public int CurrentUser { get; set; }


    }
}
