using BlazorChatApp.BLL.Contracts.DTOs;
using BlazorChatApp.BLL.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BlazorChatApp.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IAuthorizationService _authorizationService;

        public AccountController(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(string userName, string password)
        {
            LoginDto model = new LoginDto
            {
                UserName = userName,
                Password = password,
            };
            try
            {
                await _authorizationService.Login(model);
            }
            catch
            {
                return NotFound("User was not found");
            }
            
            return RedirectToPage("/chats");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _authorizationService.LogOut();
            }
            catch
            {
                return BadRequest("Something went wrong");
            }
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Register(
            string userName, string password, string confirmedPassword)
        {
            if (password != confirmedPassword)
            {
                return BadRequest("Passwords are not match");
            }
            try
            {
                var model = new RegisterDto
                {
                    UserName = userName,
                    Password = password,
                };
                await _authorizationService.Register(model);
            }
            catch
            {
                return BadRequest("User wasn't registered!");
            }
            
            return RedirectToAction("Login");
            
        }
    }
}
