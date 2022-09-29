using System.ComponentModel.DataAnnotations;

namespace Proton.Frequency.Model;

public sealed class Location : CoreModel
{
    public string? Name { get; set; }

    public decimal Latitude { get; set; }

    public decimal Longitude { get; set; }
}
