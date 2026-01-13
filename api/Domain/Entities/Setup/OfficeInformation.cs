
using System.ComponentModel.DataAnnotations;


namespace Domain.Entities.Setup
{
    public class OfficeInformation
    {
        public string Slogan { get; set; }
        public int Id { get; set; }
        [Display(Name = "मुख्य शिर्षक")]
        public string MainHeading { get; set; }
        [Display(Name = "सहायक शिर्षक १")]
        public string SubHeading1 { get; set; }
        [Display(Name = "सहायक शिर्षक २")]
        public string SubHeading2 { get; set; }
        [Display(Name = "सहायक शिर्षक ३")]
        public string SubHeading3 { get; set; }
        [Display(Name = "नाम")]
        public string Name { get; set; }
        [Display(Name = "ठेगाना १")]
        public string Address1 { get; set; }
        [Display(Name = "ठेगाना २")]
        public string Address2 { get; set; }
        [Display(Name = "फोन")]
        public string Phone { get; set; }
        [Display(Name = "ई मेल")]
        public string Email { get; set; }
        [Display(Name = "बेवसाइट")]
        public string Website { get; set; }
        public string Photo { get; set; }
        [Display(Name = "फुटर")]
        public string Footer { get; set; }


        public int CurrentUser { get; set; }
    }
}
