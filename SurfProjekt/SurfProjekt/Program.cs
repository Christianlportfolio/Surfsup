using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SurfProjekt.Data;
using System.Globalization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Authentication.Facebook;
using SurfProjekt.Models;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

//Added authentication service for Facebook and Google.

services.AddAuthentication()
    .AddGoogle(options =>
    {
        options.ClientId = "1013365460994-ffu66q30blac5ak63sgg8qsmmmuagv4a.apps.googleusercontent.com";
        options.ClientSecret = "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";
    })
.AddFacebook(options =>
    {
        options.AppId = "1857945164555064";
        options.AppSecret = "87f363592bf4b7a3d94f9ca823d33a82";
    });

builder.Services.AddDbContext<SurfProjektContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SurfProjektContext") ?? throw new InvalidOperationException("Connection string 'SurfProjektContext' not found.")));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<SurfProjektContext>();
    

// Add services to the container.
builder.Services.AddControllersWithViews();

//Denne linje sørge at man kan tilføje flere 'policies'
builder.Services.AddAuthorization(options =>
{
    //Her tilføjer man den specikke 'policy'. Metoden AddPolicy kommer fra klassen AuthorizationOptions, som har til formål at konfigurere forskellige policies.
    //Metoden AddPolicy tager et string-navn, så den ved, hvad policy'en skal hedde - og så kan man bruge den til at administere, hvad brugeren har adgang til. Altså om man fx er admin eller user.
    //Metoden RequireRole sørger for at man udpensler, hvilke roller, der har adgang til policy'en. Der kan altså godt være flere roller til en policy. Fx kunne man have en manager rolle, der også havde adgang til det samme som admin.
    options.AddPolicy(ConstantsRole.Policies.RequireAdmin, policy => policy.RequireRole(ConstantsRole.Roles.Admin));
    options.AddPolicy(ConstantsRole.Policies.RequireUser, policy => policy.RequireRole(ConstantsRole.Roles.User));

});


var app = builder.Build();

//Vi sætter programmets sprog til dansk for at den anvender komma i stedet for punktum. 
var defaultDateCulture = "da-DK";
var ci = new CultureInfo(defaultDateCulture);
ci.NumberFormat.NumberDecimalSeparator = ".";
ci.NumberFormat.CurrencyDecimalSeparator = ".";

// Configure the Localization middleware
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(ci),
    SupportedCultures = new List<CultureInfo>
    {
        ci,
    },
    SupportedUICultures = new List<CultureInfo>
    {
        ci,
    }
});

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
app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.Run();

