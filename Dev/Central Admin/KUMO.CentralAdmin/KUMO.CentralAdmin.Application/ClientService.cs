using KUMO.CentralAdmin.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KUMO.CentralAdmin.Application
{
    public class ClientService :ServiceBase<Client>
    {
        public void ValidateTenantDatabase()
        {
            //var file = File.AppendText("d:\\timer_log.txt");

            //file.WriteLine("ValidateTenantDatabase --- " + System.DateTime.Now.ToLongTimeString());
            

           var allclient = GetAll().Where(p => p.Status == ClientStatus.Deploying.ToString()).ToList();

            foreach (var item in allclient)
            {
                if (IsActiveClient(item))
                {
                    //file.WriteLine("Log : Tenant database up and running.....");
                    item.Status = ClientStatus.Deployed.ToString();
                    Update(item);
                    //Send notification email
                }
                else
                {
                    //file.WriteLine("Log : Tenant database is being deloyed.....");
                }
            }
            //file.Close();
        }

        
        private bool IsActiveClient(Client item)
        {
            string sqlConnectionString = string.Format("data source=tcp:{0};initial catalog={1};integrated security=false;User ID={2};password={3}", item.DBServer, item.DBName, item.DBUser, ConvertToConnectionStringValue(item.DBPassword));
            try
            {
                using (SqlConnection connection = new SqlConnection(sqlConnectionString))
                {
                    //SqlCommand command = new SqlCommand(script, connection);
                    connection.Open();
                    connection.Close();
                    //Verify schema to confirm db is created correctly
                    return true;
                    //command.ExecuteNonQuery();

                }
            }
            catch (Exception ex)
            {
                
                return false;
            }
           
        }

        private object ConvertToConnectionStringValue(string password)
        {
            password = password.Replace("=", "==")
                                .Replace(";", @""";");
            return password;
        }
    }
}
