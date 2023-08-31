using CityInfo.API.DbContexts;
using CityInfo.API.Entities;
using CityInfo.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly CityInfoDbContext _context;

    public AuthRepository(CityInfoDbContext context)
    {
        _context = context;
    }
    public async Task<CityInfoUser> ValidateUserCredentials(string userName, string password)
    {
        return await _context.CityInfoUsers
            .SingleOrDefaultAsync(u => u.UserName == userName && u.Password == password);
    }
}