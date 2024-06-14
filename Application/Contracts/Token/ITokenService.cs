namespace Application.Contracts.Token
{
    public interface ITokenService
    {
        Task<JWTTokenModel> GenerateJWTAsync(string email, CancellationToken cancellationToken);
        RefreshTokenModel GenerateRefreshToken();
        string GenerateEmailVerificationToken();
    }
}
