using Domain.Entities;

namespace Infrastructure.EntityConfigurations.SeedData
{
    public class Permissions
    {
        public static readonly Permission AssignPermission = new Permission
        {
            Id = 1,
            Key = "permission.assign",
            Name = "Assign permission to role.",
            DateCreated = DateTime.UtcNow
        };

        public static readonly Permission AssignRole = new Permission
        {
            Id = 2,
            Key = "role.assign",
            Name = "Assign role to user.",
            DateCreated = DateTime.UtcNow
        };

        public static readonly Permission CreateRole = new Permission
        {
            Id = 3,
            Key = "role.create",
            Name = "Create new role.",
            DateCreated = DateTime.UtcNow
        };

        public static readonly Permission CreateUser = new Permission
        {
            Id = 4,
            Key = "user.create",
            Name = "Create new user.",
            DateCreated = DateTime.UtcNow
        };

        public static readonly List<Permission> SeedAdminPermissions =
        [
            AssignRole,
            AssignPermission,
            CreateRole,
            CreateUser
        ];

        public static readonly List<Permission> SeedPermissions =
        [
            AssignRole,
            AssignPermission,
            CreateRole,
            CreateUser
        ];
    }
}
