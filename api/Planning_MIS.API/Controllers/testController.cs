using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Planning_MIS.API.Controllers
{
    [Authorize]
    public class TestController : ControllerBase
    {
        private readonly ICurrentUserService _currentUser;

        public TestController(ICurrentUserService currentUser)
        {
            _currentUser = currentUser;
        }

        [HttpGet("me")]
        public IActionResult AmIAuthorized()
        {
            return Ok(new
            {
                UserId = _currentUser.Id,
                Authenticated = _currentUser.IsAuthenticated
            });
        }
    }
}
