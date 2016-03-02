using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using test_kumo_eip0001application;
using test_kumo_eip0001model.Resources;

namespace test_kumo_eip0001web.Utility
{
    public class MetadataHelper
    {
        public SelectList GetSelectListGeneric<T>(string displayField, string idField) where T : class
        {

            var query = new ServiceBase<T>().GetAll();
            var list =  query.ToList();
         return   new SelectList(list, idField, displayField, null);

        }
        public static SelectList GetSelectList(Type type, string value, string displayfield="Name", string idfield ="Id") {

            List<object> parameters = new List<object>();
            parameters.Add(displayfield);
            parameters.Add(idfield);
            //parameters.Add(orderby);
            MethodInfo method = typeof(MetadataHelper).GetMethod("GetSelectListGeneric");
            MethodInfo generic = method.MakeGenericMethod(type);
            MetadataHelper helper = new MetadataHelper();

            var result = (SelectList)generic.Invoke(helper, parameters.ToArray());
            return result;
            //return new SelectList(list, "Id", "Name",null);
            //return  new SelectList(new[] { 
            //   new  {Name= StringResources.EmployeeStatus_Current, Id=StringResources.EmployeeStatus_Current},
            //    new  {Name=StringResources.EmployeeStatus_Fired, Id=StringResources.EmployeeStatus_Fired},
            //    new  {Name=StringResources.EmployeeStatus_Resigned, Id=StringResources.EmployeeStatus_Resigned}
            //}, "Name", "Id", value);

        }
    }
}