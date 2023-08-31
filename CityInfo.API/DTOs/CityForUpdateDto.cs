﻿using System.ComponentModel.DataAnnotations;

namespace CityInfo.API.DTOs;

public class CityForUpdateDto
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; }
    
    [MaxLength(200)]
    public string? Description { get; set; }
}