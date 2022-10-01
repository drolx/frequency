namespace Proton.Frequency.Common.Entity; 

public sealed class Category : BaseModel {
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}
