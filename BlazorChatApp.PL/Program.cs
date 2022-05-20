using AutoMapper;
using BlazorChatApp.BLL.Hubs;
using BlazorChatApp.BLL.Mappings;
using BlazorChatApp.DAL.Domain.EF;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration
    .GetConnectionString("ConnectionString") ?? 
                       throw new InvalidOperationException
                           ("Connection string 'ConnectionString' not found.");
builder.Services.AddDbContext<BlazorChatAppContext>(options =>
    options.UseSqlServer(connectionString)); ;
builder.Services
    .AddIdentity<IdentityUser, IdentityRole>(options => options
        .SignIn.RequireConfirmedEmail = false)
    .AddEntityFrameworkStores<BlazorChatAppContext>();

builder.Services.AddRazorPages();
builder.Services.AddSignalRCore();
builder.Services.AddServerSideBlazor();
builder.Services.AddControllersWithViews();

var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
});

var mapper = mapperConfig.CreateMapper();
var app = builder.Build();


if (!app.Environment.IsDevelopment())
{

    app.UseExceptionHandler("/Error");
    app.UseHsts();
}


app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseAuthentication();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapBlazorHub();
    endpoints.MapFallbackToPage("/_Host");
});
//app.UseEndpoints(endpoints =>
//    {
//        endpoints.MapControllerRoute(
//            name: "default",
//            pattern: "{controller=Home}/{action=Index}/{id?}");
//        endpoints.MapHub<ChatHub>("/chatHub");
//    }
//);
app.Run();