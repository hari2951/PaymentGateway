using Payment.PaymentGateway.Enums;
using System;

namespace Payment.PaymentGateway.Model.Payments
{
    public interface IPaymentRequest
    {
        public Guid ClientToken { get; set; }
        public Guid MerchantKey { get; set; }
        public string Description { get; set; }
        public string CardHolderName { get; set; }
        public string CardNumber { get; set; }
        public CardType CardType { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public string Cvv { get; set; }
        public decimal Amount { get; set; }
        public CurrencyCode Currency{ get; set; }
        public bool IsTest { get; set; }
        public string ReturnURL { get; set; }
        public string ErrorURL { get; set; }

    }
}
