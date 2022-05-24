//using BlazorChatApp.BLL.Contracts.DTOs;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;

//namespace BlazorChatApp.PL.Controllers
//{
//    [Authorize]
//    [Route("api/[controller]")]
//    [ApiController]
//    public class LoginController : ControllerBase
//    {
//        private readonly IConfiguration _configuration;
//        private readonly UserManager<IdentityUser> _userManager;
//        private readonly SignInManager<IdentityUser> _signInManager;

//        public LoginController(IConfiguration configuration)
//        {
//            _configuration = configuration;
//        }

//        [AllowAnonymous]
//        [HttpGet]
//        public async Task<IActionResult> Login(string userName, string password)
//        {
//            UserDto login = new UserDto();
//            login.UserName = userName;
//            login.Password = password;

//            IActionResult responce = Unauthorized();
//            var user = AuthenticateUserAsync(login);
//            if (user == null)
//            {
//                var tokenString = GenerateJSONWebToken(login);
//                responce = Ok(new { token = tokenString });
//            }
//            return responce;
//        }
//        private async Task<UserDto> AuthenticateUserAsync(UserDto login)
//        {
//            var result = await _signInManager
//                .PasswordSignInAsync(login.UserName, login.Password, true, false);
//            if (result.Succeeded)
//            {
//               // await _signInManager.SignInAsync();
//                return login;
//            }
//            login = null;
//            return login;
//        }

//        private string GenerateJSONWebToken(UserDto userInfo)
//        {
//            var tokenConfig = _configuration["Jwt:Issuer"];

//            var securityKey = new SymmetricSecurityKey(Encoding
//                .UTF8.GetBytes(_configuration["Jwt:Key"]));
//            var credentials = new SigningCredentials(securityKey,
//                SecurityAlgorithms.HmacSha256);

//            var claims = new[]{

//                new Claim(JwtRegisteredClaimNames.Sub, userInfo.UserName),
//                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),


//            };

//            var token = new JwtSecurityToken(
//                issuer: tokenConfig,
//                audience: tokenConfig,
//                claims,
//                expires: DateTime.Now.AddDays(1),
//                signingCredentials: credentials

//                );

//            var encodetoken = new JwtSecurityTokenHandler().WriteToken(token);
//            return encodetoken;

//        }
//    }
//}
