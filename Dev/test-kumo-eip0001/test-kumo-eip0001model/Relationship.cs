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
    
    public partial class Relationship
    {
        public Relationship()
        {
            this.EmergencyContacts = new HashSet<EmergencyContact>();
            this.EmployeeSensitives = new HashSet<EmployeeSensitive>();
            this.Employees = new HashSet<Employee>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
    
        public virtual ICollection<EmergencyContact> EmergencyContacts { get; set; }
        public virtual ICollection<EmployeeSensitive> EmployeeSensitives { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
    }
}