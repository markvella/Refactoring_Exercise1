using System;
using System.Collections.Generic;
using System.Text;

namespace Refactoring_Exercise1.Interfaces
{
    public interface ICardPaymentService
    {
        string Authorize(string cardHolderName, string cardNumber, string expiry);
        string Execute(string authCode);
    }
}
