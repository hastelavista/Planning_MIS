using System.ComponentModel.DataAnnotations;


namespace Domain.Entities.Setup
{
    public class NormsType
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        [Display(Name = "[[[Authority Name]]]")]
        public string AuthorityName { get; set; }
        public string Year { get; set; }
        
        
        public int CurrentUser { get; set; }
    }
}
