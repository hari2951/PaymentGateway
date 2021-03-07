using Payment.PaymentGateway.Enums;
using System;

namespace Payment.PaymentGateway.Model.Payments
{
    public interface IPaymentResponse
    {
        public Guid MerchantKey { get; set; }
        public Guid ClientToken { get; set; }
        public Guid TransactionId { get; set; }
        public string Message { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
    }
}
