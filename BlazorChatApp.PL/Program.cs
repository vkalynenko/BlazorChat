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
using BlazorChatApp.BLL.Models;
using BlazorChatApp.BLL.RequestServices.Interfaces;
using BlazorChatApp.BLL.RequestServices.Services;
using BlazorChatApp.DAL.Data;
using BlazorChatApp.DAL.Models;
using Blazored.LocalStorage;
using Blazored.Toast;
using Microsoft.AspNetCore.ResponseCompression;
using Newtonsoft.Json;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using BlazorChatApp.DAL.CustomExtensions;
using BlazorChatApp.PL.Controllers;

var builder = WebApplication.CreateBuilder(args);

var uri = builder.Configuration["VaultUri"];
var client = new SecretClient(new Uri(uri), new DefaultAzureCredential());
var connectionString = await client.GetSecretAsync("ConnectionString");
//var signalRConnection = await client.GetSecretAsync("SignalRApp");       //comment     

builder.Services.AddDbContext<BlazorChatAppContext>(options =>
    options.UseSqlServer(connectionString.Value.Value)); 
builder.Services.AddLogging();
builder.Services.AddRazorPages();

//builder.Services.AddSignalRCore().AddAzureSignalR(signalRConnection.Value.Value); //comment
builder.Services.AddServerSideBlazor();
builder.Services.AddControllers().AddNewtonsoftJson(
    options => {
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    });

builder.Services.AddTransient<IMessageRepository, MessageRepository>();
builder.Services.AddTransient<IChatRepository, ChatRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();

builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<IChatService, ChatService>();
builder.Services.AddTransient<IMessageService, MessageService>();

builder.Services.AddTransient<UserController>();

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
builder.Services.AddTransient<ProfileModel>();

builder.Services.AddTransient<BrowserImageFile>();

builder.Services.AddTransient<UserManager<IdentityUser>>();

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
    {
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireDigit = false;
        options.Password.RequireUppercase = false;
    })
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
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
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

    //app.UseAzureSignalR(endpoints =>
    //{
    //    endpoints.MapHub<ChatHub>("/chatHub");
    //});

    endpoints.MapHub<ChatHub>("/chatHub");
});
app.Run();