using Application;
using Application.Room.Dtos;
using Application.Room.Ports;
using Application.Room.Request;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoomController : ControllerBase
    {
        private readonly ILogger<RoomController> _logger;
        private readonly IRoomRepository _roomManager;

        public RoomController(ILogger<RoomController> logger, IRoomRepository ports)
        {
            _logger = logger;
            _roomManager = ports;
        }

        [HttpPost]
        public async Task<ActionResult<RoomDto>> Post(RoomDto room)
        {
            var request = new CreateRoomRequest { Data = room };

            var res = await _roomManager.CreateRoom(request);

            if (res.Success) return Created("", res.Data);

            else if (res.ErrorCode == ErrorCodes.ROOM_MISSING_REQUIRED_INFORMATION) return NotFound(res);

            else if (res.ErrorCode == ErrorCodes.ROOM_COULD_NOT_STORE_DATA) return BadRequest(res);


            _logger.LogError("Response with unknown ErrorCode returned", res);
            return BadRequest(500);
        }

        /* [HttpGet]
        public async Task<ActionResult<GuestDTO>> Get(int guestId)
        {
            var res = await _ports.GetGuest(guestId);

            if (res.Sucess) return Created("", res.Data);

            return NotFound(res);
        }*/
    }
}
