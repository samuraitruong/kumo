using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KUMO.CentralAdmin.Model
{
    public interface IRepository<T> : IQueryableRepository<T> where T : class
    {
        void AddDeferred(params T[] items);
        void UpdateDeferred(params T[] items);
        void DeleteDeferred(params T[] items);
    }
}
