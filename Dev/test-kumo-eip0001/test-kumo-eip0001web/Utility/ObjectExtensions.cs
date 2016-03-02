using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace System
{
    public static class ObjectExtensions
    {
        public static ExcelRangeBase LoadFromCollectionWithHeaders<T>(this ExcelRange excelRange, IEnumerable<T> list)
        {
            excelRange.LoadFromCollection<T>(list, true);

            int Row = 1;
            int ColumnsCount = excelRange.Worksheet.Cells.Count() / list.Count();
            for (int Column = 1; Column <= ColumnsCount; Column++)
            {
                string IncorrectHeader = (((OfficeOpenXml.ExcelRangeBase)(excelRange.Worksheet.Cells[Row, Column]))).Text;

                PropertyInfo[] Properties = typeof(T).GetProperties();
                foreach (PropertyInfo Property in Properties)
                {
                    if (IncorrectHeader == Property.Name.Replace('_', ' '))
                    {
                        object[] DisplayAttributes = Property.GetCustomAttributes(typeof(DisplayAttribute), true);

                        if (DisplayAttributes.Length == 1)
                        {
                            (((OfficeOpenXml.ExcelRangeBase)(excelRange.Worksheet.Cells[Row, Column]))).Value = ((DisplayAttribute)(DisplayAttributes[0])).Name;
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
            }

            return excelRange;
        }

        public static Type TypeOfMe<T>(this T obj)
        {
            Type t;
            if (obj == null)
                t = typeof(T);
            else
                t = obj.GetType();
            return t;
        }

        public static T CopyFrom<T>(this T toObject, object fromObject)
        {
            var fromObjectType = fromObject.GetType();

            foreach (PropertyInfo toProperty in toObject.GetType().GetProperties())
            {
                PropertyInfo fromProperty = fromObjectType.GetProperty(toProperty.Name);

                if (fromProperty != null) // match found
                {
                    // check types
                    var fromType = Nullable.GetUnderlyingType(fromProperty.PropertyType) ?? fromProperty.PropertyType;
                    var toType = Nullable.GetUnderlyingType(toProperty.PropertyType) ?? toProperty.PropertyType;

                    if (toType.IsAssignableFrom(fromType))
                    {
                        try
                        {
                            toProperty.SetValue(toObject, fromProperty.GetValue(fromObject, null), null);
                        }
                        catch(Exception ex)
                        {

                        }
                        
                    }
                }
            }

            return toObject;
        }
    }

    public static class Extensions
    {
        public static string IsSelected(this UrlHelper urlHelper, string controller)
        {
            string result = "active";

            string controllerName = urlHelper.RequestContext.RouteData.Values["controller"].ToString();

            if (!controllerName.Equals(controller, StringComparison.OrdinalIgnoreCase))
            {
                result = null;
            }

            return result;
        }
    }
}