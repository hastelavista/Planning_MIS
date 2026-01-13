
using System.ComponentModel.DataAnnotations;


namespace Domain.Entities.Setup
{
    public class SubSector
    {
        public int Id { get; set; }

        [Display(Name = "[[[Sector]]]")]
        public int SectorId { get; set; }
        public string Sector { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        [Display(Name = "[[[Is Usable]]]")]
        public bool IsUsable { get; set; }


        public int CurrentUser { get; set; }
    }
}
