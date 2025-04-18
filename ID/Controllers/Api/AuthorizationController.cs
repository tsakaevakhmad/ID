using ID.Commands;
using ID.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ID.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;

        public AuthorizationController(IMediator mediator, IConfiguration configuration) 
        {
            _mediator = mediator;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterCommand command)
        {
            try
            {
                return Ok(await _mediator.Send(command));
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("~/Auth/[action]")]
        public async Task<ActionResult> ConfirmMail(string id, string token)
        {
            try
            {
                if(await _mediator.Send(new MailConfirmationCommand(id, token)))
                    return Redirect($"/emailconfirmed");
                return Redirect($"/emailnotconfirmed");
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> LoginTwoFa(LoginQuery query)
        {
            try
            {
                return Ok(await _mediator.Send(query));
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> VerifyLoginTwoFa(LoginVerifyQuery query)
        {
            try
            {
                var result = await _mediator.Send(query);  
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> IsAuthorized()
        {
            return Ok(new { Status = "Success" });
        }
    }
}
