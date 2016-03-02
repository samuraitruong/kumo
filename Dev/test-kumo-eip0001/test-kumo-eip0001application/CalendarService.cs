using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using test_kumo_eip0001model;
using test_kumo_eip0001model.Resources;
using test_kumo_eip0001repositories;

namespace test_kumo_eip0001application
{
    public class CalendarService : ServiceBase<Calendar>
    {
        public CalendarService()
        {
        }


        public IEnumerable<object> GetJobStatus()
        {
            return new[] { 
                new  {Name= ERPResource.Status_NotStarted, Id= ERPResource.Status_NotStarted},
                new  {Name= ERPResource.Status_InProgress, Id= ERPResource.Status_InProgress},
                new  {Name= ERPResource.Status_Completed, Id= ERPResource.Status_Completed},
                new  {Name= ERPResource.Status_Deferred, Id= ERPResource.Status_Deferred},
                new  {Name= ERPResource.Status_Late, Id= ERPResource.Status_Late}
            };
        }

        public IEnumerable<object> GetPriorityStatus()
        {
            return new[] { 
                new  {Name= ERPResource.Priority_High, Id= ERPResource.Priority_High},
                new  {Name= ERPResource.Priority_Medium, Id= ERPResource.Priority_Medium},
                new  {Name= ERPResource.Priority_Low, Id= ERPResource.Priority_Low}
            };
        }
    }
}
