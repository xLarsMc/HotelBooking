using Application.Booking.DTO;

namespace Application.Payment.Ports
{
    public interface IPaymentProcessorFactory
    {
        IPaymentProcessor GetPaymentProcessor(SupportPaymentProviders supportPaymentProviders);
    }
}
