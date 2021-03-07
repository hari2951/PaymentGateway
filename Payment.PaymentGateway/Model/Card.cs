using Payment.PaymentGateway.Enums;

namespace Payment.PaymentGateway.Model
{
    public class Card
    {
        public string CardNumber { get; set; }
        public CardType CardType { get; set; }
        public string Cvv { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
    }
}
