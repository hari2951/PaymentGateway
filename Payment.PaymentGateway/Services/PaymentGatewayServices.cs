using Payment.External.V1;
using Payment.PaymentGateway.Model.Payments;

namespace Payment.PaymentGateway.Services
{
    public class PaymentGatewayServices : IPaymentGatewayServices
    {
        private readonly IPaymentService _paymentService;

        public PaymentGatewayServices(IPaymentService paymentService)
        {
            this._paymentService = paymentService;
        }
        public IPaymentResponse SubmitPaymentRequestFor(IPaymentRequest paymentRequest)
        {

            return new PaymentResponse();
        }
    }
}
