using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;
using PagedList;

using test_kumo_eip0001application;
using test_kumo_eip0001model;

namespace test_kumo_eip0001web.Controllers
{
    public class ExportController : Controller
    {
        private const int BATCH_SIZE = 5000;

        // GET: Export
        //TODO : add where condition, order by clause in generic export method, that is so cool :D

        public byte[] Export<T>(string fields, string whereClause, string orderby)  where T :  class
        {
            List<T> allrows = new List<T>();

            byte[] data = null;
            bool hasNextPage = true;
            int total = 0;
            do
            {
                var query = new ServiceBase<T>().GetAll();
                //TODO : Implement paging to improve perfomance
                if (!string.IsNullOrEmpty(whereClause))
                {
                    query = query.Where(whereClause);

                }
                if (!string.IsNullOrEmpty(orderby))
                {
                    query = query.OrderBy(orderby);
                }
                var rows = query.Skip(total).Take(BATCH_SIZE).ToList();
                total += rows.Count;

                allrows.AddRange(rows);
                hasNextPage = rows.Count > 0;
            }
            while (hasNextPage);


            data = ExportToExcel<T>(allrows, fields);
            return data;

        }

        public ActionResult Index(string exportObject ="Employee", string filename = "", string fields="", string filters="",string orderby="Id")
        {
            //ToDo : Check permission

            if(string.IsNullOrEmpty(filename)) {
                filename = exportObject +"_Exported.xlsx";
            }
            byte [] data  = null;
            var type = ResolveType(exportObject);
            //invoke a  method to get list string
            if (fields == "_auto_")
            {
                MethodInfo excelFieldsMethod = type.GetMethod("ExcelFields");
                if (excelFieldsMethod != null){
                    fields = (string)excelFieldsMethod.Invoke(null, null);
                }
                else{
                    throw new Exception(string.Format("{0} doesn't support for auto generate fields parameter."+
                    "\nplease remove fields parameter to include all field or specific list of field to be display in excel file.", type.Name));

                }

            }

            List<object> parameters = new List<object>();
            parameters.Add(fields);
            parameters.Add(filters);
            parameters.Add(orderby);
            MethodInfo method = typeof(ExportController).GetMethod("Export");
            MethodInfo generic = method.MakeGenericMethod(type);
            data = (byte[])generic.Invoke(this, parameters.ToArray());

            
            //if (type == typeof(Employee))
            //{
            //    var rows = (IEnumerable<Employee>) (new EmployeeService()).GetAll().ToList();
            //    data = ExportToExcel<Employee>(rows);

            //}

            //if (type == typeof(IssueTracker))
            //{
            //    var rows = (IEnumerable<IssueTracker>)(new IssueTrackerService()).GetAll().ToList();
            //    data = ExportToExcel<IssueTracker>(rows);

            //}

            return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
           
        }

        public byte[] ExportToExcel<T>(IEnumerable<T> datarows, string fields ="")
        {
                byte[] exportData;
                var orderedFields = fields.Split(";,".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

           
                using (var excelFile = new ExcelPackage())
                {
                    var worksheet = excelFile.Workbook.Worksheets.Add("Sheet1");
                    if (orderedFields.Length > 0)
                    {
                        worksheet.Cells["A1"].LoadFromCollection<T>(datarows, orderedFields, PrintHeaders: true, TableStyle: OfficeOpenXml.Table.TableStyles.Light1);
                    }
                    else
                    {
                        worksheet.Cells["A1"].LoadFromCollection<T>(datarows, PrintHeaders: true, TableStyle: OfficeOpenXml.Table.TableStyles.Light1);
                    }
                    
                    exportData = excelFile.GetAsByteArray();
                }
            
             return exportData;
        } 

        public Type ResolveType(string typeName)
        {
            var type = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(p => p.GetExportedTypes())
                .FirstOrDefault(p => p.Name == typeName || p.FullName == typeName);
            return type;

        }
        
    }
}