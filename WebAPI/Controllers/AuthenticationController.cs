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
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public AuthenticationController(IMediator mediator,
                                        IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [SwaggerOperation(Summary = "Register Account")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            var command = _mapper.Map<RegisterCommand>(registerRequest);
            //_ = await _mediator.Send(command);

            await BodyTemplates.VerifyEmailBody("", "", "", default);

            return Ok();
        }

        [SwaggerOperation(Summary = "Confirm Email")]
        [HttpPut]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string token, [FromQuery] string email)
        {           
            return Ok();
        }
    }
}
