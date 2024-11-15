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
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using System.Security.Cryptography;
namespace BHEcom.Data.Repositories
{

    public class Admin2Repository : IAdmin2Repository
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public Admin2Repository(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ApplicationDbContext context, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _configuration = configuration;
        }

        private string GenerateSalt()
        {
            // Generate a cryptographic random salt
            var saltBytes = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        private string HashPassword(string password, string salt)
        {
            // Use a hashing algorithm (e.g., SHA256 or SHA512) to hash the password with the salt
            using (var sha256 = SHA256.Create())
            {
                var combinedBytes = Encoding.UTF8.GetBytes(password + salt);
                var hashBytes = sha256.ComputeHash(combinedBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }

        public async Task<Guid> RegisterAsyncMembership(User model, string roleName, string email)
        {
            var application = await _context.Applications.OrderBy(a => a.ApplicationId).FirstOrDefaultAsync();
            if (application == null)
            {
                return Guid.NewGuid();
                
            }

            Guid applicationId = application.ApplicationId;


            // Step 2: Generate password salt and hash
            model.PasswordSalt = GenerateSalt();
            model.Password = HashPassword(model.Password, model.PasswordSalt);


            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    await connection.OpenAsync();

                    // Step 1: Check if the role exists
                    var role = await _context.Roles
                          .FirstOrDefaultAsync(r => r.ApplicationId == applicationId && r.RoleName.ToLower() == roleName.ToLower());

                    if (role == null)
                    {
                        // Step 2: If the role does not exist, create it
                        role = new Role
                        {
                            ApplicationId = applicationId,
                            RoleId = Guid.NewGuid(), // Generating a new GUID for the role ID
                            RoleName = roleName,
                            LoweredRoleName = roleName.ToLower(),
                            Description = $"{roleName} role created by system."
                        };

                        await _context.Roles.AddAsync(role);
                        await _context.SaveChangesAsync();
                    }

                    // Step 3: Create the user
                    Guid userId = Guid.NewGuid();
                    using (var createUserCommand = new SqlCommand("aspnet_Membership_CreateUser", connection))
                    {
                        createUserCommand.CommandType = CommandType.StoredProcedure;
                        createUserCommand.Parameters.AddWithValue("@ApplicationName", applicationId);
                        createUserCommand.Parameters.AddWithValue("@UserName", model.UserName);
                        createUserCommand.Parameters.AddWithValue("@Password", model.Password);
                        createUserCommand.Parameters.AddWithValue("@PasswordSalt", model.PasswordSalt);
                        createUserCommand.Parameters.AddWithValue("@Email", model.Email);
                        createUserCommand.Parameters.AddWithValue("@PasswordQuestion", DBNull.Value);
                        createUserCommand.Parameters.AddWithValue("@PasswordAnswer", DBNull.Value);
                        createUserCommand.Parameters.AddWithValue("@IsApproved", true);
                        createUserCommand.Parameters.AddWithValue("@CurrentTimeUtc", DateTime.UtcNow);
                        createUserCommand.Parameters.AddWithValue("@CreateDate", DateTime.UtcNow);
                        createUserCommand.Parameters.AddWithValue("@UniqueEmail", 1); // Assuming UniqueEmail is required
                        createUserCommand.Parameters.AddWithValue("@PasswordFormat", 0); // Assuming PasswordFormat is required

                        // Add UserId as OUTPUT parameter
                        var userIdParameter = new SqlParameter("@UserId", SqlDbType.UniqueIdentifier)
                        {
                            Direction = ParameterDirection.Output
                        };
                        createUserCommand.Parameters.Add(userIdParameter);

                        await createUserCommand.ExecuteNonQueryAsync();

                        // Retrieve the newly generated UserId
                        userId = (Guid)userIdParameter.Value;
                    }

                    // Step 4: Assign the user to the role

                    using (var addUserToRoleCommand = new SqlCommand("aspnet_UsersInRoles_AddUsersToRoles", connection))
                    {
                        addUserToRoleCommand.CommandType = CommandType.StoredProcedure;
                        addUserToRoleCommand.Parameters.AddWithValue("@ApplicationName", application.ApplicationName);
                        addUserToRoleCommand.Parameters.AddWithValue("@UserNames", model.UserName);
                        addUserToRoleCommand.Parameters.AddWithValue("@RoleNames", roleName);
                        addUserToRoleCommand.Parameters.AddWithValue("@CurrentTimeUtc", DateTime.UtcNow);

                        await addUserToRoleCommand.ExecuteNonQueryAsync();
                    }

                    return userId;
                }
                catch (Exception ex)
                {
                    // Log the error and return failure result
                    return Guid.NewGuid();
                }
            }
        }



        public async Task<IdentityResult> RegisterAsync(Useres model)
        {
            var user = new IdentityUser
            {
                UserName = model.Email, // Assuming the username is the email
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            Membership member = new Membership();
            if (result.Succeeded)
            {
                member.UserId = Guid.Parse(user.Id);
                member.CreateDate = DateTime.UtcNow;
                member.IsApproved = true;
                member.IsLockedOut = false;
                member.FailedPasswordAttemptCount = 0;
                member.FailedPasswordAnswerAttemptCount = 0;
                member.LoweredEmail = model.Email.ToLower();

                // Add logic to save the Membership model to your custom table, if necessary.
            }

            _context.Memberships.Add(member);
            await _context.SaveChangesAsync(); // This saves the Membership record to the database
            return result;
        }

        public async Task<SignInResult> LoginAsync(string userName, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(userName, password, isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(userName);
                if (user != null)
                {
                    var membership = await GetUserByIdAsync(Guid.Parse(user.Id));
                    if (membership != null)
                    {
                        membership.LastLoginDate = DateTime.UtcNow;
                        // Add logic to update the LastLoginDate in your custom table
                    }
                }
            }
            return result;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<Membership> GetUserByIdAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return null;

            // Assuming you have a method to retrieve the custom fields from your custom table
            return new Membership
            {
                UserId = Guid.Parse(user.Id),
                Email = user.Email,
                // Name = user.UserName,
                // Map other fields from your custom table if needed
            };
        }

        public async Task<IdentityResult> UpdateUserAsync(Membership model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId.ToString());
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });

            user.UserName = model.Email; // Assuming the username is the email
            user.Email = model.Email;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                // Update custom fields in your custom table, if needed
            }
            return result;
        }

        public async Task<IdentityResult> DeleteUserAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });

            return await _userManager.DeleteAsync(user);
        }

        public async Task<Guid> AddAsync(User user, string roleName, string email)
        {
            try
            {
                // Retrieve the top 1 ApplicationId from the Application table
                var applicationId = await _context.Applications
                                                  .OrderBy(a => a.ApplicationId)
                                                  .Select(a => a.ApplicationId)
                                                  .FirstOrDefaultAsync();

                // Check if ApplicationId is retrieved
                if (applicationId == Guid.Empty)
                {
                    return Guid.Empty;
                }

                // Set the retrieved ApplicationId to the User entity
                user.ApplicationId = applicationId;

                // Add user to the Users table
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                if (user.UserId == Guid.Empty) return Guid.Empty;

                // Generate a random password
                var password = GenerateRandomPassword();

                // Create Member 
                var membership = await AddMembershipAsync(user.UserId, applicationId, password, email);
                if (membership == null) return Guid.Empty;


                // Get role by name
                var roleId = await GetRoleIdByNameAsync(roleName, applicationId);
                if (roleId == Guid.Empty) return Guid.Empty;


                // Assign role to user
                bool isAdeed = await AssignRoleToUserAsync(user.UserId, roleId);
                if (!isAdeed) return Guid.Empty;

                // Return the newly created UserId
                return user.UserId;
            }
            catch (Exception)
            {
                return Guid.Empty;
            }
        }


        private async Task<Membership?> AddMembershipAsync(Guid userId, Guid applicationId, string password, string email)
        {
            var membership = new Membership
            {
                ApplicationId = applicationId,
                UserId = userId,
                Password = password,
                PasswordFormat = 1,
                PasswordSalt = "SomeSalt",
                IsApproved = true,
                IsLockedOut = false,
                CreateDate = DateTime.UtcNow,
                LastLoginDate = DateTime.UtcNow,
                FailedPasswordAttemptCount = 0,
                Email = email,
                LoweredEmail = email.ToLower(),
                FailedPasswordAnswerAttemptCount = 0,
                LastPasswordChangedDate = DateTime.UtcNow
            };

            _context.Memberships.Add(membership);
            var result = await _context.SaveChangesAsync();

            // Return the created Membership if saved successfully, otherwise null
            return result > 0 ? membership : null;
        }


        public async Task<bool> UpdateUserAsync(User user)
        {
            if (user?.UserId == null)
            {
                // Log the error or throw an exception if needed
                return false;
            }

            try
            {
                // Find the existing user by UserId
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserId == user.UserId);

                if (existingUser == null)
                {
                    return false; // Return false if the user is not found
                }

                // Check if the UserName has changed
                if (existingUser.UserName == user.UserName)
                {
                    return true; // No update needed, return true as nothing was changed
                }

                // Update the UserName
                existingUser.UserName = user.UserName;
                existingUser.LoweredUserName = user.LoweredUserName;

                // Save the changes to the database
                _context.Users.Update(existingUser);
                await _context.SaveChangesAsync();

                return true; // Return true to indicate success
            }
            catch (Exception)
            {
                // Log the exception if necessary
                return false; // Return false in case of an exception
            }
        }

        private string GenerateRandomPassword()
        {
            // Generate a simple random password (use more secure logic for production)
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 8)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        private async Task<bool> AssignRoleToUserAsync(Guid userId, Guid roleId)
        {
            var userInRole = new UsersInRole
            {
                UserId = userId,
                RoleId = roleId
            };

            _context.UsersInRoles.Add(userInRole);
            var result = await _context.SaveChangesAsync();

            // Return true if saved successfully, otherwise false
            return result > 0;
        }


        private async Task<Guid> GetRoleIdByNameAsync(string roleName, Guid applicationId)
        {
            var role = await _context.Roles
                .FirstOrDefaultAsync(r => r.RoleName == roleName && r.ApplicationId == applicationId);

            return role?.RoleId ?? Guid.Empty;
        }

        //public async Task<Guid> CreateUserAndAssignRoleAsync(User user, string roleName, string email)
        //{
        //    try
        //    {
        //        // Retrieve the top 1 Application from the Applications table
        //        var application = await _context.Applications
        //                                        .OrderBy(a => a.ApplicationId)
        //                                        .FirstOrDefaultAsync();

        //        if (application != null)
        //        {
        //            string password = GenerateRandomPassword();
        //            // Prepare other parameters
        //            var parameters = new[]
        //            {
        //                new SqlParameter("@ApplicationName", application.ApplicationName),
        //                new SqlParameter("@ApplicationId", application.ApplicationId),
        //                new SqlParameter("@UserName", user.UserName),
        //                new SqlParameter("@Password", password),            
        //                new SqlParameter("@Email", email),
        //                new SqlParameter("@LoweredEmail", email.ToLower()),
        //                new SqlParameter("@IsApproved", true),
        //                new SqlParameter("@IsLockedOut", false),
        //                new SqlParameter("@CreateDate",  DateTime.UtcNow),
        //                new SqlParameter("@CurrentTimeUtc", DateTime.UtcNow),

        //            };


        //            // Execute the stored procedure
        //            await _context.Database.ExecuteSqlRawAsync(
        //                "EXEC [dbo].[aspnet_Membership_CreateUser] " +
        //                "@ApplicationName,@ApplicationId, @UserName, @Password, @Email," +
        //                "@LoweredEmail, @IsApproved, @IsLockedOut, @CurrentTimeUtc, @CreateDate OUTPUT ",
        //                parameters
        //            );
        //        }
        //        return Guid.Empty;
        //    }
        //    catch (Exception)
        //    {
        //        return Guid.Empty;
        //    }
        //}


        public async Task<Guid> CreateUserAndAssignRoleAsync(User user, string roleName, string email)
        {
            try
            {
                // Retrieve the top 1 Application from the Applications table
                var application = await _context.Applications
                                                .OrderBy(a => a.ApplicationId)
                                                .FirstOrDefaultAsync();

                if (application != null)
                {
                    var (hashedPassword, salt, plainPassword) = CreateRandomHashedPassword();

                    // Prepare other parameters
                    var parameters = new[]
                    {
                        new SqlParameter("@ApplicationName", application.ApplicationName),
                        new SqlParameter("@UserName", user.UserName),
                        new SqlParameter("@Password", plainPassword),
                        new SqlParameter("@PasswordSalt", salt),
                        new SqlParameter("@Email", email),
                        new SqlParameter("@PasswordQuestion", DBNull.Value),
                        new SqlParameter("@PasswordAnswer", DBNull.Value),
                        new SqlParameter("@IsApproved", true),
                        new SqlParameter("@CurrentTimeUtc", DateTime.UtcNow),
                        //new SqlParameter("@CreateDate", DateTime.UtcNow),
                        new SqlParameter("@UniqueEmail", 1),
                        new SqlParameter("@PasswordFormat", 0), // Added @PasswordFormat parameter
                        new SqlParameter("@UserId", SqlDbType.UniqueIdentifier) { Direction = ParameterDirection.Output }
                    };

                    // Execute the stored procedure
                    await _context.Database.ExecuteSqlRawAsync(
                        "EXEC [dbo].[aspnet_Membership_CreateUser] " +
                        "@ApplicationName, @UserName, @Password, @PasswordSalt, @Email, " +
                        "@PasswordQuestion, @PasswordAnswer, @IsApproved, @CurrentTimeUtc,  " +
                        "@UniqueEmail, @PasswordFormat, @UserId OUTPUT",
                        parameters
                    );

                    // Retrieve the output UserId
                    var userId = (Guid)parameters.First(p => p.ParameterName == "@UserId").Value;
                    return userId;
                }

                return Guid.Empty;
            }
            catch (Exception)
            {
                return Guid.Empty;
            }
        }


        public static (string hashedPassword, string salt, string plainPassword) CreateRandomHashedPassword()
        {
            // Generate a random password
            string password = GenerateRandomPassword(12); // 12-character password

            // Generate a random salt
            byte[] saltBytes = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }
            string salt = Convert.ToBase64String(saltBytes);

            // Combine the password and salt, then hash them
            using (var sha256 = SHA256.Create())
            {
                var combinedPasswordSalt = password + salt;
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combinedPasswordSalt));
                string hashedPassword = Convert.ToBase64String(hashBytes);

                return (hashedPassword, salt, password); // Include the plain password for reference
            }
        }

        private static string GenerateRandomPassword(int length)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder passwordBuilder = new StringBuilder();
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] randomBytes = new byte[1];
                for (int i = 0; i < length; i++)
                {
                    rng.GetBytes(randomBytes);
                    int randomIndex = randomBytes[0] % validChars.Length;
                    passwordBuilder.Append(validChars[randomIndex]);
                }
            }
            return passwordBuilder.ToString();
        }
    }
}

