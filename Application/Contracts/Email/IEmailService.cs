namespace Application.Contracts.Email
{
    public interface IEmailService
    {
        Task SendConfirmationEmailAsync(string token, string email, CancellationToken cancellationToken = default);
    }
}
