namespace Application.Services.SeedData
{
    public class Roles
    {
        public readonly string Name;
        public readonly string Description;
        public readonly List<Permissions> RolePermissions;

        private Roles(string name, string description = null!, List<Permissions> permissions = null!)
        {
            Name = name;
            Description = description;
            RolePermissions = permissions;
        }

        private static readonly Roles Administrator = new("Administrator", permissions: Permissions.AdminPermissions);
        private static readonly Roles BasicUser = new("BasicUser");

        public static List<Roles> RoleList = new()
        {
            Administrator,
            BasicUser
        };
    }
}
