using Application.Booking;
using Application.Booking.DTO;
using Application.Payment.Dtos;
using Application.Payment.Ports;
using Application.Payment.Responses;
using Application.Room.Ports;
using Domain.Booking.Ports;
using Domain.Guest.Ports;
using Moq;

namespace ApplicationTest
{
    public class BookingManagerTest
    {
        [Test]
        public async Task Should_PayForABooking()
        {
            var dto = new PaymentRequestDto
            {
                SelectedPaymentProvider = SupportPaymentProviders.MercadoPAgo,
                PaymentIntention = "https://www.mercadopago.com.br./adsf",
                SelectedPaymentMethod = SupportPaymentMethods.CreditCard
            };

            var bookingRepository = new Mock<IBookingRepository>();
            var roomRepository = new Mock<IRoomRepository>();
            var guestRepository = new Mock<IGuestRepository>();
            var paymentProcessorFactory = new Mock<IPaymentProcessorFactory>();
            var paymentProcessor = new Mock<IPaymentProcessor>();

            var responseDto = new PaymentStateDto()
            {
                CreatedDate = DateTime.Now,
                Message = $"Succesfully paid {dto.PaymentIntention}",
                PaymentId = "123",
                Status = Status.Success
            };

            var response = new PaymentResponse()
            {
                Data = responseDto,
                Success = true,
                Message = "Payment Sucessfully processed"
            };

            paymentProcessor.Setup(x => x.CapturePayment(dto.PaymentIntention)).Returns(Task.FromResult(response));
            paymentProcessorFactory.Setup(x => x.GetPaymentProcessor(dto.SelectedPaymentProvider)).Returns(paymentProcessor.Object);

            var bookingManager = new BookingManager(
                bookingRepository.Object,
                roomRepository.Object,
                guestRepository.Object,
                paymentProcessorFactory.Object);

            var resp = await bookingManager.PayForABooking(dto);

            Assert.NotNull(resp);
            Assert.True(resp.Success);
        }
    }
}
