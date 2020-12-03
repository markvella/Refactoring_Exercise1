using System;
using System.Collections.Generic;
using System.Text;

namespace Refactoring_Exercise1.Models
{
    public class DepositReponse
    {
        public bool IsSuccessful { get; set; }
        public string Error { get; set; }
        public string TransactionId { get; set; }
    }
}
