using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Backend.Models;
using Backend.Data;
using Backend.Dtos;
#pragma warning disable CS8601 // Possible null reference assignment.

namespace Backend.Controller
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController(IConfiguration configuration, IAuthRepository AuthRepo) : ControllerBase
    {
        private readonly IConfiguration configuration = configuration;
        private readonly IAuthRepository _AuthRepo = AuthRepo;

        [HttpPost]
        public async Task<ActionResult<Users>> Register(RejesterDto userToRejester)
        {
            userToRejester.Email = userToRejester.Email.ToLower();
            if (await _AuthRepo.UserExist(userToRejester.Email))
            {
                return BadRequest("Email Already Exists");
            }

            Users user = new()
            {
                Email = userToRejester.Email,
                FirstName = userToRejester.FirstName,
                LastName = userToRejester.LastName,
                RoleId = userToRejester.RoleId,
            };

            await _AuthRepo.Register(user, userToRejester.Password); //The user object and the sent password

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserDto userToLogin)
        {
            var user = await _AuthRepo.Login(userToLogin.Email.ToLower(), userToLogin.Password);

            if (user == null)
                return BadRequest("Email or password is invalid");


            string token = CreateToken(user);
            return Ok(token);
        }

        private string CreateToken(Users User)
        {
            string userName = User.FirstName + " " + User.LastName;
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, User.Id.ToString()),
                new Claim(ClaimTypes.Email, User.Email),
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Role, User.Role.Id.ToString()), //This refrences to the role in the roles table
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                NotBefore = DateTime.Now,
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
