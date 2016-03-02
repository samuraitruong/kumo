using System;
using System.Collections.Generic;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace test_kumo_eip0001repositories.DataModel
{
    public class EntityContext : Entities
    {
        public EntityContext()
            : base(GetTenantConnectionString())
        {
            Configuration.LazyLoadingEnabled = true;
            Configuration.ProxyCreationEnabled = true;

        }

        public EntityContext(string connectionString)
            : base(GetTenantConnectionString())
        {
            Configuration.LazyLoadingEnabled = true;
            Configuration.ProxyCreationEnabled = true;

        }

        private static string GetTenantConnectionString()
        {
            if (HttpContext.Current != null && HttpContext.Current.Session["TENANT_CONNECTION_STRING_NAME"] != null)
            {
                string conc = HttpContext.Current.Session["TENANT_CONNECTION_STRING_NAME"] as string;

                return string.Format(@"metadata=res://*/DataModel.test-kumo-eip0001.csdl|res://*/DataModel.test-kumo-eip0001.ssdl|res://*/DataModel.test-kumo-eip0001.msl;provider=System.Data.SqlClient;provider connection string=""{0};MultipleActiveResultSets=True;App=EntityFramework""", conc);

            }
            throw new InvalidTenantException();


        }

        public static EntityContext CreateInstance(string host, string catalog)
        {
            return new EntityContext();
        }

        public static EntityContext CreateInstance()
        {
            return new EntityContext(GetTenantConnectionString());
        }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }
    }
}
