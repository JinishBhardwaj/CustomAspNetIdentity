
namespace Asp.Net.Identity.Context
{
    public class IdentityUserRole
    {
        #region Properties

        public string UserId { get; set; }
        public virtual IdentityUser User { get; set; }

        public string RoleId { get; set; }
        public virtual IdentityRole Role { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityUserRole"/> class
        /// </summary>
        public IdentityUserRole()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityUserRole"/> class
        /// </summary>
        /// <param name="userId">User Id for the Identity User</param>
        /// <param name="roleId">Role Id for the Identity User</param>
        public IdentityUserRole(string userId, string roleId)
        {
            this.UserId = userId;
            this.RoleId = roleId;
        }

        #endregion
    }
}
