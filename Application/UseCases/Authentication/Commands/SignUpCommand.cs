using Application.Generic;
using FluentValidation;
using MediatR;

namespace Application.UseCases.Authentication.Commands {
	public class SignUpCommand : IRequest<bool> {
	}

	public class SignUpCommandHandler : BaseHandlerRequest<SignUpCommand, bool> {
		public SignUpCommandHandler(BaseHandlerService service) : base(service) {
		}

		public override Task<bool> Handle(SignUpCommand request, CancellationToken cancellationToken) {
			throw new NotImplementedException();
		}
	}

	public class SignUpCommandValidator : AbstractValidator<SignUpCommand> {
		public SignUpCommandValidator() { }
	}
}
