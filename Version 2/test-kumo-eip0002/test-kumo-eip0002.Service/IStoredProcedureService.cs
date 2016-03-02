#region

using System.Collections.Generic;
using Kumo.Entities.Models;

#endregion

namespace Kumo.Service
{
    public interface IStoredProcedureService
    {
        IEnumerable<CustomerOrderHistory> CustomerOrderHistory(string customerID);
        int CustOrdersDetail(int? orderID);
        IEnumerable<CustomerOrderDetail> CustomerOrderDetail(string customerID);
    }
}