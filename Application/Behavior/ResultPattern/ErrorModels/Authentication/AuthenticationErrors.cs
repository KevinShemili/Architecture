using Microsoft.AspNetCore.Http;

namespace Application.Behavior.ResultPattern.ErrorModels.Authentication
{
    public static class AuthenticationErrors
    {
        public static Error UserNotFound(string Email) => new(StatusCodes.Status404NotFound, 
            $"User with email: {Email} does not exist in the system");

        public static readonly Error SignInFailure = new(StatusCodes.Status400BadRequest,
            "Please try again.");

        public static readonly Error LockedOut = new(StatusCodes.Status429TooManyRequests,
            "Please try again later.");

        public static readonly Error InvalidCredentials = new(StatusCodes.Status400BadRequest,
            "Invalid credentials.");
    }
}
