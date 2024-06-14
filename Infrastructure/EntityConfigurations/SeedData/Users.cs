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
            PasswordHash = Hasher.HashPasword("admin").Item1,
            PasswordSalt = Hasher.HashPasword("admin").Item2,
        };

        public static readonly List<User> SeedUsers =
        [
            Administrator
        ];
    }
}
