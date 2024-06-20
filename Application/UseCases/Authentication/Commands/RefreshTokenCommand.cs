using Application.Behavior.ResultPattern;
using Application.Behavior.ResultPattern.ErrorModels.Authentication;
using Application.Contracts.Token;
using Application.Generic;
using Application.Services.Transcode;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Application.UseCases.Authentication.Commands
{
    public class RefreshTokenCommand : IRequest<Result<RefreshTokenCommandResponse>>
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
    }

    public class RefreshTokenCommandHandler : BaseHandlerRequest<RefreshTokenCommand, Result<RefreshTokenCommandResponse>>
    {
        private readonly ITokenService _tokenService;

        public RefreshTokenCommandHandler(BaseHandlerService service,
                                          ITokenService tokenService) : base(service)
        {
            _tokenService = tokenService;
        }

        public override async Task<Result<RefreshTokenCommandResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            ClaimsPrincipal claims;
            
            try
            {
                claims = _tokenService.GetClaims(request.AccessToken);
            }
            catch (Exception) {
                return Result<RefreshTokenCommandResponse>.Failure(AuthenticationErrors.InvalidToken);
            }

            var email = claims.FindFirst(ClaimTypes.Email)!.Value;

            var user = await _dbContext.TableNoTracking<User>()
                                       .Include(x => x.RefreshTokens)
                                       .FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

            // Failsafe, but should never happen
            if (user is null)
                return Result<RefreshTokenCommandResponse>.Failure(AuthenticationErrors.ServerError);

            var decodedRefreshToken = Transcode.DecodeURL(request.RefreshToken);

            var currentRefreshToken = user.RefreshTokens.FirstOrDefault(x => x.Token == decodedRefreshToken);

            if (currentRefreshToken is null)
                return Result<RefreshTokenCommandResponse>.Failure(AuthenticationErrors.Unauthorized);

            if (currentRefreshToken.AccessToken != request.AccessToken)
                return Result<RefreshTokenCommandResponse>.Failure(AuthenticationErrors.Unauthorized);

            if (currentRefreshToken.Expiry >= DateTime.UtcNow)
                return Result<RefreshTokenCommandResponse>.Failure(AuthenticationErrors.Unauthorized);

            var newAccessToken = _tokenService.GenerateAccessToken(claims.Claims);
            (var newRefreshToken, var expiry) = _tokenService.GenerateRefreshToken();

            _ = await _dbContext.DeleteAsync(currentRefreshToken, cancellationToken: cancellationToken);
            _ = await _dbContext.CreateAsync(new RefreshToken
            {
                Token = newRefreshToken,
                AccessToken = newAccessToken,
                Expiry = expiry,
                UserId = user.Id
            }, cancellationToken: cancellationToken);

            _ = await _dbContext.SaveChangesAsync(cancellationToken);

            return Result<RefreshTokenCommandResponse>.Success(new RefreshTokenCommandResponse {
                AccessToken = newAccessToken,
                RefreshToken = Transcode.EncodeURL(newRefreshToken)
            });
        }
    }

    public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenCommandValidator()
        {
            RuleFor(x => x.AccessToken)
                .NotEmpty();

            RuleFor(x => x.RefreshToken)
                .NotEmpty();
        }
    }

    public class RefreshTokenCommandResponse
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
    }
}
