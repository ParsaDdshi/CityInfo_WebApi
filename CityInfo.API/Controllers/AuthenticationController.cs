using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CityInfo.API.Entities;
using CityInfo.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CityInfo.API.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IAuthRepository _authRepository;

        public AuthenticationController(IConfiguration configuration, ILogger<AuthenticationController> logger, IAuthRepository authRepository)
        {
            _configuration = configuration;
            _logger = logger;
            _authRepository = authRepository;
        }

        [HttpPost("authenticate")]
        public async Task<ActionResult<string>> Authenticate(AuthenticationRequestBody authenticationRequestBody)
        {

            CityInfoUser user = await _authRepository.ValidateUserCredentials(authenticationRequestBody.UserName,
            authenticationRequestBody.Password);

            if (user == null)
                return Unauthorized();

                SymmetricSecurityKey securityKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(_configuration["Authentication:SecretForKey"])
            );
            
            SigningCredentials signingCredentials = new SigningCredentials(
                securityKey,SecurityAlgorithms.HmacSha256
            );
            
            List<Claim> claimsForToken = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.UserName.ToString())
            };

            JwtSecurityToken jwtSecurityToke = new JwtSecurityToken(
                _configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claimsForToken,
                DateTime.Now,
                DateTime.Now.AddHours(1),
                signingCredentials
            );

            string tokenToReturn = new JwtSecurityTokenHandler()
                .WriteToken(jwtSecurityToke);
            
            return Ok(tokenToReturn);
        }
    }
}
