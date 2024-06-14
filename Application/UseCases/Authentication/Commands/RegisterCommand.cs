using Application.Behavior.ResultPattern;
using Application.Behavior.ResultPattern.ErrorModels.Authentication;
using Application.Contracts.Email;
using Application.Contracts.Token;
using Application.Generic;
using Application.Services.Hasher;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Application.UseCases.Authentication.Commands
{
    public class RegisterCommand : IRequest<Result<bool>>
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

    public class RegisterCommandHandler : BaseHandlerRequest<RegisterCommand, Result<bool>>
    {
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public RegisterCommandHandler(BaseHandlerService service,
                                      ITokenService tokenService,
                                      IEmailService emailService,
                                      IConfiguration configuration) : base(service)
        {
            _tokenService = tokenService;
            _emailService = emailService;
            _configuration = configuration;
        }

        public override async Task<Result<bool>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            // Begin transaction
            try
            {
                var isEmailRegistered = await _dbContext.TableNoTracking<User>()
                                                        .AnyAsync(x => x.Email == request.Email, cancellationToken);

                if (isEmailRegistered is true)
                    return Result<bool>.Failure(AuthenticationErrors.EmailAlreadyExists);

                var emailToken = _tokenService.GenerateEmailVerificationToken();
                await _emailService.SendConfirmationEmailAsync(emailToken, request.Email, cancellationToken);

                var user = _mapper.Map<User>(request);

                (user.PasswordHash, user.PasswordSalt) = Hasher.HashPasword(request.Password);

                _ = await _dbContext.CreateAsync(user, true, cancellationToken);
                _ = await _dbContext.CreateAsync(new EmailToken
                {
                    Token = emailToken,
                    Expiry = DateTime.UtcNow.AddHours(
                        Convert.ToDouble(_configuration["VerificationTokenExpiries:ExpiryHours"])),
                    UserId = user.Id,
                }, cancellationToken: cancellationToken);

                _ = _dbContext.SaveChangesAsync(cancellationToken);
                return Result<bool>.Success(true);
            }
            catch (Exception)
            {
                // Log
                // Rollback
                throw;
            }
        }
    }

    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty();
        }
    }
}
