using AutoMapper;
using CityInfo.API.DTOs;
using CityInfo.API.Entities;
using CityInfo.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [Authorize]
    [Route("api/cities/{cityId}/pointsofinterest")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PointsOfInterestController> _logger;
        public PointsOfInterestController(ICityInfoRepository cityInfoRepository
            , IMapper mapper,
            ILogger<PointsOfInterestController> logger)
        {
            _cityInfoRepository = cityInfoRepository ?? 
                                  throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> GetCityPoints(int cityId)
        {
            if (await _cityInfoRepository.IsCityExistAsync(cityId))
            {
                IEnumerable<PointOfInterest> cityPoints = await _cityInfoRepository
                    .GetCityPointsOfInterestAsync(cityId);

                return Ok(_mapper.Map<IEnumerable<PointOfInterestDto>>(cityPoints));
            }
            _logger.LogInformation($"City With Id {cityId} Does Not Exits In Database");
            return NotFound();
        }

        [HttpGet("{pointId}", Name = "GetPointOfInterest")]
        public async Task<ActionResult<PointOfInterestDto>> GetPointOfInterest(int cityId, int pointId)
        {
            if (!await _cityInfoRepository.IsCityExistAsync(cityId))
            {
                _logger.LogInformation($"City With Id {cityId} Does Not Exsits In Database");
                return NotFound();
            }

            PointOfInterest point = await _cityInfoRepository.GetPointOfInterestAsync(cityId, pointId);

            if (point == null)
            {
                _logger.LogInformation($"Point Of Interest With Id {pointId} Does Not Exits In Database");
                return NotFound(); 
            }

            return Ok(_mapper.Map<PointOfInterestDto>(point));
        }

        [HttpPost]
        public async Task<ActionResult<PointOfInterestDto>> InsertPoint(PointOfInterestForCreationDto point, int cityId)
        {
            if (!await _cityInfoRepository.IsCityExistAsync(cityId))
            {
                _logger.LogInformation($"City With Id {cityId} Does Not Exits In Database");
                return NotFound();
            }

            PointOfInterest finalPoint = _mapper.Map<PointOfInterest>(point);

            await _cityInfoRepository.InsertPointOfInterestAsync(cityId,finalPoint);
            await _cityInfoRepository.SaveAsync();

            PointOfInterestDto createdPoint = _mapper.Map<PointOfInterestDto>(finalPoint);
            return CreatedAtRoute("GetPointOfInterest", 
                new {cityId = cityId, pointId = createdPoint.PointId}
                ,createdPoint);
        }

        [HttpPut("{pointId}")]
        public async Task<ActionResult> UpdatePoint(int cityId, int pointId,PointOfInterestForUpdateDto point)
        {
            if (!await _cityInfoRepository.IsCityExistAsync(cityId))
            {
                _logger.LogInformation($"City With Id {cityId} Does Not Exists In Database");
                return NotFound();
            }


            PointOfInterest pointToUpdate = await _cityInfoRepository
                .GetPointOfInterestAsync(cityId, pointId);

            if (pointToUpdate == null)
            {
                _logger.LogInformation($"Point Of Interest With Id {pointId} Does Not Exsits In Database");
                return NotFound();
            }

            _mapper.Map(point, pointToUpdate);
            _cityInfoRepository.SaveAsync();

            return NoContent();
        }

        [HttpPatch("{pointId}")]
        public async Task<ActionResult> UpdatePointPartially(int cityId, int pointId
        ,JsonPatchDocument<PointOfInterestForUpdateDto> point)
        {
            if (!await _cityInfoRepository.IsCityExistAsync(cityId))
            {
                _logger.LogInformation($"City With Id {cityId} Does Not Exists In Database");
                return NotFound();
            }

            PointOfInterest pointToUpdate = await _cityInfoRepository.GetPointOfInterestAsync(cityId, pointId);
            if (pointToUpdate == null)
            {
                _logger.LogInformation($"Point With Id {pointId} Does Not Exists In Database");
                return NotFound();
            }

            PointOfInterestForUpdateDto pointToPatch = _mapper.Map<PointOfInterestForUpdateDto>(pointToUpdate);
            point.ApplyTo(pointToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                _logger.LogInformation($"Can Not Update The Point With Id {pointId} In City With Id {cityId} " +
                                       $"Partially Because Of Some Validation Issues");
                return BadRequest(ModelState);
            }

            if (!TryValidateModel(pointToPatch))
            {
                _logger.LogInformation($"Can Not Update The Point With Id {pointId} In City With Id {cityId} " +
                                       $"Partially Because Of Some Validation Issues");
                return BadRequest(ModelState);
            }

            _mapper.Map(pointToPatch, pointToUpdate);
            await _cityInfoRepository.SaveAsync();
            
            return NoContent();
        }

        [HttpDelete("pointId")]
        public async Task<ActionResult> DeletePoint(int pointId, int cityId)
        {
            if (!await _cityInfoRepository.IsCityExistAsync(cityId))
            {
                _logger.LogInformation($"City With Id {cityId} Does Not Exists In Database");
                return NotFound();
            }

            PointOfInterest point = await _cityInfoRepository.GetPointOfInterestAsync(cityId, pointId);
            if (point == null)
            {
                _logger.LogInformation($"Point With Id {pointId} Does Not Exists In Database");
                return NotFound();
            }
            
            _cityInfoRepository.DeletePointOfInterest(point);
            await _cityInfoRepository.SaveAsync();

            return NoContent();
        }
    }
}