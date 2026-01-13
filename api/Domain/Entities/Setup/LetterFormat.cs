
using System.ComponentModel.DataAnnotations;


namespace Domain.Entities.Setup
{
    public class LetterFormat
    {
        public int Id { get; set; }
        [Display(Name = "चिठी प्रकार")]
        public int LetterTypeId { get; set; }

        public int? ImplementationModelId { get; set; }

        public string ImplementationModel { get; set; }

        public string LetterType { get; set; }
        [Display(Name = "नाम")]
        public string Name { get; set; }
        [Display(Name = "विषय")]
        public string Subject { get; set; }
        [Display(Name = "चिठीको नमुना")]
        public string LetterBody { get; set; }
    }
}
