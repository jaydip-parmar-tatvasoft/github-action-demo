namespace Application.Users.Queries
{
    public sealed class UserResponse
    {
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<RoleResponse> Roles { get; set; } = [];
    }

    public sealed class RoleResponse
    {
        public int RoleId { get; set; }

        public string RoleName { get; set; } = string.Empty;

        public List<PermissionResponse> Permissions { get; set; } = [];
    }

    public sealed class PermissionResponse
    {
        public int PermissionId { get; set; }

        public string PermissionName { get; set; } = string.Empty;
    }
}
