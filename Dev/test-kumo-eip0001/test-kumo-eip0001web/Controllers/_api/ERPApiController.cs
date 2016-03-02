using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Web.Mvc;
using test_kumo_eip0001application;
using test_kumo_eip0001model;
namespace test_kumo_eip0001web.Controllers
{
    [AllowAnonymous]
    public class ERPApiController : System.Web.Http.ApiController
    {
        private CalendarService service = new CalendarService();

        [HttpPost]
        public IEnumerable<test_kumo_eip0001model.Calendar> ERPCalendarFull()
        {
            IEnumerable<test_kumo_eip0001model.Calendar> calendarItems = service.GetAll().Select(p => new { Id = p.Id, JobScope = p.JobScope, StartDate = p.StartDate, DueDate = p.DueDate }).ToList()
                .Select(p => new test_kumo_eip0001model.Calendar() { Id = p.Id, JobScope = p.JobScope, StartDate = p.StartDate, DueDate = p.DueDate });
            return calendarItems;
        }

    }
}
