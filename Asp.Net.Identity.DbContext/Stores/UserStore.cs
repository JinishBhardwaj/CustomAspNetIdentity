using Asp.Net.Identity.Context;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;

namespace Asp.Net.Identity.Context.Stores
{
    public class UserStore: IUserStore<IdentityUser>,
                            IUserRoleStore<IdentityUser>,
                            IUserPasswordStore<IdentityUser>,
                            IUserSecurityStampStore<IdentityUser>,
                            IUserEmailStore<IdentityUser>
    {
        #region Private members

        private DbContext context;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UserStore"/> class
        /// </summary>
        /// <param name="dbContext">DbContext to initialize the RoleStore with</param>
        /// <exception cref="ArgumentNullException">DbContext must not be null</exception>
        public UserStore(DbContext dbContext)
        {
            if (dbContext == null)
                throw new ArgumentNullException("dbContext");
            context = dbContext;
        }

        #endregion

        #region IUserStore<IdentityUser, string> members

        /// <summary>
        /// Creates a new Identity User
        /// </summary>
        /// <param name="user">User to create</param>
        /// <returns>Task</returns>
        public Task CreateAsync(IdentityUser user)
        {
            if (user == null) 
                throw new ArgumentNullException("user");
            context.Set<IdentityUser>().Add(user);
            return context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes an existing Identity User
        /// </summary>
        /// <param name="user">User to delete</param>
        /// <returns>Task</returns>
        public Task DeleteAsync(IdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            context.Set<IdentityUser>().Remove(user);
            return context.SaveChangesAsync();
        }

        /// <summary>
        /// Finds an Identity User with the provided user id
        /// </summary>
        /// <param name="userId">User id for the user to find</param>
        /// <returns>Identity User</returns>
        public Task<IdentityUser> FindByIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException("userId");
            return context.Set<IdentityUser>().Where(u => u.Id.ToLower() == userId.ToLower())
                                              .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Finds an Identity User with the provided username
        /// </summary>
        /// <param name="userName">Username of the user to find</param>
        /// <returns>Identity User</returns>
        public Task<IdentityUser> FindByNameAsync(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException("userName");
            return context.Set<IdentityUser>().Where(u => u.UserName.ToLower() == userName.ToLower())
                                              .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Updates an existing Identity User
        /// </summary>
        /// <param name="user">User to update</param>
        /// <returns>Task</returns>
        public Task UpdateAsync(IdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            context.Set<IdentityUser>().Attach(user);
            context.Entry(user).State = EntityState.Modified;
            return context.SaveChangesAsync();
        }        

        #endregion

        #region IUserRoleStore<IdentityUser, string> members

        /// <summary>
        /// Adds a user to a role
        /// </summary>
        /// <param name="user">User to add to the role</param>
        /// <param name="roleName">Role to add the user to</param>
        /// <returns>Task</returns>
        public Task AddToRoleAsync(IdentityUser user, string roleName)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            if (string.IsNullOrEmpty(roleName))
                throw new ArgumentNullException("roleName");
            var role = Queryable.SingleOrDefault<IdentityRole>((IQueryable<IdentityRole>)context.Set<IdentityRole>(), (Expression<Func<IdentityRole, bool>>)(r => r.Name.ToLower() == roleName));
            if (role == null)
                throw new InvalidOperationException("role not found");
            context.Set<IdentityUserRole>().Add(new IdentityUserRole 
            { 
                RoleId = role.Id,
                UserId = user.Id
            });
            return context.SaveChangesAsync();
        }

        /// <summary>
        /// Gets the roles for a user
        /// </summary>
        /// <param name="user">User to get the roles for</param>
        /// <returns>List of role names</returns>
        public Task<IList<string>> GetRolesAsync(IdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            var userRoles = (IList<string>)Enumerable.ToList<string>(Enumerable.Select<IdentityUserRole, string>((IEnumerable<IdentityUserRole>)user.Roles, (Func<IdentityUserRole, string>)(u => u.RoleId)));
            var roles = context.Set<IdentityRole>().Where(r => userRoles.Contains(r.Id)).Select(r => r.Name).ToList();
            return Task.FromResult<IList<string>>(roles);
        }

        /// <summary>
        /// Checks whether a user is in a role or not
        /// </summary>
        /// <param name="user">User to check</param>
        /// <param name="roleName">Rolename to check users membership</param>
        /// <returns>True if user is in given role, False otherwise</returns>
        public Task<bool> IsInRoleAsync(IdentityUser user, string roleName)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            if (string.IsNullOrEmpty(roleName))
                throw new ArgumentNullException("roleName");
            var userRoles = (IList<string>)Enumerable.ToList<string>(Enumerable.Select<IdentityUserRole, string>((IEnumerable<IdentityUserRole>)user.Roles, (Func<IdentityUserRole, string>)(u => u.RoleId)));
            var roles = context.Set<IdentityRole>().Where(r => userRoles.Contains(r.Id));
            return Task.FromResult<bool>(roles.Any(r => r.Name == roleName));
        }

        /// <summary>
        /// Removes a user from a role
        /// </summary>
        /// <param name="user">User to remove from the role</param>
        /// <param name="roleName">Role to remove the user from</param>
        /// <returns>Task</returns>
        public Task RemoveFromRoleAsync(IdentityUser user, string roleName)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            if (string.IsNullOrEmpty(roleName))
                throw new ArgumentNullException("roleName");
            var role = Queryable.SingleOrDefault<IdentityRole>((IQueryable<IdentityRole>)context.Set<IdentityRole>(), (Expression<Func<IdentityRole, bool>>)(r => r.Name.ToLower() == roleName));
            if (role != null)
            {
                var userRole = context.Set<IdentityUserRole>()
                                      .Where(r => r.RoleId == role.Id)
                                      .FirstOrDefault();
                context.Set<IdentityUserRole>().Remove(userRole);
                return context.SaveChangesAsync();
            }
            return Task.FromResult<int>(0);
        }

        #endregion

        #region IUserPasswordStore<IdentityUser, string> members

        /// <summary>
        /// Gets the password for an Identity User
        /// </summary>
        /// <param name="user">User to get the password for</param>
        /// <returns>Password</returns>
        public Task<string> GetPasswordHashAsync(IdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            return Task.FromResult<string>(user.PasswordHash);
        }

        /// <summary>
        /// Checks whether the user has a password
        /// </summary>
        /// <param name="user">User to check the password existence for</param>
        /// <returns>True if user has password, False otherwise</returns>
        public Task<bool> HasPasswordAsync(IdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            return Task.FromResult<bool>(!string.IsNullOrEmpty(user.PasswordHash));
        }

        /// <summary>
        /// Sets the password for a user
        /// </summary>
        /// <param name="user">User to set the password for</param>
        /// <param name="passwordHash">Password to set</param>
        /// <returns>Task</returns>
        public Task SetPasswordHashAsync(IdentityUser user, string passwordHash)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            if (string.IsNullOrEmpty(passwordHash))
                throw new ArgumentNullException("passwordHash");
            user.PasswordHash = passwordHash;
            return Task.FromResult<int>(0);
        }

        #endregion

        #region IUserSecurityStampStore<IdentityUser, string> members

        /// <summary>
        /// Gets the password for an Identity User
        /// </summary>
        /// <param name="user">User to get the security stamp for</param>
        /// <returns>Security Stamp</returns>
        public Task<string> GetSecurityStampAsync(IdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            return Task.FromResult<string>(user.SecurityStamp);
        }

        /// <summary>
        /// Sets the security stamp for an Identity User
        /// </summary>
        /// <param name="user">User to set the security stamp for</param>
        /// <param name="stamp">Security stamp to set</param>
        /// <returns>Task</returns>
        public Task SetSecurityStampAsync(IdentityUser user, string stamp)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            if (string.IsNullOrEmpty(stamp))
                throw new ArgumentNullException("stamp");
            user.SecurityStamp = stamp;
            return Task.FromResult<int>(0);
        }

        #endregion

        #region IUserEmailStore<IdentityUser, string> members

        /// <summary>
        /// Finds an Identity User with the provided email
        /// </summary>
        /// <param name="email">Email for the user to find</param>
        /// <returns>Identity User</returns>
        public Task<IdentityUser> FindByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
                throw new ArgumentNullException("email");
            return context.Set<IdentityUser>().Where(u => u.Email.ToLower() == email.ToLower())
                                              .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Gets the email for the user
        /// </summary>
        /// <param name="user">User to get email for</param>
        /// <returns>Email</returns>
        public Task<string> GetEmailAsync(IdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            return Task.FromResult<string>(user.Email);
        }

        /// <summary>
        /// Gets the flag indicating if the users email is confirmed or not
        /// </summary>
        /// <param name="user">Identity User</param>
        /// <returns>True if email confirmed, False otherwise</returns>
        public Task<bool> GetEmailConfirmedAsync(IdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            return Task.FromResult<bool>(user.EmailConfirmed);
        }

        /// <summary>
        /// Sets email for the user
        /// </summary>
        /// <param name="user">User to set email for</param>
        /// <param name="email">Email to set</param>
        /// <returns>Task</returns>
        public Task SetEmailAsync(IdentityUser user, string email)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            if (string.IsNullOrEmpty(email))
                throw new ArgumentNullException("email");
            user.Email = email;
            return Task.FromResult<int>(0);
        }

        /// <summary>
        /// Sets the flag indicating that the users email has been confirmed
        /// </summary>
        /// <param name="user">Identity User</param>
        /// <param name="confirmed">Confirmed flag</param>
        /// <returns>Task</returns>
        public Task SetEmailConfirmedAsync(IdentityUser user, bool confirmed)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            user.EmailConfirmed = confirmed;
            return Task.FromResult<int>(0);
        }

        #endregion

        #region IDisposable members

        /// <summary>
        /// Disposes the context
        /// </summary>
        public void Dispose()
        {
            if (context != null)
            {
                context = null;
            }
        }

        #endregion
    }
}
