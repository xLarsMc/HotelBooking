using Application;
using Application.Booking.DTO;
using Payment.Application.MercadoPago;
using Payments.Application;

namespace Payment.UnitTest
{
    public class Tests
    {
        [Test]
        public void ShouldReturn_NotImplementedPaymentProvider_WhenAskingForStripeProvider()
        {
            var factory = new PaymentProcessorFactory();

            var provider = factory.GetPaymentProcessor(SupportPaymentProviders.Stripe);

            Assert.AreEqual(provider.GetType(), typeof(NotImplementedPaymentProvider));
        }

        [Test]
        public void ShouldReturn_MercadoPagoAdapter_Provider()
        {
            var factory = new PaymentProcessorFactory();

            var provider = factory.GetPaymentProcessor(SupportPaymentProviders.MercadoPAgo);

            Assert.AreEqual(provider.GetType(), typeof(MercadoPagoAdapter));
        }

        [Test]
        public async Task ShouldReturnFalse_WhenCapturingPaymentFor_NotImplementedPaymentProvider()
        {
            var factory = new PaymentProcessorFactory();

            var provider = factory.GetPaymentProcessor(SupportPaymentProviders.Stripe);

            var response = await provider.CapturePayment("https://myprovider.com/asdf");

            Assert.False(response.Success);
            Assert.AreEqual(response.ErrorCode, ErrorCodes.PAYMENT_PROVIDER_NOT_IMPLEMENTED);
            Assert.AreEqual(response.Message, "The selected payment provider is not avaliable at the moment");
        }
    }
}