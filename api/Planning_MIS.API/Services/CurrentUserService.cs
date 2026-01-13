using Domain.Interfaces;
using System.Security.Claims;
using System.Text.Json;

namespace Planning_MIS.API.Services
{

    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ClaimsPrincipal User => _httpContextAccessor.HttpContext?.User;


        //public int UserId => 
        //    int.TryParse(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int id) ? id : -1;

        public int Id
        {
            get
            {
                if (User?.Identity?.IsAuthenticated == true)
                {
                    var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    return int.TryParse(claim, out int id) ? id : -1;
                }

                // No authenticated user -> system
                return -1;
            }
        }

        public string Username => User?.FindFirst("Username")?.Value;   
        public int UserTypeId => 
               int.TryParse(User?.FindFirst("UserTypeId")?.Value, out int id) ? id : 0;
        public bool IsDepartmentUser => 
            bool.TryParse(User?.FindFirst("IsDepartmentUser")?.Value, out bool b) && b;
        public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;
        public int FiscalYearId =>
               int.TryParse(User?.FindFirst("FiscalYearId")?.Value, out int fy) ? fy : 0;

        public List<int> WardIds 
        { 
            get
            {
                var wardJson = User?.FindFirst("WardIds")?.Value;
                if (string.IsNullOrEmpty(wardJson))
                    return new List<int>();

                try
                {
                    // Deserialize JSON arr to List
                    return JsonSerializer.Deserialize<List<int>>(wardJson) ?? new List<int>();
                }
                catch (Exception)
                {
                    
                    return new List<int>();
                }
            }
        }

    }
}
