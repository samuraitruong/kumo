
using Microsoft.Azure;
using Microsoft.WindowsAzure.Management.Sql;
using Microsoft.WindowsAzure.Management.Sql.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AzureAutomation
{
    class Program
    {
        //Can connect to the server then using command sql to create database, login

        //http://www.bradygaster.com/post/managing-windows-azure-sql-databases-using-the-management-libraries-for-net

        private static IEnumerable<string> SplitSqlStatements(string sqlScript)
        {
            // Split by "GO" statements
            var statements = Regex.Split(
                    sqlScript,
                    @"^\s*GO\s* ($ | \-\- .*$)",
                    RegexOptions.Multiline |
                    RegexOptions.IgnorePatternWhitespace |
                    RegexOptions.IgnoreCase);

            // Remove empties, trim, and return
            return statements
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim(' ', '\r', '\n'));
        }

        private static SqlManagementClient _sqlManagementClient;
        static void Main(string[] args)
        {
            //Run this command to get : Get-AzurePublishSettingsFile
            //https://manage.windowsazure.com/publishsettings/index?client=powershell

            var ManagementCertificate = @"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";

            string azureSubcriptionID = "95859d85-fb66-457d-a6ab-92eeeab60e75";
            //var ipaddress = (new WebClient()).DownloadString("http://checkip.dyndns.org").Split(':')[1];


            CreateSqlManagementClient(azureSubcriptionID, ManagementCertificate);
            var getServerResult = _sqlManagementClient.Servers.List();
            var ServerList = getServerResult.Servers;

            string newDBName = "test-azure-automation-2";
            foreach (var item in ServerList)
            {
                Console.WriteLine(item.Name +"   --- " + item.Location);
                var parameters = new DatabaseCreateParameters() {
                    Name = newDBName,
                    CollationName = "SQL_Latin1_General_CP1_CI_AS",
                    Edition = "Web" 
                };
               
                try
                {
                    _sqlManagementClient.Databases.Delete(item.Name, newDBName);
                    Console.WriteLine("Delete database : " + newDBName);
                }
                catch (Exception)
                {
                    
                    
                }
                
                _sqlManagementClient.Databases.Create(item.Name, parameters);
                Console.WriteLine("Database Created.");
                var listDatabaseResult =
               _sqlManagementClient.Databases.List(item.Name);
                var Databases = listDatabaseResult.Databases;

                foreach (var item1 in Databases)
                {
                    if (item1.Name == newDBName)
                    {
                        //string conn = "tcp:e4lfwb7eb4.database.windows.net,1433;User ID=test0001dbadmin;password=test0001db&dmin;Pooling=False";
                        //Exceute command
                        string server = string.Format("tcp:{0}.database.windows.net,1433", item.Name);

                        string sqlConnectionString = string.Format("Data Source={0};Initial Catalog={1};User ID=test0001dbadmin;password=test0001db&dmin", server, item1.Name);

                        FileInfo file = new FileInfo("deploy.sql");
                        string script = file.OpenText().ReadToEnd();
                        script = script.Replace("GO", string.Empty);

                        using (SqlConnection connection = new SqlConnection(sqlConnectionString))
                        {
                            SqlCommand command = new SqlCommand(script, connection);
                            connection.Open();
                            command.ExecuteNonQuery();

                        }


                    }
                    Console.WriteLine("\t " + item1.Name);
                }
                
            }

            


        }

        

        //http://www.tugberkugurlu.com/archive/windows-azure-management-client-libraries-fornet-and-it-supports-portable-class-library

        private static void CreateSqlManagementClient(string id, string certificate)
        {
            if (_sqlManagementClient != null)
                _sqlManagementClient.Dispose();

            var cert = new X509Certificate2(
                        Convert.FromBase64String(
                            certificate));

            SubscriptionCloudCredentials creds = 
		new CertificateCloudCredentials(id, cert);

            _sqlManagementClient = new SqlManagementClient(creds);

        }
    }
}

