using Microsoft.AspNet.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asp.Net.Identity.Context
{
    public class IdentityRole: IRole
    {
        #region IRole<string> members

        public string Id { get; set; }

        [Index("RoleNameIndex", IsUnique = true)]
        public virtual string Name { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityRole"/> class
        /// </summary>
        public IdentityRole()
        {
            Id = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityRole"/> class
        /// </summary>
        /// <param name="roleName">Role name for the Identity User</param>
        public IdentityRole(string roleName)
            : this()
        {
            Name = roleName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityRole"/> class
        /// </summary>
        /// <param name="roleName">Role name for the Identity User</param>
        /// <param name="id">Role Id</param>
        public IdentityRole(string roleName, string id)
        {
            Name = roleName;
            Id = id;
        }

        #endregion
    }
}
