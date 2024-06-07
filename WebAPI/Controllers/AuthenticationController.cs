using Application.UseCases.Authentication.Commands;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebAPI.Controllers.BaseController;
using WebAPI.DataTransferObjects.Authentication;

namespace WebAPI.Controllers
{
    [AllowAnonymous]
    public class AuthenticationController : MainControllerBase
    {
        public AuthenticationController(IMediator mediator, IMapper mapper) : 
            base(mediator, mapper)
        { }

        [SwaggerOperation(Summary = "Register Account")]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            /*var command = _mapper.Map<RegisterCommand>(registerDTO);
            var result = await _mediator.Send(command);

            if (result.IsFailure)
                return StatusCode(result.Error.Code, result.Error.Message);
            */
            return Ok();
        }

        [SwaggerOperation(Summary = "Sign In")]
        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn([FromBody] SignInDTO signInDTO)
        {
            /*var command = _mapper.Map<SignInCommand>(signInDTO);
            var result = await _mediator.Send(command);

            if (result.IsFailure)
                return StatusCode(result.Error.Code, result.Error.Message);
            */
            return Ok();
        }

        // [FromQuery] is always a GET request.
        [SwaggerOperation(Summary = "Confirm Email")]
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string token, [FromQuery] string email)
        {           
            /*var result = await _mediator.Send(new ConfirmEmailCommand { 
                Email = email,
                Token = token
            });

            if (result.IsFailure) 
                return StatusCode(result.Error.Code, result.Error.Message);
            */
            return Ok();
        }
    }
}
