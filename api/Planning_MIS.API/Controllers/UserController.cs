using DataAccess.UnitOfWork;
using Domain.Entities.User;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Planning_MIS.API.Services;
using Planning_MIS.API.ViewModels.User;
using System.Threading.Tasks;

namespace Planning_MIS.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _repo;
        private readonly ICurrentUserService _currentUser;


        public UserController(IUnitOfWork repo, ICurrentUserService currentUser)
        {
            _repo = repo;
            _currentUser = currentUser;
        }


        #region UserType


        #endregion


        #region User

        [HttpGet("list")]
        public async Task<IActionResult> GetUsers(int id = 0, string userName = "", string fullName = "")
        {
            var user = await _repo.Users.GetUser(id, userName, fullName);
            var result = user.Select(user => new UserDetails
            {
                Id = user.Id,
                UserName = user.UserName,
                FullName = user.FullName,
                UserTypeId = user.UserTypeId,
                DepartmentId = user.DepartmentId,
                IsDepartmentUser = user.IsDepartmentUser,
                UserTypeName = user.UserTypeName,
                WardId = user.WardId,
                EmployeeId = user.EmployeeId
            }).ToList();
            return Ok(result);
        }

        [HttpGet("get_by_username")]
        public async Task<IActionResult> GetUserByUsername(string userName)
        {
            var user = await _repo.Users.GetByUsernameAsync( userName);
            if (user == null)
                return NotFound($"User '{userName}' not found.");

            var result = new UserDetails
            {
                Id = user.Id,
                UserName = user.UserName,
                FullName = user.FullName,
                UserTypeId = user.UserTypeId,
                DepartmentId = user.DepartmentId,
                IsDepartmentUser = user.IsDepartmentUser,
                UserTypeName = user.UserTypeName,
                WardId = user.WardId,
                EmployeeId = user.EmployeeId
            };
            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateDto model)
        {
            var existingUser = await _repo.Users.GetByUsernameAsync(model.UserName);
            if (existingUser != null)
                return BadRequest("Username already exists.");

            var (hashedPassword, salt) = PasswordHelper.HashPassword(model.Password);

            var user = new User
            {
                UserName = model.UserName,
                FullName = model.FullName,
                Password = hashedPassword,
                Salt = salt,
                IsActive = model.IsActive,
                IsDepartmentUser = model.IsDepartmentUser,
                UserTypeId = model.UserTypeId,
                WardId = model.WardId,
                EmployeeId = model.EmployeeId,
                DepartmentId = model.DepartmentId,

                CurrentUser = _currentUser.Id
            };

            var result = await _repo.Users.SaveUser(user);
            return Ok(result);

        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateDto model)
        {
            var user = await _repo.Users.GetUserById(id); 
            if (user == null)
                return NotFound("User not found.");

            user.FullName = model.FullName;
            user.IsActive = model.IsActive;
            user.IsDepartmentUser = model.IsDepartmentUser;
            user.UserTypeId = model.UserTypeId;
            user.WardId = model.WardId;
            user.EmployeeId = model.EmployeeId;
            user.DepartmentId = model.DepartmentId;
            user.CurrentUser = _currentUser.Id;

            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                var (hashedPassword, salt) = PasswordHelper.HashPassword(model.Password);
                user.Password = hashedPassword;
                user.Salt = salt;
            }

            var result = await _repo.Users.SaveUser(user);           
            return Ok(result);
        }


        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _repo.Users.DeleteUser(id);
            return Ok(result);
        }
        #endregion


    }
}

