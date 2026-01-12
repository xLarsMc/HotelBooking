using Application;
using Application.Room.Commands;
using Application.Room.Dtos;
using Application.Room.Ports;
using Application.Room.Queries;
using Application.Room.Request;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoomController : ControllerBase
    {
        private readonly ILogger<RoomController> _logger;
        private readonly IRoomRepository _roomManager;
        private readonly IMediator _mediator;

        public RoomController(ILogger<RoomController> logger, IRoomRepository ports, IMediator mediator)
        {
            _logger = logger;
            _roomManager = ports;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<RoomDto>> Post(RoomDto room)
        {
            var request = new CreateRoomCommand { RoomDto = room };

            var res = await _mediator.Send(request);

            if (res.Success) return Created("", res.Data);

            else if (res.ErrorCode == ErrorCodes.ROOM_MISSING_REQUIRED_INFORMATION) return NotFound(res);

            else if (res.ErrorCode == ErrorCodes.ROOM_COULD_NOT_STORE_DATA) return BadRequest(res);


            _logger.LogError("Response with unknown ErrorCode returned", res);
            return BadRequest(500);
        }

        [HttpGet]
        public async Task<ActionResult<RoomDto>> Get(int roomId)
        {
            var query = new GetRoomQuery
            {
                Id = roomId,
            };

            var res = await _mediator.Send(query);

            if (res.Success) return Created("", res.Data);

            return NotFound(res);
        }
    }
}
