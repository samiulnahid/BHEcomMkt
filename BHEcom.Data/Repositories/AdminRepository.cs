using BHEcom.Common.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Microsoft.Data.SqlClient;
namespace BHEcom.Data.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AdminRepository> _logger;

        public AdminRepository(ApplicationDbContext context, ILogger<AdminRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        //public async Task AddAsync(Useres useres)
        //{
        //    try
        //    {
        //        User user = new User()
        //        {
        //            ApplicationId = useres.ApplicationId,
        //            UserName = useres.UserName,
        //            LoweredUserName = useres.UserName.ToLower(),
        //            LastActivityDate = DateTime.Now,
        //            MobileAlias = null,
        //            IsAnonymous = false,

        //        };

        //        if (user.UserId == Guid.Empty)
        //        {
        //            user.UserId = Guid.NewGuid();
        //        }

        //        var query = @"INSERT INTO aspnet_Users
        //                        (
        //                            ApplicationId,
        //                            UserId,
        //                            UserName,
        //                            LoweredUserName,
        //                            MobileAlias,
        //                            IsAnonymous,
        //                            LastActivityDate
        //                        )
        //                        VALUES
        //                        (
        //                            @ApplicationId,
        //                            @UserId,
        //                            @UserName,
        //                            @LoweredUserName,
        //                            @MobileAlias,
        //                            @IsAnonymous,
        //                            @LastActivityDate
        //                        );"; 

        //        var parameters = new[]
        //        {
        //            new SqlParameter("@ApplicationId", user.ApplicationId),
        //            new SqlParameter("@UserId", user.UserId),
        //            new SqlParameter("@UserName", user.UserName),
        //            new SqlParameter("@LoweredUserName", user.LoweredUserName),
        //            new SqlParameter("@MobileAlias", user.MobileAlias),
        //            new SqlParameter("@IsAnonymous", user.IsAnonymous),
        //            new SqlParameter("@LastActivityDate", user.LastActivityDate)
        //        };

        //        await _context.Database.ExecuteSqlRawAsync(query, parameters);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "An error occurred while adding a user.");
        //    }
        //}

        public async Task AddAsync(Useres useres)
        {
            try
            {
                User user = new User()
                {
                    ApplicationId = useres.ApplicationId,
                    UserName = useres.UserName,
                    LoweredUserName = useres.UserName.ToLower(),
                    LastActivityDate = DateTime.Now,
                    MobileAlias = null, // Assuming MobileAlias is null for this example
                    IsAnonymous = false,
                };
               
                if (user.UserId == Guid.Empty)
                {
                    user.UserId = Guid.NewGuid();
                }

                Membership membership = new Membership()
                {
                    
                    UserId = user.UserId,
                    Password = useres.Password,
                    CreateDate = DateTime.Now,
                    Email = useres.Email,
                    LoweredEmail = useres.Email.ToLower(),
                    MobilePIN = "mpin",
                    PasswordQuestion = useres.Password, 
                    PasswordAnswer = useres.Password,
                    Comment = "Membership"

                };

                var query = @"INSERT INTO [dbo].[aspnet_Users]
                        (
                            ApplicationId,
                            UserId,
                            UserName,
                            LoweredUserName,
                            MobileAlias,
                            IsAnonymous,
                            LastActivityDate
                        )
                        VALUES
                        (
                            @ApplicationId,
                            @UserId,
                            @UserName,
                            @LoweredUserName,
                            @MobileAlias,
                            @IsAnonymous,
                            @LastActivityDate
                        );";

                var parameters = new[]
                {
                    new SqlParameter("@ApplicationId", user.ApplicationId),
                    new SqlParameter("@UserId", user.UserId),
                    new SqlParameter("@UserName", user.UserName),
                    new SqlParameter("@LoweredUserName", user.LoweredUserName),
                    new SqlParameter("@MobileAlias", (object)user.MobileAlias ?? DBNull.Value),
                    new SqlParameter("@IsAnonymous", user.IsAnonymous),
                    new SqlParameter("@LastActivityDate", user.LastActivityDate)
                };
                var memberQuery = @"INSERT INTO [dbo].[aspnet_Membership] 
                      (
                          ApplicationId, 
                          UserId, 
                          Password, 
                          PasswordFormat, 
                          PasswordSalt, 
                          MobilePIN, 
                          Email, 
                          LoweredEmail, 
                          PasswordQuestion, 
                          PasswordAnswer, 
                          IsApproved, 
                          IsLockedOut, 
                          CreateDate, 
                          LastLoginDate, 
                          LastPasswordChangedDate, 
                          LastLockoutDate, 
                          FailedPasswordAttemptCount, 
                          FailedPasswordAttemptWindowStart, 
                          FailedPasswordAnswerAttemptCount, 
                          FailedPasswordAnswerAttemptWindowStart, 
                          Comment
                      )
                      VALUES
                      (
                          @ApplicationId, 
                          @UserId, 
                          @Password, 
                          @PasswordFormat, 
                          @PasswordSalt, 
                          @MobilePIN, 
                          @Email, 
                          @LoweredEmail, 
                          @PasswordQuestion, 
                          @PasswordAnswer, 
                          @IsApproved, 
                          @IsLockedOut, 
                          @CreateDate, 
                          @LastLoginDate, 
                          @LastPasswordChangedDate, 
                          @LastLockoutDate, 
                          @FailedPasswordAttemptCount, 
                          @FailedPasswordAttemptWindowStart, 
                          @FailedPasswordAnswerAttemptCount, 
                          @FailedPasswordAnswerAttemptWindowStart, 
                          @Comment
                      )";
                var memberParameters = new[]
                  {
                        new SqlParameter("@ApplicationId", membership.ApplicationId),
                        new SqlParameter("@UserId", membership.UserId),
                        new SqlParameter("@Password", membership.Password ?? (object)DBNull.Value),
                        new SqlParameter("@PasswordFormat", membership.PasswordFormat ?? (object)DBNull.Value),
                        new SqlParameter("@PasswordSalt", membership.PasswordSalt ?? (object)DBNull.Value),
                        new SqlParameter("@MobilePIN", membership.MobilePIN ?? (object)DBNull.Value),
                        new SqlParameter("@Email", membership.Email),
                        new SqlParameter("@LoweredEmail", membership.LoweredEmail ?? (object)DBNull.Value),
                        new SqlParameter("@PasswordQuestion", membership.PasswordQuestion ?? (object)DBNull.Value),
                        new SqlParameter("@PasswordAnswer", membership.PasswordAnswer ?? (object)DBNull.Value),
                        new SqlParameter("@IsApproved", membership.IsApproved),
                        new SqlParameter("@IsLockedOut", membership.IsLockedOut),
                        new SqlParameter("@CreateDate", membership.CreateDate ?? (object)DBNull.Value),
                        new SqlParameter("@LastLoginDate", membership.LastLoginDate ?? (object)DBNull.Value),
                        new SqlParameter("@LastPasswordChangedDate", membership.LastPasswordChangedDate ?? (object)DBNull.Value),
                        new SqlParameter("@LastLockoutDate", membership.LastLockoutDate ?? (object)DBNull.Value),
                        new SqlParameter("@FailedPasswordAttemptCount", membership.FailedPasswordAttemptCount ?? (object)DBNull.Value),
                        new SqlParameter("@FailedPasswordAttemptWindowStart", membership.FailedPasswordAttemptWindowStart ?? (object)DBNull.Value),
                        new SqlParameter("@FailedPasswordAnswerAttemptCount", membership.FailedPasswordAnswerAttemptCount ?? (object)DBNull.Value),
                        new SqlParameter("@FailedPasswordAnswerAttemptWindowStart", membership.FailedPasswordAnswerAttemptWindowStart ?? (object)DBNull.Value),
                        new SqlParameter("@Comment", membership.Comment ?? (object)DBNull.Value)
                    };

                await _context.Database.ExecuteSqlRawAsync(query, parameters);
                await _context.Database.ExecuteSqlRawAsync(memberQuery, memberParameters);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a user.");
            }
        }



        //public async Task<Useres> GetByIdAsync(Guid id)
        //{
        //   return await _context.Addresses.FindAsync(id);
        //}

        public async Task<Useres> GetByIdAsync(Guid id)
        {
            try
            {
                Useres useres = new Useres();
                var query = "SELECT * FROM aspnet_Users WHERE UserId = @UserId";

                var parameter = new SqlParameter("@UserId", id);

                var user = await _context.Users
                                         .FromSqlRaw(query, parameter)
                                         .FirstOrDefaultAsync();

                useres.UserId = user.UserId;
                useres.UserName = user.UserName;
               // useres.ApplicationId = user.ApplicationId;
                useres.CreateDate = user.LastActivityDate;


                return useres;

            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the user by ID.");
                throw;
            }
        }

        //public async Task<IEnumerable<Useres>> GetAllAsync()
        //{
        //    try
        //    {
        //        IEnumerable<Useres> useres = new IEnumerable<Useres>();
        //        var query = "SELECT * FROM aspnet_Users";

        //        var allUser = await _context.Users
        //                                  .FromSqlRaw(query)
        //                                  .ToListAsync();


        //        useres.UserId = allUser.UserId,
        //        return useres;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "An error occurred while retrieving all users.");
        //        throw;
        //    }
        //}

        public async Task<IEnumerable<Useres>> GetAllAsync()
        {
            try
            {
                var query = "SELECT * FROM aspnet_Users";

                var allUsers = await _context.Users
                                             .FromSqlRaw(query)
                                             .ToListAsync();

                var useresList = allUsers.Select(u => new Useres
                {
                    UserId = u.UserId,
                   // ApplicationId = u.ApplicationId,
                    UserName = u.UserName,
                    LoginDate = u.LastActivityDate,
                    Password = string.Empty,
                    Email = string.Empty,
                    IsLogin = false,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now
                }).ToList();

                return useresList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all users.");
                throw;
            }
        }


        public async Task UpdateAsync(Useres useres)
        {
            try
            {
                User user = new User()
                {
                    ApplicationId = useres.ApplicationId,
                    UserName = useres.UserName,
                    LoweredUserName = useres.UserName.ToLower(),
                    LastActivityDate = DateTime.Now,
                    MobileAlias = null,
                    IsAnonymous = false,

                };

                var query = @"UPDATE aspnet_Users
                                SET 
                                    ApplicationId = @ApplicationId
                                    UserName = @UserName,
                                    LoweredUserName = @LoweredUserName,
                                    MobileAlias = @MobileAlias,
                                    IsAnonymous = @IsAnonymous,
                                    LastActivityDate = @LastActivityDate
                                WHERE 
                                    UserId = @UserId ;";

                var parameters = new[]
                {
                    new SqlParameter("@UserId", user.UserId),
                    new SqlParameter("@ApplicationId", user.ApplicationId),
                    new SqlParameter("@UserName", user.UserName),
                    new SqlParameter("@LoweredUserName", user.LoweredUserName),
                    new SqlParameter("@MobileAlias", user.MobileAlias),
                    new SqlParameter("@IsAnonymous", user.IsAnonymous),
                    new SqlParameter("@LastActivityDate", user.LastActivityDate)
                };

                await _context.Database.ExecuteSqlRawAsync(query, parameters);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the user.");
                throw;
            }
        }


        //public async Task DeleteAsync(Guid id)
        //{
        //    var address = await _context.Addresses.FindAsync(id);
        //    if (address != null)
        //    {
        //        _context.Addresses.Remove(admin);
        //        await _context.SaveChangesAsync();
        //    }
        //}

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var query = "DELETE FROM aspnet_Users WHERE UserId = @Id"; // Assuming 'AddressId' is the primary key column

                var parameter = new SqlParameter("@Id", id);

                await _context.Database.ExecuteSqlRawAsync(query, parameter);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting an address.");
            }
        }


    }

}
