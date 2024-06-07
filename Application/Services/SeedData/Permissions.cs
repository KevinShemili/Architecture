namespace Application.Services.SeedData
{
    public class Permissions
    {
        private Permissions(string type, string value, string groupName, string description = null!)
        {
            Type = type;
            Value = value;
            Description = description;
        }

        private const string PERMISSION = "Permission";
        public readonly string Type;
        public readonly string Value;
        public readonly string Description;

        private static readonly Permissions AssignPermission = new(PERMISSION, "permission.assign", "Assign permission to role.");
        private static readonly Permissions AssignRole = new(PERMISSION, "role.assign", "Assign role to user.");
        private static readonly Permissions CreateRole = new(PERMISSION, "role.create", "Create new role.");
        private static readonly Permissions CreateUser = new(PERMISSION, "user.create", "Create new user.");

        public static List<Permissions> AdminPermissions =
        [
            AssignRole,
            AssignPermission,
            CreateRole,
            CreateUser
        ];

    }
}
