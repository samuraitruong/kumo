using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using test_kumo_eip0001repositories;
using System.Linq.Dynamic;
using PagedList;


namespace test_kumo_eip0001application
{
     
    public  class  ServiceBase<T> : IServiceBase<T> where T : class
    {
        internal Repository<T> repository;
        public ServiceBase()
        {
            repository = new Repository<T>();
        }
        

        public T GetById(int id, string entityKey="Id")
        {
            string where = entityKey +" = " + id.ToString();

            T result = repository.GetAll().Where(where).FirstOrDefault();
            return result;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="where">is the condition to query data ex: Id = 4</param>
        /// <returns></returns>
        public T GetByTerm(string where)
        {
          
            T result = repository.GetAll().Where(where).FirstOrDefault();
            return result;

        }

        public void Update(T entity)
        {
            repository.UpdateDeferred(new T[] { entity });
        }

        public void Update(T[] entities)
        {
            repository.UpdateDeferred(entities);
        }

        public void Delete(T entity)
        {
            repository.DeleteDeferred(new T[] { entity });
        }

        public void Delete(T[] entities)
        {
            repository.DeleteDeferred(entities);
        }

        public IQueryable<T> GetAll()
        {
            return repository.GetAll();
        }
        public void Add(T entity)
        {
            repository.AddDeferred(new T[] { entity });
        }

        public void Add(T[] entities)
        {
            repository.AddDeferred(entities);
        }

        public IPagedList<T> GetPaged(int page = 1, int pageSize = 20, string where = "Id > 0", string orderBy = "Id asc")
        {
            return repository.GetAll()
                .Where(where)
                .OrderBy(orderBy)
                .ToPagedList(page, pageSize);
        }

    }
}
