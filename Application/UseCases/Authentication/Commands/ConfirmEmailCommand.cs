using Application.Behavior.ResultPattern;
using Application.Behavior.ResultPattern.ErrorModels.Authentication;
using Application.Contracts.Email;
using Application.Contracts.Token;
using Application.Generic;
using Application.Services.Transcode;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Application.UseCases.Authentication.Commands
{
    public class ConfirmEmailCommand : IRequest<Result<bool>>
    {
        public required string Email { get; set; }
        public required string Token { get; set; }
    }

    public class ConfirmEmailCommandHandler : BaseHandlerRequest<ConfirmEmailCommand, Result<bool>>
    {
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public ConfirmEmailCommandHandler(BaseHandlerService service,
                                          ITokenService tokenService,
                                          IEmailService emailService,
                                          IConfiguration configuration) : base(service)
        {
            _tokenService = tokenService;
            _emailService = emailService;
            _configuration = configuration;
        }

        public override async Task<Result<bool>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.EntityAsDbSet<User>()
                                       .Include(x => x.EmailTokens)
                                       .Where(x => x.Email == request.Email)
                                       .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            if (user is null)
                return Result<bool>.Failure(AuthenticationErrors.InvalidEmailToken);

            if (user.IsEmailVerified is true)
                return Result<bool>.Failure(AuthenticationErrors.AccountAlreadyVerified);

            var decodedToken = Transcode.DecodeURL(request.Token);

            var token = user.EmailTokens.FirstOrDefault(x => x.Token == decodedToken);

            if (token is null)
                return Result<bool>.Failure(AuthenticationErrors.InvalidEmailToken);

            if (DateTime.UtcNow > token.Expiry)
            {
                var emailToken = _tokenService.GenerateEmailVerificationToken();
                await _emailService.SendConfirmationEmailAsync(emailToken, request.Email, cancellationToken);

                _ = await _dbContext.DeleteAsync(token, cancellationToken: cancellationToken);
                
                user.EmailTokens.Add(new EmailToken
                {
                    Token = emailToken,
                    Expiry = DateTime.UtcNow.AddHours(
                        Convert.ToDouble(_configuration["VerificationTokenExpiries:ExpiryHours"]))
                });

                _ = await _dbContext.SaveChangesAsync(cancellationToken);

                return Result<bool>.Failure(AuthenticationErrors.ExpiredEmailToken);
            }

            user.IsEmailVerified = true;
            _ = await _dbContext.DeleteAsync(token, cancellationToken: cancellationToken);
            
            _ = await _dbContext.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true);
        }
    }

    public class ConfirmEmailCommandValidator : AbstractValidator<ConfirmEmailCommand>
    {
        public ConfirmEmailCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty();

            RuleFor(x => x.Token)
                .NotEmpty();
        }
    }
}
