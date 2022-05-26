using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BlazorChatApp.DAL.Domain.Entities;

namespace BlazorChatApp.BLL.Infrastructure.Interfaces
{
    public interface ITokenService
    {
        string GenerateJwtToken(List<Claim> authClaims);
    }
}
