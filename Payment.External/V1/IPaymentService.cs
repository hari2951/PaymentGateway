using System;
using System.Collections.Generic;
using System.Text;

namespace Payment.External.V1
{
    public interface IPaymentService
    {
        IPaymentServiceResult RequestPayment(PaymentServiceRequest paymentServiceRequest);
    }
}
