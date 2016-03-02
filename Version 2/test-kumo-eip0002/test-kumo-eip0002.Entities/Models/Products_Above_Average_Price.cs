using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;

namespace Kumo.Entities.Models
{
    public partial class Products_Above_Average_Price : Entity
    {
        public string ProductName { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
    }
}
