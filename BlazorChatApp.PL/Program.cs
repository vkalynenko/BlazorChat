using BlazorChatApp.BLL.Contracts.DTOs;
using BlazorChatApp.BLL.Infrastructure.Interfaces;
using BlazorChatApp.BLL.Infrastructure.Services;
using BlazorChatApp.DAL.Data.Interfaces;
using BlazorChatApp.DAL.Data.Repositories;
using BlazorChatApp.DAL.Domain.EF;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BlazorChatApp.BLL.Hubs;
using BlazorChatApp.BLL.MainRequestServices.Interfaces;
using BlazorChatApp.BLL.Models;
using BlazorChatApp.BLL.RequestServices.Services;
using BlazorChatApp.DAL.Data;
using Blazored.LocalStorage;
using Blazored.Toast;
using Microsoft.AspNetCore.ResponseCompression;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration
    .GetConnectionString("ConnectionString") ??
                       throw new InvalidOperationException
                           ("Connection string 'ConnectionString' not found.");
builder.Services.AddDbContext<BlazorChatAppContext>(options =>
    options.UseSqlServer(connectionString)); ;

builder.Services.AddRazorPages();
builder.Services.AddSignalRCore();
builder.Services.AddServerSideBlazor();
builder.Services.AddControllers().AddNewtonsoftJson(
    options => {
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    }); ;


builder.Services.AddTransient<IMessageRepository, MessageRepository>();
builder.Services.AddTransient<IChatRepository, ChatRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();

builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<IChatService, ChatService>();
builder.Services.AddTransient<IMessageService, MessageService>();

builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

builder.Services.AddTransient<IRequestChatService, RequestChatService>();
builder.Services.AddTransient<IRequestMessageService, RequestMessageService>();
builder.Services.AddTransient<IRequestUserService, RequestUserService>();

builder.Services.AddTransient<LoginDto>();
builder.Services.AddTransient<RegisterDto>();
builder.Services.AddTransient<MessageDto>();

builder.Services.AddTransient<ReplyToGroupModel>();
builder.Services.AddTransient<ReplyToUserModel>();
builder.Services.AddTransient<RoomModel>();

builder.Services.AddTransient<UserManager<IdentityUser>>();

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<BlazorChatAppContext>()
    .AddDefaultTokenProviders();

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazoredToast();

builder.Services.AddHttpClient("Authorization",  client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Url:Route"]);
});

builder.Services.AddSingleton<HttpClient>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})

    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JWT:ValidAudience"],
            ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
        };
    });

builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults
        .MimeTypes.Concat(new[] {"application/octet-stream"});

});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{

    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseCookiePolicy();

app.UseEndpoints(endpoints =>
{
    endpoints.MapBlazorHub();
    endpoints.MapFallbackToPage("/_Host");
    endpoints.MapControllers();
    endpoints.MapHub<ChatHub>("/chatHub");
});

app.Run();