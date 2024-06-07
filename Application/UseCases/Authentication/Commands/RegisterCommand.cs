using Application.Behavior.ResultPattern;
using Application.Contracts.Email;
using Application.Generic;
using FluentValidation;
using MediatR;

namespace Application.UseCases.Authentication.Commands
{

    public class RegisterCommand : IRequest<Result<bool>> {
		public required string Email { get; set; }
		public required string Password { get; set; }
	}

	public class RegisterCommandHandler : BaseHandlerRequest<RegisterCommand, Result<bool>> {

		private readonly IEmailService _emailService;

        public RegisterCommandHandler(BaseHandlerService service,
									  IEmailService emailService) : base(service)
        {
            _emailService = emailService;
        }

        public override async Task<Result<bool>> Handle(RegisterCommand request, CancellationToken cancellationToken) 
		{
			return Result<bool>.Success(true);
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
