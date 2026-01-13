using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Setup
{
    public class CommitteeLetter
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Miti { get; set; }
        public int CommitteeId { get; set; }
        public string CommitteeName { get; set; }
        public string RegistrationNo { get; set; }
        public int LetterTypeId { get; set; }
        public int LetterFormatId { get; set; }
        public string Subject { get; set; }

        [DataType(DataType.Html)]
        public string LetterBody { get; set; }

        public int CurrentUser { get; set; }

        public string LetterHeadOffice { get; set; }


        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string CreatedMiti { get; set; }
        
    }
}
