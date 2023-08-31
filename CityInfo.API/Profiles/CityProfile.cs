using AutoMapper;
using CityInfo.API.DTOs;
using CityInfo.API.Entities;

namespace CityInfo.API.Profiles;

public class CityProfile : Profile
{
    public CityProfile()
    {
        CreateMap<City, CityWithoutPointsOfInterestDto>();
        CreateMap<City, CityForUpdateDto>();
        CreateMap<City, CityDto>();
        CreateMap<CityForCreationDto, City>();
        CreateMap<CityForUpdateDto, City>();
    }
}