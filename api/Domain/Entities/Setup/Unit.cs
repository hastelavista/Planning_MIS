
using System.ComponentModel.DataAnnotations;


namespace Domain.Entities.Setup
{
    public class Unit
    {
        public int Id { get; set; }

        [Display(Name = "[[[Symbol]]]")]
        public string Name { get; set; }

        [Display(Name = "[[[Name]]]")]
        public string Description { get; set; }

        [Display(Name = "[[[Name Nepali]]]")]
        public string Name_Np { get; set; }
        public string Symbol { get; set; }

        [Display(Name = "[[[Unit Type]]]")]
        public int UnitTypeId { get; set; }
        public string UnitTypeName { get; set; }

        [Display(Name = "[[[Is Usable]]]")]
        public bool IsUsable { get; set; }



        public int CurrentUser { get; set; }
    }
}
