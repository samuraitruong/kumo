using Microsoft.Azure;
using Microsoft.WindowsAzure.Management.Sql;
using Microsoft.WindowsAzure.Management.Sql.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace KUMO.CentralAdmin.Web.Utilities
{
    //http://kitsula.com/Article/How-to-create-custom-user-login-for-Azure-SQL-Database

    public class AzureSQLHelper
    {
        private static string MANAGEMENT_CERTIFICATE = AppSettings.ManagementCertificate;

        private static string SUBCRIPTION_ID = AppSettings.SubscriptionId;

        public static string MASTER_DB_USER = AppSettings.MasterDBUser;

        public static string MASTER_DB_USER_PASSWORD = AppSettings.MasterDBPassword;

        private static SqlManagementClient _sqlManagementClient;

        public static string BuildConnectionString(string dbname, string serverName, string username = "", string password = "")
        {
            string server = string.Format("tcp:{0}.database.windows.net,1433", serverName.Split('.')[0]);
            //TODO : add master username and password to configuration.
            string sqlConnectionString = string.Format("Data Source={0};Initial Catalog={1};User ID={2};password={3}", server, dbname, username, password);

            return sqlConnectionString;

        }
        public static List<string> GetListServers(string certPath, string certPass)
        {
            CreateSqlManagementClient(SUBCRIPTION_ID, certPath, certPass);

            var getServerResult = _sqlManagementClient.Servers.List();
            var ServerList = getServerResult.Servers;

            return ServerList.AsEnumerable().Select(p => p.FullyQualifiedDomainName).ToList();



        }
        public static bool CreateDB(string dbname, string serverName, string certPath, string certPass,
            bool dropExisting = false, string username = "", string password = "")
        {
            //
            string serverNameWithoutDomain = serverName.Split('.')[0];

            CreateSqlManagementClient(SUBCRIPTION_ID, certPath, certPass);

            var sqlServer = FindServer(serverNameWithoutDomain);
            if (sqlServer != null)
            {
                CreateDatabaseInternal(dbname, sqlServer, AppSettings.DefaultDBEdition, AppSettings.DefaultDBSizeMB, dropExisting);
            }
            return true;

        }

        public static bool DeleteDB(string dbname, string serverName, string certPath, string certPass)
        {
            //
            string serverNameWithoutDomain = serverName.Split('.')[0];

            CreateSqlManagementClient(SUBCRIPTION_ID, certPath, certPass);

            var sqlServer = FindServer(serverNameWithoutDomain);
            if (sqlServer != null)
            {
                try
                {
                    ExecuteSQL(dbname, sqlServer.Name, "DELETE FROM [AspNetUsers]");
                    _sqlManagementClient.Databases.Delete(sqlServer.Name, dbname);
                }
                catch (Exception ex)
                {
                }
            }
            return true;

        }

        public static bool CreateLoginUser(string dbname, string azureserver, string username, 
            string password, string certPath, string certPass)
        {

            
            CreateSqlManagementClient(SUBCRIPTION_ID, certPath, certPass);
            string sql = string.Format("DROP USER {0}", username);
            ExecuteSQL("master", azureserver, sql);

             sql = string.Format("DROP LOGIN {0}", username);
            ExecuteSQL("master", azureserver, sql);

            sql = string.Format("CREATE LOGIN {0} WITH password='{1}'", username, password);

            ExecuteSQL("master", azureserver, sql);

            sql = string.Format("CREATE USER [{0}] FOR LOGIN [{0}] WITH DEFAULT_SCHEMA=[dbo]", username);

            ExecuteSQL("master", azureserver, sql);

            ExecuteSQL(dbname, azureserver, sql);

            sql = string.Format("EXEC sp_addrolemember 'db_owner', '{0}';", username);

            ExecuteSQL(dbname, azureserver, sql);

            return true;

        }     
        public static bool ExecuteSQL(string dbname, string azureserver, string sql)
        {

            string server = string.Format("tcp:{0}.database.windows.net,1433", azureserver.Split('.')[0]);
            //TODO : add master username and password to configuration.
            string sqlConnectionString = string.Format("Data Source={0};Initial Catalog={1};User ID={2};password={3}", server, dbname, MASTER_DB_USER, MASTER_DB_USER_PASSWORD);

            //FileInfo file = new FileInfo("deploy.sql");
            string script = sql;

            script = script.Replace("GO", string.Empty);
            
            using (SqlConnection connection = new SqlConnection(sqlConnectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(script, connection);
                    connection.Open();
                    command.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    return false;
                    
                }
                finally{
                    connection.Close();

                }
               
            }

            return true;

        }

        private static bool CreateDatabaseInternal(string dbname, Server sqlServer, string edition="Web", int size=100 , bool dropExisting = false)
        {
            //Create database
            var parameters = new DatabaseCreateParameters()
            {
                Name = dbname,
                CollationName = "SQL_Latin1_General_CP1_CI_AS",
                Edition = edition,
                MaximumDatabaseSizeInBytes = 1024*1024*size
            };

            try
            {
                if (dropExisting)
                {
                    _sqlManagementClient.Databases.Delete(sqlServer.Name, dbname);

                }//Console.WriteLine("Delete database : " + newDBName);
            }
            catch (Exception)
            {


            }
            
            _sqlManagementClient.Databases.Create(sqlServer.Name, parameters);
            
            
            return true;
            
        }
        private static Microsoft.WindowsAzure.Management.Sql.Models.Server  FindServer(string servername)
        {
             var getServerResult = _sqlManagementClient.Servers.List();
            var ServerList = getServerResult.Servers;

            
            foreach (var item in ServerList)
            {
                if (servername == item.Name.ToLower())
                {
                    return item;
                }
            }
            return null;
        }

        private static void CreateSqlManagementClient(string id, string certPath, string certPass)
        {
            if (_sqlManagementClient != null)
                _sqlManagementClient.Dispose();

            var cert = new X509Certificate2(certPath, certPass);

            SubscriptionCloudCredentials creds =
        new CertificateCloudCredentials(id, cert);

            _sqlManagementClient = new SqlManagementClient(creds);

        }


        internal static void CreateDBLogin(string dbname, string server, string user, string password)
        {
            ExecuteSQL("master", server, string.Format("CREATE LOGIN {0} WITH password='{1}';", user, password));
            string command =    string.Format("CREATE USER {0} FROM LOGIN {1};\n", user) +
                                string.Format("EXEC sp_addrolemember N'db_owner', N'{0}'", user);
            ExecuteSQL(dbname, server, command);

            
        }
    }
}