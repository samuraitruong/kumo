//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KUMO.CentralAdmin.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class EIPUser
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public Nullable<int> ClientId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public bool IsAdmin { get; set; }
        public string Company { get; set; }
        public string Phone { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string Type { get; set; }
    
        public virtual Client Client { get; set; }
    }
}