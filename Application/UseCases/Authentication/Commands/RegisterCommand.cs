using Application.Contracts.Email;
using Application.Generic;
using Domain.Entities.IdentityExtensions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.UseCases.Authentication.Commands {
	public class RegisterCommand : IRequest<bool> {
		public required string Email { get; set; }
		public required string Password { get; set; }
	}

	public class RegisterCommandHandler : BaseHandlerRequest<RegisterCommand, bool> {

		private readonly UserManager<User> _userManager;
		private readonly IEmailService _emailService;

        public RegisterCommandHandler(BaseHandlerService service,
									  UserManager<User> userManager,
									  IEmailService emailService) : base(service)
        {
            _userManager = userManager;
            _emailService = emailService;
        }

        public override async Task<bool> Handle(RegisterCommand request, CancellationToken cancellationToken) {

			var user = _mapper.Map<User>(request);
            var result = await _userManager.CreateAsync(user, request.Password);

			if (result.Succeeded is false)
			{
				// ...
			}

			var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
			_ = _emailService.SendConfirmationEmail(token, request.Email, cancellationToken);

			// Add Role...

			return true;
		}
	}

	public class RegisterCommandValidator : AbstractValidator<RegisterCommand> {
		public RegisterCommandValidator() {
			RuleFor(x => x.Email)
				.NotEmpty();

			RuleFor(x => x.Password)
				.NotEmpty();
		}
	}
}
