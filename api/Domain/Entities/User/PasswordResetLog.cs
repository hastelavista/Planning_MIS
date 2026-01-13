using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.User
{
    public class PasswordResetLog
    {
        public int UserId { get; set; }
        public string Token { get; set; }
        public string Browser { get; set; }
        public string Device { get; set; }
        public string IPAddress { get; set; }

        [NotMapped]
        public DateTime Date { get; set; }

        [NotMapped]
        public DateTime? UsedDate { get; set; }
    }
}
