﻿using Microsoft.AspNetCore.Identity;

namespace BlazorChatApp.BLL.Responses
{
    public class GetAllUsersResponse : BaseResponse
    {
        public  Task<List<IdentityUser>?>? Users { get; set; }
    }
}
