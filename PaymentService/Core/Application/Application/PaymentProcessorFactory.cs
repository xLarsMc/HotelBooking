using Application.Booking.DTO;
using Application.Payment.Ports;
using Payment.Application.MercadoPago;

namespace Payments.Application
{
    public class PaymentProcessorFactory : IPaymentProcessorFactory
    {
        public IPaymentProcessor GetPaymentProcessor(SupportPaymentProviders supportPaymentProviders)
        {
            switch (supportPaymentProviders)
            {
                case SupportPaymentProviders.MercadoPAgo:
                    return new MercadoPagoAdapter();

                default: return new NotImplementedPaymentProvider();
            }
        }
    }
}
