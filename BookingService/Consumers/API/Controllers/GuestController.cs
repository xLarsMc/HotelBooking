using Application;
using Application.Guest.DTO;
using Application.Guest.Ports;
using Application.Guest.Requests;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GuestController : ControllerBase
    {
        private readonly ILogger<GuestController> _logger;
        private readonly IGuestManager _ports;

        public GuestController(ILogger<GuestController> logger, IGuestManager ports)
        {
            _logger = logger;
            _ports = ports;
        }

        [HttpPost]
        public async Task<ActionResult<GuestDTO>> Post(GuestDTO guest)
        {
            var request = new CreateGuestRequest { Data = guest };

            var res = await _ports.CreateGuest(request);

            if(res.Sucess) return Created("", res.Data);

            if (res.ErrorCode == ErrorCodes.NOT_FOUND) return NotFound(res);

            if (res.ErrorCode == ErrorCodes.INVALID_PERSON_ID) return BadRequest(res);

            if (res.ErrorCode == ErrorCodes.MISSING_REQUIRED_INFORMATION) return BadRequest(res);

            if (res.ErrorCode == ErrorCodes.COULD_NOT_STORE_DATA) return BadRequest(res);

            _logger.LogError("Response with unknown ErrorCode returned", res);
            return BadRequest(500);
        }

        [HttpGet]
        public async Task<ActionResult<GuestDTO>> Get(int guestId)
        {
            var res = await _ports.GetGuest(guestId);

            if (res.Sucess) return Created("", res.Data);

            return NotFound(res);
        }
    }
}
