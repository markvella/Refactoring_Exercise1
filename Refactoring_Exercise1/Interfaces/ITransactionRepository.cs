using Refactoring_Exercise1.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Refactoring_Exercise1.Interfaces
{
    public interface ITransactionRepository
    {
        Transaction Insert(string customerId, decimal amount);
        bool UpdateTransaction(Transaction transaction);
    }
}
