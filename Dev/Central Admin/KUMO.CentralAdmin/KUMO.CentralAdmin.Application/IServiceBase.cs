using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KUMO.CentralAdmin.Application
{
    public interface IServiceBase<T> where T : class
    {
        T GetById(int id, string entityKey = "Id");
        void Update(T entity);
        void Delete(T entity);
    }
}
