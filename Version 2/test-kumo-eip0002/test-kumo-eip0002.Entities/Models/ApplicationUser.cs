using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Repository.Pattern.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Kumo.Entities.Models
{

    public class ApplicationUserRole : IdentityUserRole, IObjectState
    {
        public ApplicationUserRole()
        {
            //this.ObjectState = Pattern.Infrastructure.ObjectState.Added;
        }
        [NotMapped]
        public ObjectState ObjectState { get; set; }
        [Key]
        [Column(Order = 0)]
        public override string RoleId { get; set; }
        [Key]
        [Column(Order = 1)]
        public override string UserId { get; set; }
    }

    public class ApplicationUser : IdentityUser, IObjectState
    {
        public string Firstname { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(
       UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one 
            // defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity =
                await manager.CreateIdentityAsync(this,
                    DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }


        [NotMapped]
        public ObjectState ObjectState { get; set; }
    }

    public class ApplicationRole : IdentityRole, IObjectState
    {
        public ApplicationRole() : base() { }
        //public ApplicationRole(string name) : base(name) { }
       [NotMapped]
        public string Description { get; set; }

        [NotMapped]
        public ObjectState ObjectState
        {
            get;
            set;
        }
        
    }

}
