
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Setup
{
    public class PublicRepresentative
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Display(Name = "[[[Designation]]]")]
        public int DesignationId { get; set; }
        public string Designation { get; set; }

        [Display(Name = "[[[Ward]]]")]
        public int WardId { get; set; }
        public string Ward { get; set; }

        [Display(Name = "[[[Mobile No]]]")]
        public string MobileNo { get; set; }

        [Display(Name = "[[[Address]]]")]
        public string Address { get; set; }

        [Display(Name = "[[[Display Order]]]")]
        public int? DisplayOrder { get; set; }

        [Display(Name = "[[[Is Active]]]")]
        public bool IsActive { get; set; }


        public int CurrentUser { get; set; }
    }
}
