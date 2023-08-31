using CityInfo.API.Entities;

namespace CityInfo.API.DTOs;

public class CityDto
{
    public int CityId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public ICollection<PointOfInterest>? PointOfInterests { get; set; } =
        new List<PointOfInterest>();
}