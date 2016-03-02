using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;

namespace Kumo.Entities.Models
{
    public partial class CustomerAndSuppliersByCity : Entity
    {
        public string City { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string Relationship { get; set; }
    }
}
