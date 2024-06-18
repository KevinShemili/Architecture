using Application.Behavior.ResultPattern;
using Application.Behavior.ResultPattern.ErrorModels.Authentication;
using Application.Generic;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Authentication.Commands
{
    public class RevokeRefreshTokenCommand : IRequest<Result<bool>>
    {
        public int UserId { get; set; }
    }

    public class RevokeRefreshTokenCommandHandler : BaseHandlerRequest<RevokeRefreshTokenCommand, Result<bool>>
    {
        public RevokeRefreshTokenCommandHandler(BaseHandlerService service) : base(service)
        {
        }

        public override async Task<Result<bool>> Handle(RevokeRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var refreshToken = await _dbContext.EntityAsDbSet<RefreshToken>()
                                               .FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken);

            if (refreshToken is null)
                return Result<bool>.Failure(AuthenticationErrors.UserNotFound(request.UserId));

            _ = await _dbContext.DeleteAsync(refreshToken, true, cancellationToken);

            return Result<bool>.Success(true);
        }
    }

    public class RevokeRefreshCommandValidator : AbstractValidator<RevokeRefreshTokenCommand>
    {
        public RevokeRefreshCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();
        }
    }
}
