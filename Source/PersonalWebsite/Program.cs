using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using PersonalWebsite.Constants;
using PersonalWebsite.Data;
using PersonalWebsite.Models;
using PersonalWebsite.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton<IDBContent<SiteSetting>, SiteSettingProvider>();
builder.Services.AddSingleton<IDBContent<PageContent>, PageContentProvider>();

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

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();

    if (!db.SiteSettings.Any())
    {
        db.SiteSettings.AddRange(
            new SiteSetting { SettingName = SettingNames.WebsiteName, Value = "PersonalWebsite" },
            new SiteSetting { SettingName = SettingNames.OwnerName, Value = "Your Name" }
        );

        db.SaveChanges();
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler(Routes.Public.Error);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
