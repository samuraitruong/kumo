using System;
using Microsoft.Practices.Unity;
using Kumo.Entities.Models;
using Kumo.Service;
using Repository.Pattern.DataContext;
using Repository.Pattern.Ef6;
using Repository.Pattern.Repositories;
using Repository.Pattern.UnitOfWork;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using Microsoft.Owin.Security;
using System.Web;

namespace Kumo.Web
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            container
                .RegisterType(typeof(UserManager<>),   new InjectionConstructor(typeof(IUserStore<>)))
                .RegisterType<IDataContextAsync, KumoContext>(new PerRequestLifetimeManager())
                .RegisterType<IUnitOfWorkAsync, UnitOfWork>(new PerRequestLifetimeManager())
                .RegisterType<IRepositoryAsync<Customer>, Repository<Customer>>()
                .RegisterType<IRepositoryAsync<Product>, Repository<Product>>()
                .RegisterType<IProductService, ProductService>()
                .RegisterType<ICustomerService, CustomerService>()
                .RegisterType<IKumoStoredProcedures, KumoContext>(new PerRequestLifetimeManager())
                .RegisterType<IStoredProcedureService, StoredProcedureService>();

            container.RegisterType<ApplicationRoleManager>(new HierarchicalLifetimeManager());

                container.RegisterType<ApplicationUserManager>(new HierarchicalLifetimeManager());

                container.RegisterType<IdentityUserRole, ApplicationUserRole>(new PerRequestLifetimeManager());

                container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>(new HierarchicalLifetimeManager());
                container.RegisterType<IRoleStore<ApplicationRole,string>, RoleStore<ApplicationRole>>(new HierarchicalLifetimeManager());
                container.RegisterType<DbContext, KumoContext>(new HierarchicalLifetimeManager());
                container.RegisterType<IAuthenticationManager>(new InjectionFactory(o => HttpContext.Current.GetOwinContext().Authentication));
        }
    }
}
