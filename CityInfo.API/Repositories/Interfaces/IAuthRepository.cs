using CityInfo.API.Entities;

namespace CityInfo.API.Repositories.Interfaces;

public interface IAuthRepository
{
    Task<CityInfoUser> ValidateUserCredentials(string userName, string password);
}