using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace CityInfo.API.Entities;

public class PointOfInterest
{
    public PointOfInterest(string name)
    {
        Name = name;
    }
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int PointId { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; }
    
    [MaxLength(200)]
    public string? Description { get; set; }


    #region  Relations

    public int CityId { get; set; }
    
    [ForeignKey("CityId")]
    [JsonIgnore]
    public City? City { get; set; }

    #endregion
}