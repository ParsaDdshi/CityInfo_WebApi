using AutoMapper;
using CityInfo.API.DTOs;
using CityInfo.API.Entities;
using CityInfo.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    [ApiController]
    public class CitiesApiController : ControllerBase
    {
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CitiesApiController> _logger;
        public CitiesApiController(ICityInfoRepository cityInfoRepository
        , IMapper mapper, ILogger<CitiesApiController> logger)
        {
            _cityInfoRepository = cityInfoRepository ?? 
                                  throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDto>>> GetCities()
        {
            IEnumerable<City> cities = await _cityInfoRepository.GetCitiesAsync();
            return Ok(
                _mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cities));
        }

        [HttpGet("{cityId}", Name = "GetCity")]
        public async Task<IActionResult> GetCity(int cityId, bool includePoints = false)
        {
            City city = await _cityInfoRepository.GetCityByIdAsync(cityId, includePoints);
            if (city == null) return NotFound();

            if (includePoints)
                return Ok(
                    _mapper.Map<CityDto>(city));

            return Ok(
                _mapper.Map<CityWithoutPointsOfInterestDto>(city));
        }

        [HttpPost]
        public async Task<ActionResult<CityDto>> InsertCity(CityForCreationDto city)
        {
            City finalCity = _mapper.Map<City>(city);
            await _cityInfoRepository.InsertCity(finalCity);
            await _cityInfoRepository.SaveAsync();

            CityDto createdCity = _mapper.Map<CityDto>(finalCity);

            return CreatedAtRoute("GetCity", 
                new {cityId = createdCity.CityId},
                createdCity
            );
        }

        [HttpPut("{cityId}")]
        public async Task<ActionResult> UpdateCity(int cityId, CityForUpdateDto city)
        {
            City cityToUpdate = await _cityInfoRepository.GetCityByIdAsync(cityId, false);
            
            if (city == null)
            {
                _logger.LogInformation($"City With Id {cityId} Does Not Exists In Database");
                return NotFound();
            }

            _mapper.Map(city, cityToUpdate);
            _cityInfoRepository.SaveAsync();

            return NoContent();
        }

        [HttpPatch("{cityId}")]
        public async Task<ActionResult> UpdateCityPartially(int cityId, JsonPatchDocument<CityForUpdateDto> city)
        {
            City cityToUpdate = await _cityInfoRepository.GetCityByIdAsync(cityId, false);
            if (cityToUpdate == null)
            {
                _logger.LogInformation($"City With Id {cityId} Does Not Exists In Database");
                return NotFound();
            }

            CityForUpdateDto cityToPatch = _mapper.Map<CityForUpdateDto>(cityToUpdate);
            city.ApplyTo(cityToPatch, ModelState);
            if (!ModelState.IsValid)
            {
                _logger.LogInformation($"Can Not Update City With Id {cityId} Because Of Some Validation Problem");
                return BadRequest(ModelState);
            }
            
            if (!TryValidateModel(cityToPatch))
            {
                _logger.LogInformation($"Can Not Update City With Id {cityId} Because Of Some Validation Problem");
                return BadRequest(ModelState);
            }

            _mapper.Map(cityToPatch, cityToUpdate);
            await _cityInfoRepository.SaveAsync();

            return NoContent();
        }

        [HttpDelete("{cityId}")]
        public async Task<ActionResult> RemoveCity(int cityId)
        {
            City city = await _cityInfoRepository.GetCityByIdAsync(cityId, false);
            
            if (city == null)
            {
                _logger.LogInformation($"City With Id {cityId} Does Not Exists In Database");
                return NotFound();
            }
            
            _cityInfoRepository.DeleteCity(city);
            await _cityInfoRepository.SaveAsync();

            return NoContent();
        }
    }
}
