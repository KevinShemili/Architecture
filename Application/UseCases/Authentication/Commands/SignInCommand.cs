using Application.Behavior.ResultPattern;
using Application.Contracts.Token;
using Application.Generic;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Authentication.Commands
{
    public class SignInCommand : IRequest<List<User>>
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

    public class SignInCommandHandler : BaseHandlerRequest<SignInCommand, List<User>>
    {
        private readonly ITokenService _tokenService;

        public SignInCommandHandler(BaseHandlerService service,
                                    ITokenService tokenService) : base(service)
        {
            _tokenService = tokenService;
        }

        public async override Task<List<User>> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            var result1 = await _dbContext.EntityAsDbSet<User>().ToListAsync(cancellationToken);
            var result2 = await _dbContext.EntityAsDbSet<User>().IgnoreQueryFilters().ToListAsync(cancellationToken: cancellationToken);

            return result1;
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
