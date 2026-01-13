using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Setup
{
    public class FiscalYear
    {
        public int Id { get; set; }
        [Display(Name = "नाम")]
        public string Name { get; set; }
        [Display(Name = "नाम अंग्रेजी")]
        public string Name_En { get; set; }
        [Display(Name = "कोड")]
        public int Code { get; set; }
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        [Display(Name = "देखाउने क्र म")]
        public int DisplayPosition { get; set; }
        [Display(Name = "मितिदेखि")]
        public string DateFrom { get; set; }
        [Display(Name = "मिति सम्म")]
        public string DateTo { get; set; }
        public DateTime DateFromEng { get; set; }
        public DateTime DateToEng { get; set; }
        public bool IsDeleted { get; set; }



        public int CurrentUser { get; set; }      


    }
}
