using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;

namespace Kumo.Entities.Models
{
    public partial class CurrentProductList : Entity
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
    }
}
