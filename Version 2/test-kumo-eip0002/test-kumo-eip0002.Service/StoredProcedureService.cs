using System.Collections.Generic;
using Kumo.Entities.Models;

namespace Kumo.Service
{
    public class StoredProcedureService : IStoredProcedureService
    {
        private readonly IKumoStoredProcedures _storedProcedures;

        public StoredProcedureService(IKumoStoredProcedures storedProcedures)
        {
            _storedProcedures = storedProcedures;
        }

        public IEnumerable<CustomerOrderHistory> CustomerOrderHistory(string customerID)
        {
            return _storedProcedures.CustomerOrderHistory(customerID);
        }

        public int CustOrdersDetail(int? orderID)
        {
            return _storedProcedures.CustOrdersDetail(orderID);
        }

        public IEnumerable<CustomerOrderDetail> CustomerOrderDetail(string customerID)
        {
            return _storedProcedures.CustomerOrderDetail(customerID);
        }
    }
}