namespace Application.Contracts.Email
{
    public interface IEmailService
    {
        Task<bool> SendConfirmationEmail(string token, string email, CancellationToken cancellationToken = default);
    }
}
