using Microsoft.AspNetCore.Http;

namespace Application.Behavior.ResultPattern.ErrorModels.Authentication
{
    public static class AuthenticationErrors
    {
        public static Error SignUp(IEnumerable<Error> errors) => new(
            StatusCodes.Status400BadRequest, string.Join(Environment.NewLine, errors.Select(x => x.Message)));
    }
}
