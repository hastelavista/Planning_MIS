
using System.ComponentModel.DataAnnotations;


namespace Domain.Entities.Setup
{
    public class ItemType
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Display(Name = "[[[Group]]]")]
        public int? ParentId { get; set; }
        public string Parent { get; set; }
        public string Code { get; set; }


        public int CurrentUser { get; set; }

    }
}
