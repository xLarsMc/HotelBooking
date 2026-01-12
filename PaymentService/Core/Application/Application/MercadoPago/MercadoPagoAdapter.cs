using Application;
using Application.MercadoPago.Exceptions;
using Application.Payment;
using Application.Payment.Dtos;
using Application.Payment.Ports;
using Application.Payment.Responses;

namespace Payment.Application.MercadoPago
{
    public class MercadoPagoAdapter : IPaymentProcessor
    {
        public Task<PaymentResponse> CapturePayment(string paymentIntention)
        {
            try
            {
                if (string.IsNullOrEmpty(paymentIntention)) throw new InvalidPaymentIntentionException();

                paymentIntention += "/success";

                var dto = new PaymentStateDto()
                {
                    CreatedDate = DateTime.Now,
                    Message = "Successfully Paid " + paymentIntention,
                    PaymentId = "123",
                    Status = Status.Success
                };

                var response = new PaymentResponse()
                {
                    Data = dto,
                    Success = true,
                    Message = "Payment Sucessfully processed"
                };

                return Task.FromResult(response);
            }
            catch (InvalidPaymentIntentionException)
            {
                var resp = new PaymentResponse()
                {
                    Success = false,
                    ErrorCode = ErrorCodes.PAYMENT_INVALID_PAYMENT_INTENTION,
                };

                return Task.FromResult(resp);
            }
        }
    }
}
