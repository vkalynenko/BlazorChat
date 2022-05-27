using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BlazorChatApp.BLL.Contracts.DTOs;
using BlazorChatApp.BLL.Helpers;
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
        private readonly ILocalStorageService _localStorage;
        private readonly IUserService _userService;
        public AuthController(UserManager<IdentityUser> userManager, ILocalStorageService localStorage, ITokenService tokenService, IUserService userService)
        {
            _userManager = userManager;
            _localStorage = localStorage;
            _tokenService = tokenService;
            _userService = userService;
        }

        [AllowAnonymous]
        //[Route("login")]
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginDto model /*string userName, string password*/)
        {
            // LoginDto model = new LoginDto { UserName = userName, Password = password };
            try
            {
                var check = _userManager.Users.FirstOrDefault(x => x.UserName == model.UserName);

                if (check != null)
                {
                    var user = await _userService.Login(model);
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
                    //var token = _tokenService.GenerateJwtToken(authClaims);

                    //await _localStorage.SetItemAsync("token", token);
                    return Ok(new
                        {
                            GeneratedToken = _tokenService.GenerateJwtToken(authClaims)
                        }
                    );
                }
            }
            catch 
            {
                return NotFound("User wasn't found!");
            }

            return NotFound("Something went wrong!");
        }
    }
}
