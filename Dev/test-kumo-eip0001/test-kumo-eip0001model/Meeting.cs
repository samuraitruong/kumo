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
    
    public partial class Meeting
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public string Location { get; set; }
        public int AssignTo { get; set; }
        public string Description { get; set; }
    
        public virtual Employee Employee { get; set; }
    }
}
