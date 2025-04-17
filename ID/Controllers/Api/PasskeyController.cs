using Fido2NetLib;
using ID.Commands.Passkey;
using ID.Commands.PassKey;
using ID.Queries.Passkey;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ID.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PasskeyController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PasskeyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> BeginRegistration()
        {
            try
            {
                var result = await _mediator.Send(new RegistrationCredentialsOptionsQuery());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> FinishRegistration([FromBody] RegistrationCredentialCommand request)
        {
            try
            {
                await _mediator.Send(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
