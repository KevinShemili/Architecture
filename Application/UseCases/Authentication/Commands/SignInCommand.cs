using Application.Behavior.ResultPattern;
using Application.Behavior.ResultPattern.ErrorModels.Authentication;
using Application.Contracts.Token;
using Application.Generic;
using Application.Services.Hasher;
using Application.Services.Transcode;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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
        private readonly IConfiguration _configuration;

        public SignInCommandHandler(BaseHandlerService service,
                                    ITokenService tokenService,
                                    IConfiguration configuration) : base(service)
        {
            _tokenService = tokenService;
            _configuration = configuration;
        }

        public async override Task<Result<SignInCommandResult>> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.EntityAsDbSet<User>()
                                       .Include(x => x.RefreshTokens)
                                       .FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken: cancellationToken);

            if (user is null)
                return Result<SignInCommandResult>.Failure(AuthenticationErrors.UserNotFound(request.Email));

            if (user.IsEmailVerified is false)
                return Result<SignInCommandResult>.Failure(AuthenticationErrors.AccountNotVerified);

            if (user.IsBlocked is true)
                return Result<SignInCommandResult>.Failure(AuthenticationErrors.LockedOut);

            var isPasswordCorrect = Hasher.VerifyPassword(request.Password, user.PasswordHash, user.PasswordSalt);

            if (isPasswordCorrect is false)
            {
                var maxTries = int.Parse(_configuration["FailedLogin:MaxTries"]!);

                if (user.FailedLoginTries >= maxTries)
                {
                    user.IsBlocked = true;

                    _ = await _dbContext.SaveChangesAsync(cancellationToken);

                    return Result<SignInCommandResult>.Failure(AuthenticationErrors.LockedOut);
                }

                user.FailedLoginTries += 1;

                _ = await _dbContext.SaveChangesAsync(cancellationToken);

                return Result<SignInCommandResult>.Failure(AuthenticationErrors.SignInFailure);
            }

            user.FailedLoginTries = 0;

            var accessToken = await _tokenService.GenerateAccessTokenAsync(request.Email, cancellationToken);
            (var refreshToken, var refreshExpiry) = _tokenService.GenerateRefreshToken();

            var lastRefreshToken = user.RefreshTokens.OrderByDescending(x => x.DateCreated)
                                                     .FirstOrDefault();

            if (lastRefreshToken is not null)
                _ = await _dbContext.DeleteAsync(lastRefreshToken, cancellationToken: cancellationToken);

            user.RefreshTokens.Add(new RefreshToken
            {
                Token = refreshToken,
                Expiry = refreshExpiry,
                AccessToken = accessToken
            });

            _ = await _dbContext.SaveChangesAsync(cancellationToken);

            return Result<SignInCommandResult>.Success(new SignInCommandResult
            {
                AccessToken = accessToken,
                RefreshToken = Transcode.EncodeURL(refreshToken)
            });
        }
    }

    public class SignInCommandValidator : AbstractValidator<SignInCommand>
    {
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
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
    }
}