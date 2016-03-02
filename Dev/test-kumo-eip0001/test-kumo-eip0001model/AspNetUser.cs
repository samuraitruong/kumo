//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace test_kumo_eip0001model
{
    using System;
    using System.Collections.Generic;
    
    public partial class AspNetUser
    {
        public AspNetUser()
        {
            this.AspNetUserClaims = new HashSet<AspNetUserClaim>();
            this.AspNetUserLogins = new HashSet<AspNetUserLogin>();
            this.Calendars = new HashSet<Calendar>();
            this.Documents = new HashSet<Document>();
            this.Employees = new HashSet<Employee>();
            this.Employees1 = new HashSet<Employee>();
            this.IssueTrackers = new HashSet<IssueTracker>();
            this.TaskTrackers = new HashSet<TaskTracker>();
            this.UserActions = new HashSet<UserAction>();
            this.AspNetRoles = new HashSet<AspNetRole>();
        }
    
        public string Id { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public Nullable<System.DateTime> LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string UserName { get; set; }
        public string Company { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Status { get; set; }
    
        public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual ICollection<Calendar> Calendars { get; set; }
        public virtual ICollection<Document> Documents { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<Employee> Employees1 { get; set; }
        public virtual ICollection<IssueTracker> IssueTrackers { get; set; }
        public virtual ICollection<TaskTracker> TaskTrackers { get; set; }
        public virtual ICollection<UserAction> UserActions { get; set; }
        public virtual ICollection<AspNetRole> AspNetRoles { get; set; }
    }
}
