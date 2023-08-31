﻿namespace CityInfo.API.DTOs;

public class CityWithoutPointsOfInterestDto
{
    public int CityId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}