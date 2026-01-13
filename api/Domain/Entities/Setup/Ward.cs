using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Domain.Entities.Setup
{
    public class Ward
    {
        public int Id { get; set; }
        [Display(Name = "नाम")]
        public string Name { get; set; }
        [Display(Name = "देखाउने क्रम")]
        public int Code { get; set; }
        [Display(Name = "कोड")]
        public int DisplayOrder { get; set; }
        public int CurrentUser { get; set; }

        public string SigningEmp1 { get; set; }
        public string SigningEmp2 { get; set; }
        public string SigningEmp3 { get; set; }

        public string SigningDesignation1 { get; set; }
        public string SigningDesignation2 { get; set; }
        public string SigningDesignation3 { get; set; }
    }
}
