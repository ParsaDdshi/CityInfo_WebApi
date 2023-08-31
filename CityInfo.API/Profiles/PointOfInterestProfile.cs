using AutoMapper;
using CityInfo.API.DTOs;
using CityInfo.API.Entities;

namespace CityInfo.API.Profiles;

public class PointOfInterestProfile : Profile
{
    public PointOfInterestProfile()
    {
        CreateMap<PointOfInterest, PointOfInterestDto>();
        CreateMap<PointOfInterest, PointOfInterestForUpdateDto>();
        CreateMap<PointOfInterestForCreationDto, PointOfInterest>();
        CreateMap<PointOfInterestForUpdateDto, PointOfInterest>();
    }
}