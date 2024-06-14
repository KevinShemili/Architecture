namespace Application.Contracts.Token
{
    public class JWTTokenModel
    {
        public string JWTToken { get; set; }
        public DateTime Expiry {  get; set; }
    }
}
