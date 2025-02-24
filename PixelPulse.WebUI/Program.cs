using PixelPulse.Domain.Settings;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));

builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    
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
/*app.UseAuthorization();*/

//app.MapStaticAssets();
app.UseStaticFiles();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}")
    .WithStaticAssets();

await app.RunAsync();