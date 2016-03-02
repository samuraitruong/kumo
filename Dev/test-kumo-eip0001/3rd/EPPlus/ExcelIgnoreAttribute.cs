using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OfficeOpenXml
{
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]

    public class ExcelIgnoredAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]

    public class ExcelOrderAttribute : Attribute
    {
        public int Order { get; set; }
        public ExcelOrderAttribute(int order)
        {
            this.Order = order;
        }
    }

}
