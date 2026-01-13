
using System.ComponentModel.DataAnnotations;


namespace Domain.Entities.Setup
{
    public class UnitType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [Display(Name = "[[[Name Nepali]]]")]
        public string Name_Np { get; set; }

        [Display(Name = "[[[Display Order]]]")]
        public int DisplayOrder { get; set; }

        [Display(Name = "[[[Is Usable]]]")]
        public bool IsUsable { get; set; }



        public int CurrentUser { get; set; }
    }
}
