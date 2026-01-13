
using System.ComponentModel.DataAnnotations;


namespace Domain.Entities.User
{
    public class UserType
    {
        public int Id { get; set; }
        [Display(Name = "नाम")]
        public string Name { get; set; }
        [Display(Name = "विवरण")]
        public string Description { get; set; }
        [Display(Name = "अधिकारहरु")]
        public List<int> PermissionId { get; set; }
        public int CurrentUser { get; set; }
        [Display(Name = "सक्रिय")]
        public bool IsActive { get; set; }
    }
}
