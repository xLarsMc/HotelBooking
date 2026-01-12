namespace Application.Booking.DTO
{
    public enum SupportPaymentProviders
    {
        PayPal = 1,
        Stripe = 2,
        PagSeguro = 3,
        MercadoPAgo = 4,
    }

    public enum SupportPaymentMethods
    {
        DebitCard = 1,
        CreditCard = 2,
        BankTransfer = 3,
    }
    public class PaymentRequestDto
    {
        public int BookingId { get; set; }
        public string PaymentIntention { get; set; }
        public SupportPaymentProviders SelectedPaymentProvider { get; set; }
        public SupportPaymentMethods SelectedPaymentMethod { get; set; }
    }
}
