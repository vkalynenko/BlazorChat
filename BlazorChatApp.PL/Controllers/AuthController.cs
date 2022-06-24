using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BlazorChatApp.BLL.Contracts.DTOs;
using BlazorChatApp.BLL.Infrastructure.Interfaces;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlazorChatApp.PL.Controllers
{
    [Authorize]
    [Route("/api/auth")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;

        public AuthController(UserManager<IdentityUser> userManager,
            ITokenService tokenService, IUserService userService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginDto model)
        {
            try
            {
                var user = await _userService.Login(model);
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new(ClaimTypes.Name, user.UserName),
                    new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                foreach (var userRole in userRoles) authClaims.Add(new Claim(ClaimTypes.Role, userRole));

                return Ok(new
                    {
                        GeneratedToken = _tokenService.GenerateJwtToken(authClaims)
                    }
                );
              
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterDto model)
        {
            try
            {
                await _userService.Register(model);
                return Ok(model);
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}