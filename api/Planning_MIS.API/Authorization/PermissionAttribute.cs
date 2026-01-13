using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Planning_MIS.API.Services;

namespace Planning_MIS.API.Authorization
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public int PermissionId { get; }

        public PermissionRequirement(int permissionId)
        {
            PermissionId = permissionId;
        }
    }


    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IUserRepository _userRepo;
        private readonly ICurrentUserService _currentUser;

        public PermissionHandler(IUserRepository userRepo, ICurrentUserService currentUser)
        {
            _userRepo = userRepo;
            _currentUser = currentUser;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (!_currentUser.IsAuthenticated)
            {
                context.Fail();
            return;
            }

            bool allowed = await _userRepo.HasPermission(_currentUser.Id, requirement.PermissionId);

            if (allowed)
                context.Succeed(requirement);
            else
                context.Fail();
        }


        
    }

}
