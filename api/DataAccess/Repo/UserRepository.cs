using Domain.Entities.User;
using Domain.Interfaces;
using Helper;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repo
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseContext _db;
        private readonly ICurrentUserService _currentUser;
        private readonly IMemoryCache _cache;


        public UserRepository(DatabaseContext db, IMemoryCache cache, ICurrentUserService currentUser)
        {
            _db = db;
            _cache = cache;
            _currentUser = currentUser;
        }
        
        #region UserType
        public async Task<List<UserType>> GetUserType(int id = 0, string name = "")
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                     .AddMore("@Name", name);
            var q = @"Select ut.* from UserType ut Where (@Id = 0 or ut.Id = @Id) 
		                                           And (@Name = '' or ut.Name Like '%' + @Name + '%')

                      Select Id, UserTypeId, PermissionId from UserTypePermission Where UserTypeId = @Id";
            var dt = await _db.ExecuteDataTableAsync(CommandType.Text, q, p);
            return dt.TransformToList<UserType>().ToList();
        }
        public async Task<DbResponse> SaveUserType(UserType model)
        {
            var p = model.PrepareSQLParameters();
            var q = QueryBuilder.GetCommandText["User.UserTypeSave"];
            var dbResponse = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return dbResponse;
        }
        public async Task<DbResponse> DeleteUserType(int id, int currentUser)
        {
            var p = _db.SqlParameters.AddMore("@Id", id);
            var q = @"Delete from UserType Where Id = @Id";
            var dbResponse = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return dbResponse;
        }


        

        #endregion


        #region User
        public async Task<User> GetByUsernameAsync(string username)
        {
            var p = _db.SqlParameters.AddMore("@username", username);
            var q = @"SELECT Id, FullName, UserName, Password, Salt, IsDepartmentUser, UserTypeId, WardId  
                     FROM dbo.[User] WHERE Username = @username AND IsActive = 1";

            var data = await _db.ExecuteDataTableAsync(CommandType.Text, q, p);
            return data.TransformToList<User>().FirstOrDefault();
        }
        public async Task<List<User>> GetUser(int id = 0, string userName = "", string fullName = "")
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                    .AddMore("@UserName", userName)
                                    .AddMore("@FullName", fullName);
            var q = QueryBuilder.GetCommandText["User.User.Get"];
            var dt = await _db.ExecuteDataTableAsync(CommandType.Text, q, p);
            return dt.TransformToList<User>().ToList();

        }
        public async Task<User?> GetUserById(int id)
        {
            var dt = await GetUser(id);
            return dt.FirstOrDefault();
        }
        public async Task<DbResponse> SaveUser(User model)
        {
            var p = model.PrepareSQLParameters();
            var q = QueryBuilder.GetCommandText["User.User.Save"];
            var dbResponse = await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return dbResponse;
        }
        public async Task<DbResponse> DeleteUser(int id)
        {
            var p = _db.SqlParameters.AddMore("@Id", id)
                                    .AddMore("@CurrentUser", _currentUser.Id);
            var q = @"Update [User] Set IsDeleted = 1, ModifiedDate = GETDATE(), ModifiedBy = @CurrentUser Where Id = @Id";
            return await _db.ExecuteNonQueryAsync(CommandType.Text, q, p);
        } 

        #endregion


        #region UserPermission
        
        public async Task<bool> HasPermission(int userId, int permissionId)
        {
            int userTypeId = await GetUserTypeId(userId);
            var permissions = await GetPermissionsForUserType(userTypeId);
            return permissions.Contains(permissionId);
        }

        public async Task<int> GetUserTypeId(int userId)
        {
            string cacheKey = $"user_usertype_{userId}";

            if (!_cache.TryGetValue(cacheKey, out int userTypeId))
            {
                var p = _db.SqlParameters.AddMore("@UserId", userId);
                var q = "SELECT UserTypeId FROM [User] WHERE Id = @UserId";

                var response = await _db.ExecuteScalarAsync(CommandType.Text, q, p);

                userTypeId = Convert.ToInt32(response.Response);

                _cache.Set(cacheKey, userTypeId, TimeSpan.FromMinutes(10));
            }

            return userTypeId;
        }

        public async Task<HashSet<int>> GetPermissionsForUserType(int userTypeId)
        {
            string cacheKey = $"permissions_usertype_{userTypeId}";

            if (!_cache.TryGetValue(cacheKey, out HashSet<int> permissions))
            {
                permissions = await LoadPermissionFromDbForUserType(userTypeId);
                _cache.Set(cacheKey, permissions, TimeSpan.FromMinutes(30));
            }

            return permissions;
        }
        public async Task<HashSet<int>> LoadPermissionFromDbForUserType(int userTypeId)
        {
            var p = _db.SqlParameters.AddMore("@UserTypeId", userTypeId);
            var q = @"SELECT PermissionId FROM UserTypePermission WHERE UserTypeId = @UserTypeId";
            var dt = await _db.ExecuteDataTableAsync(CommandType.Text, q, p);
           
            var set = new HashSet<int>();
            foreach (DataRow row in dt.Rows)
            {
                set.Add(Convert.ToInt32(row["PermissionId"]));
            }
            return set;
        }
        public void InvalidateUserTypePermissionCache(int userTypeId)
        {
            _cache.Remove($"permissions_usertype_{userTypeId}");
        }


        public async Task<DataTable> GetPermission(int id = 0)
        {
            var p = _db.SqlParameters.AddMore("@Id", id);
            var q = @"Select * from Permission Where @Id = 0 or Id = @Id";
            var dt = await _db.ExecuteDataTableAsync(CommandType.Text, q, p);
            return dt;
        }
        //public async Task<DataTable> GetUserTypePermission(int permissionId = 0, int userTypeId = 0)
        //{
        //    var p = _db.SqlParameters.AddMore("@PermissionId", permissionId)
        //                             .AddMore("@UserTypeId", userTypeId);

        //    var q = @"Select * from UserTypePermission Where (@PermissionId = 0 or PermissionId = @PermissionId) and (@UserTypeId = 0 or UserTypeId = @UserTypeId)";
        //    var dt = await _db.ExecuteDataTableAsync(CommandType.Text, q, p);
        //    return dt;
        //}

        #endregion


    }
}
