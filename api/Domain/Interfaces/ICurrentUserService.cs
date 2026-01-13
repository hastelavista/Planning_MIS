using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{

    
        public interface ICurrentUserService
        {
            int Id { get; }
            string Username { get; }
            int UserTypeId { get; }
            bool IsDepartmentUser { get; }
            bool IsAuthenticated { get; }
            int FiscalYearId { get; }

            List<int> WardIds { get; }

        }

}
