using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;

namespace Kumo.Entities.Models
{
    public partial class SummaryOfSalesByYear : Entity
    {
        public Nullable<System.DateTime> ShippedDate { get; set; }
        public int OrderID { get; set; }
        public Nullable<decimal> Subtotal { get; set; }
    }
}
