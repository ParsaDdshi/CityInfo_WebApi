using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityInfo.API.Entities;

public class City
{
    public City(string name)
    {
        Name = name;
    }
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CityId { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; }
    
    [MaxLength(200)]
    public string? Description { get; set; }


    #region Relations

    public ICollection<PointOfInterest> PointOfInterests { get; set; } =
        new List<PointOfInterest>();

    #endregion
}