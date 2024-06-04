using System.Reflection;

namespace Infrastructure.Services.Email.HTMLTemplates
{
    public static class BodyTemplates
    {
        public static async Task<string> VerifyEmailBody(string url, string email, string token, 
            CancellationToken cancellationToken = default)
        {
            var baseDirectory = AppContext.BaseDirectory;
            var relativePath = Path.Combine(baseDirectory, "Services", "Email", "HTMLTemplates", "ConfirmEmail.html");
            var result = await File.ReadAllTextAsync(relativePath, cancellationToken);

            var body = result.Replace("LINKHERE", $"{url}/api/Authentication?token={token}&email={email}");

            return body;
        }

        /*public static async Task<string> ResetPasswordBody(string url, string email, string token)
        {

            var path = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "ForgotPasswordTemplate.html");
            string htmlTemplate = await File.ReadAllTextAsync(path);

            var body = htmlTemplate.Replace("LinkHere", $"{url}/api/Authentication/reset-password?token={token}&email={email}");

            return body;
        }*/
    }
}
