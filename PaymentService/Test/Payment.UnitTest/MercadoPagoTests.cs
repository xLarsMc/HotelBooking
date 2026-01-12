using Application;
using Application.Booking.DTO;
using Payment.Application.MercadoPago;
using Payments.Application;

namespace Payment.UnitTest
{
    public class MercadoPagoTests
    {
        [Test]
        public void ShouldReturn_MercadoPagoAdapter_Provider()
        {
            var factory = new PaymentProcessorFactory();

            var provider = factory.GetPaymentProcessor(SupportPaymentProviders.MercadoPAgo);

            Assert.AreEqual(provider.GetType(), typeof(MercadoPagoAdapter));
        }

        [Test]
        public async Task Should_FailWhenPaymentIntentionStringIsValid()
        {
            var factory = new PaymentProcessorFactory();

            var provider = factory.GetPaymentProcessor(SupportPaymentProviders.MercadoPAgo);

            var res = await provider.CapturePayment("");

            Assert.False(res.Success);
            Assert.AreEqual(res.ErrorCode, ErrorCodes.PAYMENT_INVALID_PAYMENT_INTENTION);
        }

        [Test]
        public async Task Should_SuccessfullyProcessPayment()
        {
            var factory = new PaymentProcessorFactory();

            var provider = factory.GetPaymentProcessor(SupportPaymentProviders.MercadoPAgo);

            var res = await provider.CapturePayment("https://mercadopago.com.br/asdf");

            Assert.IsTrue(res.Success);
            Assert.IsNotNull(res.Data);
            Assert.IsNotNull(res.Data.CreatedDate);
            Assert.IsNotNull(res.Data.PaymentId);
        }

    }
}
