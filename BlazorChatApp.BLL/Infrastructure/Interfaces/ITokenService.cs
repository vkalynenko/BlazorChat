using System.Security.Claims;

namespace BlazorChatApp.BLL.Infrastructure.Interfaces
{
    public interface ITokenService
    {
        string GenerateJwtToken(List<Claim> authClaims);
    }
}
