using ID.Commands.Admin;
using ID.Domain.Entity;
using ID.Queries;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

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
                    return Redirect($"{_configuration["FrontendRoute"]}/emailconfirmed");
                return Redirect($"{_configuration["FrontendRoute"]}/emailnotconfirmed");
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
    }
}
