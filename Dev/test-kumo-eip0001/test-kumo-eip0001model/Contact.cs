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
    
    public partial class Contact
    {
        public Contact()
        {
            this.RevenueTrackers = new HashSet<RevenueTracker>();
        }
    
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int RoleId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string JobTitle { get; set; }
        public string EmailAddress { get; set; }
        public string MobileNumber { get; set; }
        public string OfficeNumber { get; set; }
        public string FaxNumber { get; set; }
    
        public virtual Customer Customer { get; set; }
        public virtual ContactRole ContactRole { get; set; }
        public virtual ICollection<RevenueTracker> RevenueTrackers { get; set; }
    }
}
