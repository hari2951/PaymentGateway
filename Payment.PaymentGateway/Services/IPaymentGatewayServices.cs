using Payment.PaymentGateway.Model.Payments;

namespace Payment.PaymentGateway.Services
{
    public interface IPaymentGatewayServices
    {
        IPaymentResponse SubmitPaymentRequestFor(IPaymentRequest paymentRequest);
    }
}
