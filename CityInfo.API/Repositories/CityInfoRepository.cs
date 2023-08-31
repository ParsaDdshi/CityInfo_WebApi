using CityInfo.API.DbContexts;
using CityInfo.API.Entities;
using CityInfo.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Repositories;

public class CityInfoRepository : ICityInfoRepository
{
    private readonly CityInfoDbContext _context;

    public CityInfoRepository(CityInfoDbContext context)
    {
        _context = context ?? throw new ArgumentNullException();
    }
    
    public async Task<IEnumerable<City>> GetCitiesAsync()
    {
        return await _context.Cities
            .OrderBy(c => c.Name).ToListAsync();
    }

    public async Task<City?> GetCityByIdAsync(int cityId, bool includePoints)
    {
        if (includePoints)
        {
            return await _context.Cities
                .Include(c => c.PointOfInterests)
                .SingleOrDefaultAsync(c => c.CityId == cityId);
        }

        return await _context.Cities.SingleOrDefaultAsync(c => c.CityId == cityId);
    }

    public async Task InsertCity(City city)
    {
        await _context.Cities.AddAsync(city);
    }

    public void DeleteCity(City city)
    {
        _context.PointOfInterests.Where(p => p.CityId == city.CityId)
            .ToList().ForEach(p => _context.PointOfInterests.Remove(p));

        _context.Cities.Remove(city);
    }

    public async Task<bool> IsCityExistAsync(int cityId)
    {
        return await _context.Cities.AnyAsync(c => c.CityId == cityId);
    }

    public async Task<IEnumerable<PointOfInterest>> GetCityPointsOfInterestAsync(int cityId)
    {
        return await _context.PointOfInterests
            .Where(p => p.CityId == cityId).ToListAsync();
    }

    public async Task<PointOfInterest?> GetPointOfInterestAsync(int cityId, int pointId)
    {
        return await _context.PointOfInterests
            .SingleOrDefaultAsync(p => p.CityId == cityId && p.PointId == pointId);
    }

    public async Task InsertPointOfInterestAsync(int cityId, PointOfInterest point)
    {
        if (await IsCityExistAsync(cityId))
        {
            point.CityId = cityId;
            await _context.PointOfInterests.AddAsync(point);
        }
    }

    public void DeletePointOfInterest(PointOfInterest point)
    { 
        _context.PointOfInterests.Remove(point);
    }

    public async Task<bool> SaveAsync()
    {
        return (await _context.SaveChangesAsync() > 0);
    }
}