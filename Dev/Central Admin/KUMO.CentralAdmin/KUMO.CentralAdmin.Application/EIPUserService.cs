using KUMO.CentralAdmin.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace KUMO.CentralAdmin.Application
{
    public class EIPUserService : ServiceBase<EIPUser>
    {
        private  string BuildConnectionString(string dbname, string serverName, string username = "", string password = "")
        {
            string server = string.Format("tcp:{0}.database.windows.net,1433", serverName.Split('.')[0]);
            string user = !string.IsNullOrEmpty(username) ? string.Format("{0}@{1}", username, serverName.Split('.')[0]) : string.Empty;
            //TODO : add master username and password to configuration.
            string sqlConnectionString = string.Format("Data Source={0};Initial Catalog={1};User ID={2};password={3}", server, dbname, user, password);

            return sqlConnectionString;

        }

        public string GetConnectionString(string email, out Client client)
        {
            client = null;
            ClientService clientservice = new ClientService();
            EIPUserService service = new EIPUserService();
            var eip = service.GetAll().FirstOrDefault(p => p.Email == email);
            string connectionString = "";
            if (eip != null)
            {
                client = eip.Client;

                connectionString = BuildConnectionString(eip.Client.DBName, eip.Client.DBServer, eip.Client.DBUser, eip.Client.DBPassword);

            }
            else
            {
                return string.Empty;
            }

            
            var resourceName = "KUMO.CentralAdmin.Application.PublicKey.xml";

            using (Stream stream = this.GetType().Assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string publicKey = reader.ReadToEnd();

                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();

                //var publicKey = File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath("~/App_data/PublicKey.xml"));
                rsa.FromXmlString(publicKey);
                rsa.PersistKeyInCsp = false;

                var data = rsa.Encrypt(System.Text.UTF8Encoding.UTF8.GetBytes(connectionString), true);
                //return BitConverter.ToString(data);
                return Convert.ToBase64String(data);

            }

        }
    }
}
