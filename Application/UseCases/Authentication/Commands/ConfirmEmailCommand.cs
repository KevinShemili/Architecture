using Application.Behavior.ResultPattern;
using Application.Generic;
using FluentValidation;
using MediatR;

namespace Application.UseCases.Authentication.Commands
{
    public class ConfirmEmailCommand : IRequest<Result<bool>>
    {
        public required string Email { get; set; }
        public required string Token { get; set; }
    }

    public class ConfirmEmailCommandHandler : BaseHandlerRequest<ConfirmEmailCommand, Result<bool>>
    {
        public ConfirmEmailCommandHandler(BaseHandlerService service) : base(service) {
        }

        public override async Task<Result<bool>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken) 
        {
            return Result<bool>.Success(false);
        }
    }

    public class ConfirmEmailCommandValidator : AbstractValidator<ConfirmEmailCommand> {
        public ConfirmEmailCommandValidator() {
            RuleFor(x => x.Email)
                .NotEmpty();

            RuleFor(x => x.Token)
                .NotEmpty();
        }
    }
}
