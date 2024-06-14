using Domain.Entities;

namespace Infrastructure.EntityConfigurations.SeedData
{
    public class Roles
    {
        public static readonly Role Administrator = new Role
        {
            Id = 1,
            Name = "administrator",
        };

        public static readonly Role BasicUser = new Role
        {
            Id = 2,
            Name = "basic-user",
        };

        public static readonly List<Role> SeedRoles =
        [
            Administrator,
            BasicUser
        ];
    }
}
