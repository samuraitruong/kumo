using KUMO.CentralAdmin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.Migrations;
using System.Threading.Tasks;

namespace KUMO.CentralAdmin.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly IContextFactory ContextFactory;

        protected EntityContext DB
        {
            get { return ContextFactory.GetCurrent(); }
        }

        public Repository()
        {
            this.ContextFactory = new ContextFactory();
        }

        public virtual IQueryable<T> GetAll()
        {
            return ContextFactory.GetCurrent().Set<T>();
        }

        public virtual IQueryable<T> GetAllAsNoTracking()
        {
            return ContextFactory.GetCurrent().Set<T>().AsNoTracking();
        }

        public virtual void DeleteDeferred(params T[] items)
        {
            ContextFactory.GetCurrent().Set<T>().RemoveRange(items.ToArray());
            CommitChanges();
        }

        public void CommitChanges()
        {
            ContextFactory.GetCurrent().SaveChanges();
        }


        public virtual void AddDeferred(params T[] items)
        {
            ContextFactory.GetCurrent().Set<T>().AddRange(items);
            CommitChanges();
        }

        public virtual void UpdateDeferred(params T[] items)
        {
            ContextFactory.GetCurrent().Set<T>().AddOrUpdate(items);
            CommitChanges();
        }
    }
}
