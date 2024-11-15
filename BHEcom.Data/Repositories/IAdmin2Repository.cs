using BHEcom.Common.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Data.Repositories
{
    public interface IAdmin2Repository
    {
        //Task AddAsync(Useres user);
        //Task<Useres> GetByIdAsync(Guid id);
        //Task<IEnumerable<Useres>> GetAllAsync();
        //Task UpdateAsync(Useres user);
        //Task DeleteAsync(Guid id);
        Task<IdentityResult> RegisterAsync(Useres model);
        Task<Guid> RegisterAsyncMembership(User model, string roleName, string email);
        Task<SignInResult> LoginAsync(string userName, string password);
        Task LogoutAsync();
        Task<Membership> GetUserByIdAsync(Guid userId);
        Task<IdentityResult> UpdateUserAsync(Membership model);
        Task<IdentityResult> DeleteUserAsync(Guid userId);
        Task<Guid> AddAsync(User user, string roleName, string email);
        Task<Guid> CreateUserAndAssignRoleAsync(User user, string roleName, string email);
        Task<bool> UpdateUserAsync(User user);


    }

}
