using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Setup
{
    public class Firm
    {
        public int Id { get; set; }

        [Display(Name = "प्रकार")]
        public int FirmTypeId { get; set; }
        public string FirmTypeName { get; set; }

        [Required, Display(Name = "[[[Name]]]")]
        public string Name { get; set; }

        [Required, Display(Name = "[[[Address]]]")]
        public string Address { get; set; }

        [Display(Name = "[[[Pan No]]]")]
        public string PanNo { get; set; }

        [Display(Name = "[[[Phone No]]]")]
        public string PhoneNo { get; set; }
        [Display(Name = "बैक")]
        public string Bank { get; set; }
        [Display(Name = "शाखा")]
        public string Branch { get; set; }
        [Display(Name = "खाता नं")]
        public string AccountNo { get; set; }

        [Required, Display(Name = "[[[Representative]]]")]
        public string Representative { get; set; }

        [Display(Name = "[[[Designation]]]")]
        public string RepresentativeDesignation { get; set; }
        [Display(Name = "प्रतिनिधिको ठेगाना")]
        public string RepresentativeAddress { get; set; }

        [Display(Name = "[[[Mobile No]]]")]
        public string MobileNo { get; set; }



        public int CurrentUser { get; set; }
    }
}
