using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.User
{
    public class User
    {
        public int Id { get; set; }

        public string UserName { get; set; } = string.Empty;
        public string Salt { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public bool IsActive { get; set; }
        public bool IsDepartmentUser { get; set; }
        public int UserTypeId { get; set; }
        public string UserTypeName { get; set; }
        public List<int> WardId { get; set; }
        public int EmployeeId { get; set; }
        public int DepartmentId { get; set; }
        public bool IsStatic { get; set; }

        public int CurrentUser { get; set; }

    }
}
