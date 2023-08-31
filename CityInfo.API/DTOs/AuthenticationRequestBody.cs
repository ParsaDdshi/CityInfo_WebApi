using System.ComponentModel.DataAnnotations;

namespace CityInfo.API.Entities;

public class AuthenticationRequestBody
{
    [Required]
    [MaxLength(50)]
    public string UserName { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Password { get; set; }
}