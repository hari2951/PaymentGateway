using Payment.PaymentGateway.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payment.External
{
    public interface IPaymentServiceResult
    {
        public Guid MerchantKey { get; set; }
        public Guid ClientToken { get; set; }
        public Guid TransactionId { get; set; }
        public string Message { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
    }
}
