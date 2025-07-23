using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using PersonalWebsite.Constants;
using PersonalWebsite.Data;
using PersonalWebsite.Middleware;
using PersonalWebsite.Models;
using PersonalWebsite.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton<IDataCacheService<SiteSetting>, SiteSettingService>();
builder.Services.AddSingleton<IDataCacheService<SocialMedia>, SocialMediaService>();
builder.Services.AddSingleton<IDataCacheService<PageSetting>, PageSettingService>();
builder.Services.AddSingleton<IDataCacheService<PageContent>, PageContentService>();

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder(Routes.Admin.Root);
    options.Conventions.AllowAnonymousToPage(Routes.Admin.Login);
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = Routes.Admin.Login;
    });

builder.Services.AddAuthorization();
builder.Services.AddHttpClient();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();

    if (!db.SiteSettings.Any())
    {
        db.SiteSettings.AddRange(
            new SiteSetting(SettingNames.WebsiteName, "PersonalWebsite"),
            new SiteSetting(SettingNames.OwnerName, "Your Name")
        );

        db.SaveChanges();
    }
}

app.UseStatusCodePagesWithReExecute(Routes.Public.Error, "?code={0}");

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler($"{Routes.Public.Error}?code=500");
    app.UseHsts();
}

app.UseMiddleware<MaintenanceMiddleware>();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
