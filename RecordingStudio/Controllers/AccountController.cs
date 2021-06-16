using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using RecordingStudio.Dto;
using RecordingStudio.Initializers;
using RecordingStudio.Models;
using RecordingStudio.Models.Input;

namespace RecordingStudio.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly RecordingStudioDbContext _context;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public AccountController(RecordingStudioDbContext context, UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost("registration")]
        public async Task<IActionResult> Register(UserInput model)
        {
            User user = new User()
            {
                Email = model.Email,
                UserName = model.UserName
            };
            // добавляем пользователя
            try
            {
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, Rolse.User);
                    var token = await Token(model);
                    return token;
                }

                return BadRequest(result.Errors);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Token([FromBody] LoginDto request)
        {
            var identity = await GetIdentity(request.UserName, request.Password);
            if (identity == null)
            {
                return BadRequest(new
                {
                    errorText = "Invalid username or password."
                });
            }

            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(),
                    SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            var response = new
            {
                accessToken = encodedJwt,
                role = identity.Claims.First(c => c.Type == ClaimsIdentity.DefaultRoleClaimType).Value
            };
            return new JsonResult(response);
        }

        private async Task<ClaimsIdentity> GetIdentity(string userName, string password)
        {
            User user =  _userManager.Users.FirstOrDefault(u=>u.UserName == userName);
            // добавляем пользователя
            var result = await _userManager.CheckPasswordAsync(user, password);
            if (user != null && result)
            {
                var role = await _userManager.GetRolesAsync(user);
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, role.First()),
                };
                ClaimsIdentity claimsIdentity =
                    new ClaimsIdentity(claims,
                        "Token",
                        ClaimsIdentity.DefaultNameClaimType,
                        ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            // если пользователя не найдено
            return null;
        }
    }
}