using AutoMapper;
using BlazorChatApp.BLL.Contracts.DTOs;
using Microsoft.AspNetCore.Identity;

namespace BlazorChatApp.BLL.Mappings
{
    public class MappingProfile  : Profile
    {
        public MappingProfile()
        {
            CreateMap<IdentityUser, LoginDto>().ReverseMap();
            CreateMap<IdentityUser, RegisterDto>().ReverseMap();    
        }
    }
}
