using Refactoring_Exercise1.Enums;
using Refactoring_Exercise1.Interfaces;
using Refactoring_Exercise1.Models;
using System;
using System.Text.RegularExpressions;

namespace Refactoring_Exercise1
{
    public class Payment
    {
        private readonly ICustomerService _customer;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICardPaymentService _cardPaymentService;

        public Payment(
            ICustomerService customer,
            ITransactionRepository transactionRepository,
            ICardPaymentService cardPaymentService)
        {
            _customer = customer;
            _transactionRepository = transactionRepository;
            _cardPaymentService = cardPaymentService;
        }
        public DepositReponse Deposit(
            string customerId,
            string cardHolderName,
            string cardNumber,
            string expiry,
            decimal amount)
        {
            var response = new DepositReponse
            {
                IsSuccessful = false
            };

            if (string.IsNullOrWhiteSpace(customerId))
            {
                return new DepositReponse
                {
                    IsSuccessful = false,
                    Error = "customerId Invalid"
                };
            }

            var customer = _customer.GetCustomer(customerId);
            if (customer == null)
            {
                return new DepositReponse
                {
                    IsSuccessful = false,
                    Error = "Customer not found"
                };
            }

            if (string.IsNullOrWhiteSpace(cardHolderName))
            {
                return new DepositReponse
                {
                    IsSuccessful = false,
                    Error = "cardHolderName Invalid"
                };
            }

            if (string.IsNullOrWhiteSpace(cardNumber))
            {
                return new DepositReponse
                {
                    IsSuccessful = false,
                    Error = "cardNumber Invalid"
                };
            }

            if (string.IsNullOrWhiteSpace(expiry) ||
                !Regex.IsMatch(expiry, @"^\d{2}\/\d{2}$"))
            {
                return new DepositReponse
                {
                    IsSuccessful = false,
                    Error = "expiry Invalid"
                };
            }

            if (amount <= 0)
            {
                return new DepositReponse
                {
                    IsSuccessful = false,
                    Error = "amount Invalid"
                };
            }

            Transaction transaction = _transactionRepository.Insert(customerId, amount);
            if (transaction.Status != Status.Created)
            {
                response.Error = "Transaction creation failed";
                return response;
            }

            response.TransactionId = transaction.TransactionId;

            var authCode = string.Empty;
            try
            {
                authCode = _cardPaymentService.Authorize(cardHolderName, cardNumber, expiry);
            }
            catch (Exception)
            {
                response.Error = "Authorize exception";
                return response;
            }

            transaction.AuthCode = authCode;
            transaction.Status = Status.Pending;

            if (!_transactionRepository.UpdateTransaction(transaction))
            {
                response.Error = "Transaction update failed";
                return response;
            }

            var reference = string.Empty;
            try
            {
                reference = _cardPaymentService.Execute(authCode);
            }
            catch (Exception)
            {
                response.Error = "Execute exception";
                return response;
            }

            transaction.Reference = reference;
            transaction.Status = Status.Approved;

            if (!_transactionRepository.UpdateTransaction(transaction))
            {
                response.Error = "Transaction update failed";
                return response;
            }

            response.IsSuccessful = true;
            return response;
        }
    }
}
