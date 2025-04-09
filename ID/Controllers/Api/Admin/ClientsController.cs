using ID.Commands.Admin.Clients;
using ID.Domain.Dto.Admin;
using ID.Queries.Admin.Clients;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using System.Reflection;

namespace ID.Controllers.Api.Admin
{
    [Area("Admin")]
    [Route("api/v1/[area]/[controller]/[action]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ClientsController(IMediator mediator) 
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateClientCommand command)
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

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                return Ok(await _mediator.Send(new GetClientDetailsQuery(id)));
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Search(GetClientsQuery query)
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

        [HttpPut]
        public async Task<IActionResult> Update(UpdateClientCommand command)
        {
            try
            {
                await _mediator.Send(command);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
