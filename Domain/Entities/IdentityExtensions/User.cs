using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.IdentityExtensions
{
    public class User : IdentityUser
    {
        public string? Initials { get; set; }
    }
}
