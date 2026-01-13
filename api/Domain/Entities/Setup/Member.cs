using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Domain.Entities.Setup
{
    public class Member
    {
        public int Id { get; set; }

        [Required, Display(Name = "[[[Name]]]")]
        public string Name { get; set; }

        [Required, Display(Name = "[[[Gender]]]")]
        public char Gender { get; set; }
        [Required(ErrorMessage = "कृपया पद राख्नुहोला"), Display(Name = "[[[Designation]]]")]
        public int DesignationId { get; set; }
        public string Designation { get; set; }

        [Display(Name = "[[[Grand Father Name]]]")]
        public string GrandFatherName { get; set; }

        [Display(Name = "[[[Father Name]]]")]
        public string FatherName { get; set; }

        [Required, Display(Name = "[[[Address]]]")]
        public string Address { get; set; }

        [Required, Display(Name = "[[[Citizenship Number]]]")]
        public string CitizenshipNo { get; set; }

        [Required, Display(Name = "[[[Mobile No]]]")]
        public string MobileNo { get; set; }
        [Display(Name = "खातावाला हो?")]
        public bool IsAccountHolder { get; set; }

        [Display(Name = "अनुगमन समिति हो?")]
        public bool IsInspectionCommittee { get; set; }

        public int CommitteeId { get; set; }
        public int UpdatedCommitteeId { get; set; }
        public string CurrentActiveProject { get; set; }

        public string Committee { get; set; }

        public string Status { get; set; }



        public int CurrentUser { get; set; }
    }
}
