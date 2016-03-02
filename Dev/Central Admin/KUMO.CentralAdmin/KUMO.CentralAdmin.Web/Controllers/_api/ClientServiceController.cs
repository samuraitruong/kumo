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
    
    public class ClientServiceController : ApiController
    {
        // GET: api/ClientService
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        //[Route("_api/clientservice/{email}")]//

        [HttpGet]
        // GET: api/ClientService/5
        [Route("_api/clientservice/decrypt")]
        public string DecodeTest(string code)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();

            var publicKey = File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath("~/App_data/PrivateKey.xml"));
            rsa.FromXmlString(publicKey);
            rsa.PersistKeyInCsp = false;
            var data = rsa.Decrypt(Convert.FromBase64String(code), true);
            return System.Text.UTF8Encoding.UTF8.GetString(data);
            //return null;
        }
        public HttpResponseMessage Get(string email)
        {

            EIPUserService service = new EIPUserService();
            Client client = null;
            var connstring =  service.GetConnectionString(email, out client);
            if (client == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "");
            }
            var obj = new
            {
                ConnString = connstring,
                Name = client.ClientName,
                Id = client.Id,
                Components = client.ActiveComponents
            };

            return Request.CreateResponse(HttpStatusCode.OK, obj);
        }

        // POST: api/ClientService
        public HttpResponseMessage Post(string email)
        {
            EIPUserService service = new EIPUserService();
            Client client = null;

            var obj = new
            {
                ConnString = service.GetConnectionString(email, out client),
                Name = client.ClientName,
                Id = client.Id
            };

            return Request.CreateResponse(HttpStatusCode.OK, obj);
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
