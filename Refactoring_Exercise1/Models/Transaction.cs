using Refactoring_Exercise1.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Refactoring_Exercise1.Models
{
    public class Transaction
    {
        public string TransactionId { get; set; }
        public Status Status { get; set; }
        public string CustomerId { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string AuthCode { get; internal set; }
        public string Reference { get; internal set; }
    }
}
