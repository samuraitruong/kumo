using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test_kumo_eip0001model.Repositories
{
    public interface IRepository<T> : IQueryableRepository<T> where T : class
    {
        void AddDeferred(params T[] items);
        void UpdateDeferred(params T[] items);
        void DeleteDeferred(params T[] items);
    }
}
