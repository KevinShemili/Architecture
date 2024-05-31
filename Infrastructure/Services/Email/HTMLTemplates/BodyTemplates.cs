using System.Diagnostics;
using System.Reflection;

namespace Infrastructure.Services.Email.HTMLTemplates
{
    public static class BodyTemplates
    {
        public static async Task<string> VerifyEmailBody(string url, string email, string token, CancellationToken cancellationToken = default)
        {
            var temp = Environment.CurrentDirectory;
            var case1 = Path.GetFullPath(Path.Combine(temp, @"ConfirmEmail.html"));
            var result =  await File.ReadAllTextAsync(case1);

            return string.Empty;
        }

        /*public static async Task<string> ResetPasswordBody(string url, string email, string token)
        {

            var path = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "ForgotPasswordTemplate.html");
            string htmlTemplate = await File.ReadAllTextAsync(path);

            var body = htmlTemplate.Replace("LinkHere", $"{url}/api/Authentication/reset-password?token={token}&email={email}");

            return body;
        }*/

        private static string GetTemplateFilePath(string folderName, string fileName)
        {
            var dirPath = Assembly.GetEntryAssembly()?.Location;

            if (dirPath == null)
            {
                throw new InvalidOperationException("Unable to determine the directory of the executing assembly.");
            }

            dirPath = Path.GetDirectoryName(dirPath);

            if (dirPath == null)
            {
                throw new InvalidOperationException("Unable to determine the directory of the executing assembly.");
            }

            var fullPath = Path.Combine(dirPath, folderName, fileName);

            return fullPath;
        }
    }
}
