using BHEcom.Common.Models;
using BHEcom.Data.Repositories;
using BHEcom.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Services.Implementations
{
    public class AdminService : IAdminService
    {
        private readonly IAdmin2Repository _adminRepository;

        public AdminService(IAdmin2Repository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public async Task<IdentityResult> RegisterAsync(User model)
        {
            var result = await _adminRepository.RegisterAsync(model);
            return result;
        }
        public async Task<Guid> RegisterAsyncMembership(User model, string roleName, string email)
        {
            return await _adminRepository.RegisterAsyncMembership(model, roleName, email);
        }
        public async Task<Guid> CreateUserAsync(User model, string roleName, string email)
        {
            //return await _adminRepository.AddAsync(model, roleName, email); 
            return await _adminRepository.RegisterAsyncMembership(model, roleName, email);
        }
        public async Task<bool> UpdateUserNameAsync(User user)
        {
            return await _adminRepository.UpdateUserAsync(user);
        }
       public async Task<bool> CheckUserNameExistAsync(User user)
       {
            return await _adminRepository.CheckUserNameExistAsync(user);
        }
    
        public async Task<SignInResult> LoginAsync(string userName, string password)
        {
            return await _adminRepository.LoginAsync(userName, password);
        }
        public async Task LogoutAsync()
        {
             await _adminRepository.LogoutAsync();
        }

        public async Task<Membership> GetUserByIdAsync(Guid userId)
        {
            return await _adminRepository.GetUserByIdAsync(userId);
        }

        public async Task<IdentityResult> UpdateUserAsync(Membership model)
        {
            return await _adminRepository.UpdateUserAsync(model);
        }
        public async Task<IdentityResult> DeleteUserAsync(Guid userId)
        {
            return await (_adminRepository.DeleteUserAsync(userId));
        }

       public async Task<(bool IsSuccess, Guid UserId, string RoleName, string UserName)> ValidateUser(string userName, string password)
        {
            return await _adminRepository.ValidateUser(userName, password);
        }

      
    }
}
