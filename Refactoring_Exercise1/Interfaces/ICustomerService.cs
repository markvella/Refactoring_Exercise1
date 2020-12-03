using Refactoring_Exercise1.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Refactoring_Exercise1.Interfaces
{
    public interface ICustomerService
    {
        Customer GetCustomer(string customerId);
    }
}
