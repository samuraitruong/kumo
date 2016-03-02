using System;
using System.Collections.Generic;

namespace Kumo.Entities.Models
{
public interface IKumoStoredProcedures
{
    IEnumerable<CustomerOrderHistory> CustomerOrderHistory(string customerID);
    int CustOrdersDetail(int? orderID);
    IEnumerable<CustomerOrderDetail> CustomerOrderDetail(string customerID);
    int EmployeeSalesByCountry(DateTime? beginningDate, DateTime? endingDate);
    int SalesByCategory(string categoryName, string ordYear);
    int SalesByYear(DateTime? beginningDate, DateTime? endingDate);
}
}