namespace Proton.Frequency.Common.Entity; 

public sealed class User : BaseModel {
    public string FullName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool Admin { get; set; } = false;
    public string Password { get; set; } = string.Empty;
    public string Salt { get; set; } = string.Empty;
}
