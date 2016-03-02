using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Web;
using test_kumo_eip0001application;
using test_kumo_eip0001model;
using test_kumo_eip0001repositories.Model;

namespace test_kumo_eip0001web.Utility
{
    public class TenantHelper
    {
       

       public static bool AddEIPUser(string email, string firstname, string lastname, string role) {

           var serviceHost = ConfigurationManager.AppSettings["CAServiceHost"];
           string dbResolveUrl = "/_api/eipservice";

           var client = new RestClient(serviceHost);
           // client.Authenticator = new HttpBasicAuthenticator(username, password);

           var request = new RestRequest(dbResolveUrl, Method.POST);
           request.AddParameter("email", email);
           request.AddParameter("firstname", firstname);
           request.AddParameter("lastname", lastname);
           request.AddParameter("clientId", SessionManager.CurrentTenant.Id);
           request.AddHeader("Content-type", "application/json; charset=utf-8");
           //request.AddBody(request.JsonSerializer.Serialize(email));

           request.RequestFormat = DataFormat.Json;
      
           // execute the request
           var response = client.Execute(request);

           return response.StatusCode == System.Net.HttpStatusCode.OK;

           
       }
       public static string GetConnectionString(string email)
       {
           var serviceHost = ConfigurationManager.AppSettings["CAServiceHost"];
           string dbResolveUrl = "/_api/clientservice";

           var client = new RestClient(serviceHost);
           // client.Authenticator = new HttpBasicAuthenticator(username, password);

           var request = new RestRequest(dbResolveUrl, Method.GET);
           request.AddParameter("email", email);
           request.AddHeader("Content-type", "application/json; charset=utf-8");
           request.AddBody(request.JsonSerializer.Serialize(email));

           request.RequestFormat = DataFormat.Json;

           // execute the request
           var response = client.Execute<ClientInfo>(request);

           if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
           {
               return string.Empty;
           }
           //Decrypt connections tring here
           if (response.StatusCode == System.Net.HttpStatusCode.OK)
           {

               var content = response.Data.ConnString;
               //Save tenant info to session
               SessionManager.CurrentTenant = response.Data;
               //
               var resourceName = "test_kumo_eip0001web.PrivateKey.xml";

               using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
               using (StreamReader reader = new StreamReader(stream))
               {
                   string privateKey = reader.ReadToEnd();

                   RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();

                   //var publicKey = File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath("~/App_data/PublicKey.xml"));
                   rsa.FromXmlString(privateKey);
                   rsa.PersistKeyInCsp = false;
                   var bytes = Convert.FromBase64String(content);

                   var data = rsa.Decrypt(bytes, true);
                   //return BitConverter.ToString(data);
                   return System.Text.ASCIIEncoding.UTF8.GetString(data);
               }
           }
           else
           {
               throw new CAServiceException();
           }
       }


        internal static bool IsExistingUser(string email)
        {
            var serviceHost = ConfigurationManager.AppSettings["CAServiceHost"];
            string dbResolveUrl = "/_api/eipservice";

            var client = new RestClient(serviceHost);
            // client.Authenticator = new HttpBasicAuthenticator(username, password);

            var request = new RestRequest(dbResolveUrl, Method.GET);
            request.AddParameter("email", email);
           
            request.AddHeader("Content-type", "application/json; charset=utf-8");
            

            request.RequestFormat = DataFormat.Json;

            // execute the request
            var response = client.Execute(request);

            return response.StatusCode == System.Net.HttpStatusCode.Forbidden;
        }

        public static bool IsComponentAvailable(string componentName)
        {
            bool isValid = false;
            if (SessionManager.CurrentComponents != null && SessionManager.CurrentComponents.Count > 0)
            {
                var componentService = new ComponentService();
                Component currentComponent;
                if (SessionManager.Components == null || SessionManager.Components.Count <= 0)
                {
                    currentComponent = componentService.GetComponentByName(componentName);
                }
                else
                {
                    currentComponent = SessionManager.Components.FirstOrDefault(x => x.Name == componentName);
                }
                if (SessionManager.CurrentComponents.Contains(currentComponent.Id.ToString()))
                {
                    isValid = true;
                }
            }
            return isValid;
        }
    }
}