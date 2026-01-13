using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Setup
{
    public class Employee
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        [Display(Name = "[[[Name]]]")]
        public string Name { get; set; }

        [Display(Name = "[[[Address]]]")]
        public string Address { get; set; }

        [Display(Name = "[[[Mobile No]]]")]
        public string MobileNo { get; set; }

        [Display(Name = "[[[Department]]]")]
        public string Department { get; set; }

        [Display(Name = "[[[Department]]]")]
        public int DepartmentId { get; set; }

        [Display(Name = "[[[Designation]]]")]
        public string Designation { get; set; }

        [Display(Name = "[[[Designation]]]")]
        public int DesignationId { get; set; }

        [Display(Name = "[[[Is Technical Person]]]")]
        public bool IsTechnicalPerson { get; set; }

        [Display(Name = "[[[Is Ward Secratery]]]")]
        public bool IsWardSecratery { get; set; }

        [Display(Name = "लेखा हो?")]
        public bool IsAccountant { get; set; }
        
        
        public int CurrentUser { get; set; }
    }
}
