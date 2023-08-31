using CityInfo.API.Entities;

namespace CityInfo.API.Repositories.Interfaces;

public interface ICityInfoRepository
{
    Task<IEnumerable<City>> GetCitiesAsync();
    Task<City?> GetCityByIdAsync(int cityId, bool includePoints);
    Task InsertCity(City city);
    void DeleteCity(City city);
    Task<bool> IsCityExistAsync(int cityId);
    Task<IEnumerable<PointOfInterest>> GetCityPointsOfInterestAsync(int cityId);
    Task<PointOfInterest?> GetPointOfInterestAsync(int cityId, int pointId);
    Task InsertPointOfInterestAsync(int cityId, PointOfInterest point);
    void DeletePointOfInterest(PointOfInterest point);
    Task<bool> SaveAsync();
}