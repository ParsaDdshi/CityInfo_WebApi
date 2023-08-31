using System.ComponentModel.DataAnnotations;

namespace CityInfo.API.Entities;

public class CityInfoUser
{
    [Key]
    public int UserId { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string UserName { get; set; }

    [Required]
    [MaxLength(50)]
    public string Password { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string LastName { get; set; }
}