using Application.Behavior.ResultPattern;
using Application.Contracts.Token;
using Application.Generic;
using FluentValidation;
using MediatR;

namespace Application.UseCases.Authentication.Commands
{
    public class SignInCommand : IRequest<Result<SignInCommandResult>>
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

    public class SignInCommandHandler : BaseHandlerRequest<SignInCommand, Result<SignInCommandResult>>
    {
        private readonly ITokenService _tokenService;

        public SignInCommandHandler(BaseHandlerService service,
                                    ITokenService tokenService) : base(service)
        {
            _tokenService = tokenService;
        }

        public async override Task<Result<SignInCommandResult>> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            return Result<SignInCommandResult>
                .Success(new SignInCommandResult {} );
        }
    }

    public class SignInCommandValidator : AbstractValidator<SignInCommand> {
        public SignInCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty();

            RuleFor(x => x.Password)
                .NotEmpty();
        }
    }

    public class SignInCommandResult
    {
        public string? AccessToken { get; set; }
        public DateTime AccessTokenExpiry { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiry { get; set; }
    }
}
