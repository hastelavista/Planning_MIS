using Domain.ViewModel.GeneralSetup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Setup
{
    public class Committee
    {
        public int Id { get; set; }

        [Display(Name = "[[[Committee Type]]]")]
        public int CommitteeTypeId { get; set; }
        public string CommitteeType { get; set; }

        [Display(Name = "दर्ता नं")]
        public string RegistrationNo { get; set; }

        [Display(Name = "[[[Registered Date]]]")]
        public DateTime RegisteredDate { get; set; }

        [Required, Display(Name = "[[[Registered Miti]]]")]
        public string RegisteredMiti { get; set; }

        [Required, Display(Name = "[[[Name]]]")]
        public string Name { get; set; }

        [Display(Name = "[[[Ward No]]]")]
        public int WardId { get; set; }

        [Required, Display(Name = "[[[Address]]]")]
        public string Address { get; set; }

        [Display(Name = "[[[Bank]]]")]
        public int? BankId { get; set; }
        public string Bank { get; set; }

        [Display(Name = "[[[Branch]]]")]
        public string Branch { get; set; }

        [Display(Name = "खाता नं")]
        public string AccountNo { get; set; }

        [Display(Name = "बनाउने निकाय")]
        public string FormedBy { get; set; }

        [Display(Name = "उपस्थित संख्या")]
        public int TotalAttendedMember { get; set; }

        public int CurrentUser { get; set; }

        public List<Member> MemberDetail { get; set; }
        public bool IsAssigned { get; set; }

        public string CurrentProject { get; set; }
        public int CurrentProjectId { get; set; }
    }
}
