using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.Net.Identity.Context.Stores
{
    public class RoleStore: IRoleStore<IdentityRole>
    {
        #region Private members

        private DbContext context;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleStore"/> class
        /// </summary>
        /// <param name="dbContext">DbContext to initialize the RoleStore with</param>
        /// <exception cref="ArgumentNullException">DbContext must not be null</exception>
        public RoleStore(DbContext dbContext)
        {
            if (dbContext == null)
                throw new ArgumentNullException("dbContext");
            context = dbContext;
        }

        #endregion

        #region IRoleStore<TRole> members

        /// <summary>
        /// Creates a new Identity Role
        /// </summary>
        /// <param name="role">Role to create</param>
        /// <returns>Task</returns>
        public Task CreateAsync(IdentityRole role)
        {
            if (role == null)
                throw new ArgumentNullException("role");
            context.Set<IdentityRole>().Add(role);
            return context.SaveChangesAsync();
        }

        /// <summary>
        /// Removes an existing Identity Role
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public Task DeleteAsync(IdentityRole role)
        {
            if (role == null)
                throw new ArgumentNullException("role");
            context.Set<IdentityRole>().Remove(role);
            return context.SaveChangesAsync();
        }

        /// <summary>
        /// Finds a role with the provided role id
        /// </summary>
        /// <param name="roleId">Id of the role to find</param>
        /// <returns>Identity Role</returns>
        public Task<IdentityRole> FindByIdAsync(string roleId)
        {
            if (string.IsNullOrEmpty(roleId))
                throw new ArgumentNullException("roleId");
            return context.Set<IdentityRole>().Where(r => r.Id.ToLower() == roleId.ToLower())
                                              .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Finds a role with the provided name
        /// </summary>
        /// <param name="roleName">Name of the role to find</param>
        /// <returns>Identity Role</returns>
        public Task<IdentityRole> FindByNameAsync(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
                throw new ArgumentNullException("roleName");
            return context.Set<IdentityRole>().Where(r => r.Name.ToLower() == roleName.ToLower())
                                              .FirstOrDefaultAsync(); 
        }

        /// <summary>
        /// Updates an existing role
        /// </summary>
        /// <param name="role">Role to update</param>
        /// <returns>Task</returns>
        public Task UpdateAsync(IdentityRole role)
        {
            if (role == null)
                throw new ArgumentNullException("role");
            context.Set<IdentityRole>().Attach(role);
            context.Entry(role).State = EntityState.Modified;
            return context.SaveChangesAsync();
        }

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

        #endregion
    }
}
