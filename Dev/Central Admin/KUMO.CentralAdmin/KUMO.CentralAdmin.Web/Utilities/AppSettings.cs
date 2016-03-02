using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;

namespace KUMO.CentralAdmin.Web.Utilities
{
    public static class AppSettings
    {

        public static string ManagementCertificate
        {
            get
            {
                return Setting<string>("ManagementCertificate");
            }
        }

        public static string SubscriptionId
        {
            get
            {
                return Setting<string>("SubscriptionId");
            }
        }

        public static string MasterDBUser
        {
            get
            {
                return Setting<string>("MasterDBUser");
            }
        }

        public static string MasterDBPassword
        {
            get
            {
                return Setting<string>("MasterDBPassword");
            }
        }

        public static int DefaultDBSizeMB
        {
            get
            {
                return Setting<int>("DefaultDBSizeMB");
            }
        }

        public static string DefaultDBEdition
        {
            get
            {
                return Setting<string>("DefaultDBEdition");
            }
        }


        public static string EIPWebUrl
        {
            get
            {
                return Setting<string>("EIPWebUrl");
            }
        }

        public static string DBServer
        {
            get
            {
                return Setting<string>("DBServer");
            }
        }

        public static string CertificatePath
        {
            get
            {
                return Setting<string>("CertificatePath");
            }
        }

        public static string CertificatePassword
        {
            get
            {
                return Setting<string>("CertificatePassword");
            }
        }

        private static T Setting<T>(string name)
        {
            string value = ConfigurationManager.AppSettings[name];

            if (value == null)
            {
                throw new Exception(String.Format("Could not find setting '{0}',", name));
            }

            return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
        }
    }
}