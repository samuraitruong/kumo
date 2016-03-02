using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test_kumo_eip0001model.Repositories
{
    public interface IQueryableRepository<out T> where T : class
    {
        /// <summary>
        /// Combine with a query filter to filter result
        /// <para>Example: GetAll().Where(x => x.Status != IdeaStatus.Deleted);</para>
        /// <para>Example: GetAll().SingleOrDefault(x => x.Id ==10);</para>
        /// </summary>
        IQueryable<T> GetAll();

        IQueryable<T> GetAllAsNoTracking();
    }
}
