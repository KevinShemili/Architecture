using Application.Services.Hasher;
using Domain.Entities;

namespace Infrastructure.EntityConfigurations.SeedData
{
    public class Users
    {
        public static readonly User Administrator = new User
        {            
            Id = 1,
            UserName = "admin",
            Email = "admin@mail.com",
            IsEmailVerified = true,
            PasswordHash = Hasher.AdminHash,
            PasswordSalt = Hasher.AdminSalt,
            DateCreated = DateTime.UtcNow
        };

        public static readonly List<User> SeedUsers =
        [
            Administrator
        ];
    }
}
