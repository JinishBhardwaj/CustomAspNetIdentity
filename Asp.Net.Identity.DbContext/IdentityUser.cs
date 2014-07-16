using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asp.Net.Identity.Context
{
    public class IdentityUser: IUser
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityUser"/> class
        /// </summary>
        public IdentityUser()
        {
            Id = Guid.NewGuid().ToString();
            Roles = (ICollection<IdentityUserRole>)new List<IdentityUserRole>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityUser"/> class
        /// </summary>
        /// <param name="username">Username for the user</param>
        public IdentityUser(string username): this()
        {
            UserName = username;
        }

        #endregion

        #region IUser<string> members

        public string Id { get; set; }

        [Index("UserNameIndex", IsUnique = true)]
        public virtual string UserName { get; set; }

        #endregion

        #region Properties

        public virtual string PasswordHash { get; set; }

        public virtual string SecurityStamp { get; set; }

        public virtual string Email { get; set; }

        public virtual bool EmailConfirmed { get; set; }

        public virtual int AccessFailedCount { get; set; }

        public virtual ICollection<IdentityUserRole> Roles { get; set; }

        #endregion
    }
}
