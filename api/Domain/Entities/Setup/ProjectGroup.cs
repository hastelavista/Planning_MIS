
using System.ComponentModel.DataAnnotations;


namespace Domain.Entities.Setup
{
    public class ProjectGroup
    {

        public int Id { get; set; }

        [Display(Name = "[[[Project Group]]]")]

        public int? ParentId { get; set; }
        public string ParentName { get; set; }

        [Display(Name = "[[[Sector]]]")]
        public int? SectorId { get; set; }
        public string SectorName { get; set; }

        [Required, Display(Name = "[[[Name]]]")]
        public string Name { get; set; }
        [Required, Display(Name = "[[[Code]]]")]
        public string Code { get; set; }
        public string DisplayName { get; set; }
        
        
        public int CurrentUser { get; set; }


    }
}
