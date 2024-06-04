using Application.Behavior.ResultPattern;
using Application.Behavior.ResultPattern.ErrorModels;
using Application.Behavior.ResultPattern.ErrorModels.Authentication;
using Application.Contracts.Email;
using Application.Generic;
using Domain.Entities.IdentityExtensions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Application.UseCases.Authentication.Commands {

	public class RegisterCommand : IRequest<Result<bool>> {
		public required string Email { get; set; }
		public required string Password { get; set; }
	}

	public class RegisterCommandHandler : BaseHandlerRequest<RegisterCommand, Result<bool>> {

		private readonly UserManager<User> _userManager;
		private readonly IEmailService _emailService;

        public RegisterCommandHandler(BaseHandlerService<RegisterCommand> service,
									  UserManager<User> userManager,
									  IEmailService emailService) : base(service)
        {
            _userManager = userManager;
            _emailService = emailService;
        }

        public override async Task<Result<bool>> Handle(RegisterCommand request, CancellationToken cancellationToken) {

			var user = _mapper.Map<User>(request);
            var result = await _userManager.CreateAsync(user, request.Password);

			if (result.Succeeded is false)
			{
				return Result<bool>.Failure(AuthenticationErrors.SignUp(GetErrors(result)));
			}

			var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
			await _emailService.SendConfirmationEmailAsync(token, request.Email, cancellationToken);

			return Result<bool>.Success(true);
		}

		private List<Error> GetErrors(IdentityResult result)
		{
			var errors = result.Errors;
			var errorsList = new List<Error>();

			foreach (var error in errors)
			{
				errorsList.Add(new Error(StatusCodes.Status400BadRequest, error.Description));
			}

			return errorsList;
		}
	}

	public class RegisterCommandValidator : AbstractValidator<RegisterCommand> {
		public RegisterCommandValidator() {
			RuleFor(x => x.Email)
				.NotEmpty()
				.EmailAddress();

			RuleFor(x => x.Password)
				.NotEmpty();
		}
	}
}
