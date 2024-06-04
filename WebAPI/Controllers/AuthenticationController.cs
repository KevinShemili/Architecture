using Application.UseCases.Authentication.Commands;
using AutoMapper;
using Infrastructure.Services.Email.HTMLTemplates;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebAPI.Controllers.BaseController;

namespace WebAPI.Controllers
{
    [AllowAnonymous]
    public class AuthenticationController : MainControllerBase
    {
        public AuthenticationController(IMediator mediator, IMapper mapper) : base(mediator, mapper)
        {
        }

        [SwaggerOperation(Summary = "Register Account")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            var command = _mapper.Map<RegisterCommand>(registerRequest);
            var result = await _mediator.Send(command);

            if (result.IsFailure)
                return StatusCode(result.Error.Code, result.Error.Message);

            return Ok(result.IsSuccess);
        }

        [SwaggerOperation(Summary = "Confirm Email")]
        [HttpPut]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string token, [FromQuery] string email)
        {           
            return Ok();
        }
    }
}
