using System.ComponentModel;

namespace Planning_MIS.API.ViewModels.User
{
    public class UserDetails
    {
        public int Id { get; set; }
        public string UserName { get; set; } 
        public string FullName { get; set; }
        public bool IsDepartmentUser { get; set; }
        public int UserTypeId { get; set; }
        public string UserTypeName { get; set; }
        public List<int> WardId { get; set; }
        public int EmployeeId { get; set; }
        public int DepartmentId { get; set; }

    }
    public class UserCreateDto
    {
        public string UserName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public bool IsDepartmentUser { get; set; }
        public int UserTypeId { get; set; }
        public List<int> WardId { get; set; } = new();
        public int EmployeeId { get; set; }
        public int DepartmentId { get; set; }
    }

    public class UserUpdateDto
    {
        public string FullName { get; set; } = string.Empty;
        public string? Password { get; set; } 
        public bool IsActive { get; set; }
        public bool IsDepartmentUser { get; set; }
        public int UserTypeId { get; set; }
        public List<int> WardId { get; set; } = new();
        public int EmployeeId { get; set; }
        public int DepartmentId { get; set; }
    }
}
