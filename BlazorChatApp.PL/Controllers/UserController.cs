using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Storage.Blobs;
using BlazorChatApp.BLL.Infrastructure.Interfaces;
using BlazorChatApp.DAL.CustomExtensions;
using BlazorChatApp.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlazorChatApp.PL.Controllers
{
    [Authorize]
    [Route("/api/user")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        public UserController(UserManager<IdentityUser> userManager, IUserService userService, 
            IConfiguration configuration):base(userManager)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [HttpGet("getOtherUsers")]
        public async Task<IEnumerable<IdentityUser>> GetOtherUsers() 
        {
            try
            {
                var currentUser = await GetUserId();
                var users = _userService.GetOtherUsers(currentUser);
                return users;
            }
            catch
            {
                return new List<IdentityUser>();
            }
        }

        [HttpGet("getUserId")] 
        public async Task<string> GetId()
        {
            try
            {
                return await GetUserId();
            }
            catch
            {
                return String.Empty;
            }
        }

        [HttpGet("getUserName")]
        public string GetName()
        {
            try
            {
                return GetUserName();
            }
            catch
            {
                return String.Empty;
            }
        }

        [HttpPost("saveProfile")]
        public async Task<IActionResult> SaveProfile(BrowserImageFile profile)
        {
            try
            {
                var userId  = await GetUserId();
                profile.UserId = userId;
                var containerName = "images";

                SecretClient secretClient =
                    new SecretClient(new Uri(_configuration["VaultUri"]), new DefaultAzureCredential()); // key vault client

                var secret = secretClient.GetSecretAsync("AzureBlobStorage").Result.Value.Value; // secret for blob from key vault

                BlobContainerClient blobContainerClient = new BlobContainerClient(
                    secret, containerName); 

                BlobClient blobClient = blobContainerClient.GetBlobClient(userId + " " + profile.Name);

                var path = Path.Combine(containerName, userId, profile.Name);

                var fs = Convert.FromBase64String(profile.Data);

                var url = blobClient.Uri.AbsoluteUri;

                profile.ImageUrl = url;


                var result = await _userService.SaveProfile(profile);

                if (result)
                {
                    return Ok();
                }
                    

                return BadRequest("Failed to update an info");
            }
            catch
            {
                return BadRequest("Failed to save the image");
            }
        }
    }
}

