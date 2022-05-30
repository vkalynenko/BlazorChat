using System.Net.Http.Headers;
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
using BlazorChatApp.DAL.Data;
using Blazored.LocalStorage;

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
builder.Services.AddControllersWithViews();

builder.Services.AddTransient<IMessageRepository, MessageRepository>();
builder.Services.AddTransient<IChatRepository, ChatRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();

builder.Services.AddTransient<IRequestService, RequestService>();
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<IChatService, ChatService>();

builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

builder.Services.AddSingleton<LoginDto>();
builder.Services.AddSingleton<RegisterDto>();

builder.Services.AddTransient<UserManager<IdentityUser>>();

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<BlazorChatAppContext>()
    .AddDefaultTokenProviders();

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddHttpClient("Authorization",  client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Url:Route"]);
    //var token = await Activator.CreateInstance<ILocalStorageService>().GetItemAsync<string>("token");
   // client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

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
    endpoints.MapControllers();
    endpoints.MapBlazorHub();
    endpoints.MapDefaultControllerRoute(); 
    endpoints.MapFallbackToPage("/_Host");
});

app.Run();