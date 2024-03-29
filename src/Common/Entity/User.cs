using Frequency.Common.Enums;
using Frequency.Common.Shared;

namespace Frequency.Common.Entity;

public sealed class User : GroupedEntity {
    public string FullName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Permission Permission { get; set; } = Permission.Standard;
    public string Password { get; set; } = string.Empty;
    public string Salt { get; set; } = string.Empty;
}