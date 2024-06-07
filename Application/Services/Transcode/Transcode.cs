using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace Application.Services.Transcode
{
    public static class Transcode
    {
        public static string Encode(string content)
        {
            return WebEncoders.Base64UrlEncode(
                Encoding.UTF8.GetBytes(content));
        }

        public static string Decode(string content)
        {
            return Encoding.UTF8.GetString(
                WebEncoders.Base64UrlDecode(content));
        }
    }
}
