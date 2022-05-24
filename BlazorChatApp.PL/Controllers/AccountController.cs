using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BlazorChatApp.BLL.Contracts.DTOs;
using Microsoft.AspNetCore.Identity;
using IAuthorizationService = BlazorChatApp.BLL.Infrastructure.Interfaces.IAuthorizationService;

namespace BlazorChatApp.PL.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IAuthorizationService _service;

        public AccountController(UserManager<IdentityUser> userManager, IAuthorizationService service)
        {
            _userManager = userManager;
            _service = service;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync(string userName, string password)
        {
            var model = new LoginDto
            {
                UserName = userName,
                Password = password
            };
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = _service.GenerateJwtToken(authClaims);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }

            return Unauthorized();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterAsync(string userName, string password)
        {
            var model = new RegisterDto
            {
                UserName = userName,
                Password = password,

            };

            var userExists = await _userManager.FindByNameAsync(model.UserName);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new ResponseDto { Status = "Error", Message = "User already exists!" });

            IdentityUser user = new()
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserName
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new ResponseDto { Status = "Error", 
                        Message = "User creation failed! Please check user details and try again." });

            return Ok(new ResponseDto { Status = "Success", Message = "User created successfully!" });
        }


        [HttpGet]
        [Route("/logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            await _service.LogOutAsync();
            return Ok();
        }
        
    }
}
