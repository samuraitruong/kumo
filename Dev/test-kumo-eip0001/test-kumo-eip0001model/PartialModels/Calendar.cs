using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test_kumo_eip0001model
{
    public partial class Calendar
    {
        public  AspNetUser User { 
            get {return this.AspNetUser;}
            set { this.AspNetUser = value; }
        }
    }
}
