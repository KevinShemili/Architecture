namespace Application.Contracts.Token
{
    public class RefreshTokenModel
    {
        public required string RefreshToken { get; set; }
        public DateTime Expiry { get; set; }
    }
}
