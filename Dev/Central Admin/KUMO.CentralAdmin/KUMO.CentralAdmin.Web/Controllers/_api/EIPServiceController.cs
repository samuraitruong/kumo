using KUMO.CentralAdmin.Application;
using KUMO.CentralAdmin.Model;
using KUMO.CentralAdmin.Web.Utilities;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Web.Http;

namespace KUMO.CentralAdmin.Web.Controllers._api
{
    
    public class EIPServiceController : ApiController
    {
        // GET: api/ClientService
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet]
        public HttpResponseMessage Get(string email)
        {
            EIPUserService service = new EIPUserService();
            var user = service.GetAll().FirstOrDefault(p => p.Email == email);
            if (user != null) return Request.CreateResponse(HttpStatusCode.Forbidden, "");


            return Request.CreateResponse(HttpStatusCode.OK, "");


            //return string.Empty;
            
        }

        // POST: api/ClientService
        [HttpPost]
        public HttpResponseMessage Post(EIPUser newUser)
        {
            EIPUserService service = new EIPUserService();
            service.Add(newUser);


            return Request.CreateResponse(HttpStatusCode.OK, newUser);
        }

        // PUT: api/ClientService/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ClientService/5
        public void Delete(int id)
        {
        }
    }
}
