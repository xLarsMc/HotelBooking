using Application;
using Application.Booking.Commands;
using Application.Booking.DTO;
using Application.Booking.Ports;
using Application.Booking.Queries;
using Application.Payment.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly ILogger<BookingController> _logger;
        private readonly IBookingManager _bookingManager;
        private readonly IMediator _mediator;
        public BookingController(ILogger<BookingController> logger, IBookingManager bookingManager, IMediator mediator)
        {
            _logger = logger;
            _bookingManager = bookingManager;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<BookingDto>> Post(BookingDto booking)
        {
            var coomand = new CreateBookingCommand
            {
                BookingDto = booking,
            };

            var res = await _mediator.Send(coomand);

            if (res.Success) return Created("", res.Data);

            else if (res.ErrorCode == ErrorCodes.ROOM_MISSING_REQUIRED_INFORMATION) return NotFound(res);

            else if (res.ErrorCode == ErrorCodes.ROOM_COULD_NOT_STORE_DATA) return BadRequest(res);

            _logger.LogError("Response with unknow ErrorCode Returned", res);

            return BadRequest(500);
        }

        [HttpPost]
        [Route("{bookingId}/Pay")]
        public async Task<ActionResult<PaymentResponse>> Pay(PaymentRequestDto paymentRequestDto, int bookingId)
        {
            paymentRequestDto.BookingId = bookingId;
            var response = await _bookingManager.PayForABooking(paymentRequestDto);

            if (response.Success) return Ok(response.Data);

            return BadRequest(response);
        }

        [HttpGet]
        public async Task<ActionResult<BookingDto>> Get(int id)
        {
            var query = new GetBookingQuery
            {
                Id = id
            };

            var response = await _mediator.Send(query);

            if (response.Success) return Created("", response.Data);

            _logger.LogError("Response with unknow ErrorCode Returned", response);
            return BadRequest(response);
        }
    }
}
