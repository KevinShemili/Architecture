using Domain.Entities;

namespace Infrastructure.EntityConfigurations.SeedData
{
    public class Roles
    {
        public static readonly Role Administrator = new Role
        {
            Id = 1,
            Name = "administrator",
            DateCreated = DateTime.UtcNow
        };

        public static readonly Role BasicUser = new Role
        {
            Id = 2,
            Name = "user",
            DateCreated = DateTime.UtcNow
        };

        public static readonly List<Role> SeedRoles =
        [
            Administrator,
            BasicUser
        ];
    }
}
