using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using test_kumo_eip0001model;
using test_kumo_eip0001repositories;

namespace test_kumo_eip0001application
{
    public class UserService
    {
        private readonly AspNetUserRepository aspNetUserRepository;
        private readonly AspNetUserClaimRepository aspNetUserClaimRepository;
        private readonly AspNetUserLoginRepository aspNetUserLoginRepository;
        private readonly AspNetRoleRepository aspNetRoleRepository;
        public UserService()
        {
            aspNetUserRepository = new AspNetUserRepository();
            aspNetUserClaimRepository = new AspNetUserClaimRepository();
            aspNetUserLoginRepository = new AspNetUserLoginRepository();
            aspNetRoleRepository = new AspNetRoleRepository();
        }
        

        #region [AspNetUser]
        public void DeleteUser(params AspNetUser[] users)
        {
            aspNetUserRepository.DeleteDeferred(users);
        }

        public void AddUser(params AspNetUser[] users)
        {
            aspNetUserRepository.AddDeferred(users);
        }

        public void UpdateUser(params AspNetUser[] users)
        {
            aspNetUserRepository.UpdateDeferred(users);
        }

        public IQueryable<AspNetUser> GetUsers()
        {
            return aspNetUserRepository.GetAll();
        }

        #endregion [AspNetUser]

        #region [AspNetUserClaim]
        public void DeleteUserClaim(params AspNetUserClaim[] userClaims)
        {
            aspNetUserClaimRepository.DeleteDeferred(userClaims);
        }

        public void AddUserClaim(params AspNetUserClaim[] userClaims)
        {
            aspNetUserClaimRepository.AddDeferred(userClaims);
        }

        public void UpdateUserClaim(params AspNetUserClaim[] userClaims)
        {
            aspNetUserClaimRepository.UpdateDeferred(userClaims);
        }

        public IQueryable<AspNetUserClaim> GetUserClaims()
        {
            return aspNetUserClaimRepository.GetAll();
        }

        #endregion [AspNetUserClaim]

        #region [AspNetUserLogin]
        public void DeleteUserLogin(params AspNetUserLogin[] userLogins)
        {
            aspNetUserLoginRepository.DeleteDeferred(userLogins);
        }

        public void AddUserLogin(params AspNetUserLogin[] userLogins)
        {
            aspNetUserLoginRepository.AddDeferred(userLogins);
        }

        public void UpdateUserLogin(params AspNetUserLogin[] userLogins)
        {
            aspNetUserLoginRepository.UpdateDeferred(userLogins);
        }

        public IQueryable<AspNetUserLogin> GetUserLogins()
        {
            return aspNetUserLoginRepository.GetAll();
        }

        #endregion [AspNetUserClaim]

        #region [AspNetRole]
        public void DeleteRole(params AspNetRole[] roles)
        {
            aspNetRoleRepository.DeleteDeferred(roles);
        }

        public void AddRole(params AspNetRole[] roles)
        {
            aspNetRoleRepository.AddDeferred(roles);
        }

        public void UpdateRole(params AspNetRole[] roles)
        {
            aspNetRoleRepository.UpdateDeferred(roles);
        }

        public IQueryable<AspNetRole> GetRoles()
        {
            return aspNetRoleRepository.GetAll();
        }

        #endregion [AspNetRole]
    }
}
