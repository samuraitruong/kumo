using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using test_kumo_eip0001repositories.DataModel;

namespace test_kumo_eip0001repositories
{
    public class ContextFactory : IContextFactory, IContextDisposer
    {
        private const string key = "DB_Context";
        private static Dictionary<int, EntityContext> threadContexts;

        public ContextFactory()
        {
            threadContexts = new Dictionary<int, EntityContext>();
        }

        private EntityContext EntityContext
        {
            get
            {
                if (HttpContext.Current == null)
                {
                    if (!threadContexts.ContainsKey(Thread.CurrentThread.ManagedThreadId))
                    {
                        return null;
                    }
                    else
                    {
                        return threadContexts[Thread.CurrentThread.ManagedThreadId];
                    }
                }

                return HttpContext.Current.Items[key] as EntityContext;
            }

            set
            {
                if (HttpContext.Current != null)
                    HttpContext.Current.Items[key] = value;
                else
                {
                    if (!threadContexts.ContainsKey(Thread.CurrentThread.ManagedThreadId))
                    {
                        threadContexts.Add(Thread.CurrentThread.ManagedThreadId, new EntityContext());
                    }
                }
            }
        }


        public EntityContext GetCurrent()
        {
            if (this.EntityContext == null)
                SetUp();

            if (this.EntityContext.Database.Connection.Database == "aspnet-WebApplication1-20150516105125")
            {
                EntityContext = EntityContext.CreateInstance();
            }

            return EntityContext;
        }

        public void SetUp()
        {
            CleanUp();
            EntityContext = EntityContext.CreateInstance(".", "test-kumo-eip0001db");
        }

        public void CleanUp()
        {
            if (EntityContext != null)
            {
                EntityContext.Dispose();
                EntityContext = null;
            }

            if (HttpContext.Current == null)
            {
                threadContexts.Remove(Thread.CurrentThread.ManagedThreadId);
            }

        }
    }
}
