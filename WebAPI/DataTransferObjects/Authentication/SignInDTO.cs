namespace WebAPI.DataTransferObjects.Authentication
{
    public class SignInDTO
    {
        public required string Email { get; set; }
        public required string Password { get; set; }    
    }
}
