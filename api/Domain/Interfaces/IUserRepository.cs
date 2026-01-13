using Domain.Entities.User;
using Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Domain.Interfaces
{
    public interface IUserRepository
    {


        // -----------------------------------------------------------
        // 🔹 UserManagement
        // -----------------------------------------------------------



        #region UserType
        Task<List<UserType>> GetUserType(int id = 0, string name = "");
        Task<DbResponse> SaveUserType(UserType model);
        Task<DbResponse> DeleteUserType(int id, int currentUser);

        Task<int> GetUserTypeId(int userId);

        #endregion


        #region User

        Task<User?> GetUserById(int id);
        Task<User> GetByUsernameAsync(string username);
        Task<List<User>> GetUser(int id = 0, string userName = "", string fullName = "");
        Task<DbResponse> SaveUser(User model);
        Task<DbResponse> DeleteUser(int id);

        #endregion





        #region UserPermission
        Task<bool> HasPermission(int userId, int permissionId);
        Task<HashSet<int>> GetPermissionsForUserType(int userTypeId);
        Task<HashSet<int>> LoadPermissionFromDbForUserType(int userTypeId);

        Task<DataTable> GetPermission(int id = 0);
        //Task<DataTable> GetUserTypePermission(int permissionId = 0, int userTypeId = 0);

        #endregion
    }
}
