namespace Proton.Frequency.Common.Entity; 

public sealed class User : BaseModel {
    public string FullName { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public bool Admin { get; set; } = false;
    public string Password { get; set; }
}
