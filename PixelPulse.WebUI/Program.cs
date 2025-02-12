using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PixelPulse.Application.Interfaces.Repositories;
using PixelPulse.Application.Mappers;
using PixelPulse.Domain.Entities;
using PixelPulse.Infrastructure.Content.Data;
using PixelPulse.Infrastructure.Content.Seeds;
using PixelPulse.Infrastructure.Services.Repositories;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppIdentityContext>(options =>
    options.UseSqlServer(connectionString));

//builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));

builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
    {
        options.Password.RequiredLength = 8;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
    })
    .AddEntityFrameworkStores<AppIdentityContext>()
    .AddDefaultTokenProviders();

var config = TypeAdapterConfig.GlobalSettings;
config.Scan(typeof(RegisterMapper).Assembly);

builder.Services.AddSingleton(config);
builder.Services.AddScoped<IMapper, ServiceMapper>();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

builder.Services.AddMemoryCache();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo {Title = "PixelPulse", Version = "v1"});
});
builder.Services.AddResponseCompression();

var app = builder.Build();
app.UseResponseCompression();

using (IServiceScope scope = app.Services.CreateAsyncScope())
{
    var service = scope.ServiceProvider;
    var loggerFactory = service.GetRequiredService<ILoggerFactory>();
    try
    {
        var context = service.GetRequiredService<AppIdentityContext>();
        var userManager = service.GetRequiredService<UserManager<AppUser>>();
        var roleManager = service.GetRequiredService<RoleManager<IdentityRole>>();
        
        await DefaultRoles.SeedRoles(roleManager);
        await DefaultUsers.SeedUsers(userManager);
    }
    catch (Exception ex)
    {
        var logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}")
    .WithStaticAssets();

app.MapStaticAssets();

await app.RunAsync();