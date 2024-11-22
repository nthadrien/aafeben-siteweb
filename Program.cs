using System.Globalization;
using Aafeben.Data;
using Aafeben.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AafebenDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultAafeben")));

// addcookies auth ---------------------
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(240);
        options.SlidingExpiration = true;
        options.AccessDeniedPath = "/Forbidden/";
        options.LogoutPath = "/";
        options.LoginPath = "/{*culture}/administrateurs";
    });

// i18n Localizers

var supportedCultures = new[] { new CultureInfo(name:"en"), new CultureInfo(name:"fr") };
var supportedCulturesString = new[] { 
    "en","fr"
};

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddMvc()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization();


builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { "en", "fr" };
    options.SetDefaultCulture(supportedCultures[0])
        .AddSupportedCultures(supportedCultures)
        .AddSupportedUICultures(supportedCultures);
});

// -------------------------------

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// --------------- seed data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    SeedData.Initialize(services);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Auth notified
app.UseAuthentication();
app.UseAuthorization();


// i18n middleware 

var requestProvider = new RouteDataRequestCultureProvider();
 
var requestLocalizationOptions = new RequestLocalizationOptions 
{
    DefaultRequestCulture = new RequestCulture("fr"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures,
};

requestLocalizationOptions.RequestCultureProviders.Insert(index: 0, requestProvider );
app.UseRequestLocalization(requestLocalizationOptions);

// ----------------------------------------
var cookiePolicyOptions = new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
};

app.UseCookiePolicy(cookiePolicyOptions);

app.MapControllerRoute(
    name: "default",
    pattern: "{culture=fr}/{controller=Home}/{action=Index}/{id?}"
);

app.Run();
