using FluentValidation.AspNetCore;
using LazZiya.ExpressLocalization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProjectWeb.APIServices.IServiceBackendAPIs;
using ProjectWeb.APIServices.Services;
using ProjectWeb.EcommerceApp.LocalizationResources;
using ProjectWeb.Models.FluentValidations.SystemUsers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
var cultures = new[]
{
                new CultureInfo("en"),
                new CultureInfo("vi"),
            };

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login/";
        options.AccessDeniedPath = "/Account/Forbidden";
    });

builder.Services.AddControllersWithViews()
    .AddExpressLocalization<ExpressLocalizationResource, ViewLocalizationResource>(ops =>
    {
                    // When using all the culture providers, the localization process will
                    // check all available culture providers in order to detect the request culture.
                    // If the request culture is found it will stop checking and do localization accordingly.
                    // If the request culture is not found it will check the next provider by order.
                    // If no culture is detected the default culture will be used.

                    // Checking order for request culture:
                    // 1) RouteSegmentCultureProvider
                    //      e.g. http://localhost:1234/tr
                    // 2) QueryStringCultureProvider
                    //      e.g. http://localhost:1234/?culture=tr
                    // 3) CookieCultureProvider
                    //      Determines the culture information for a request via the value of a cookie.
                    // 4) AcceptedLanguageHeaderRequestCultureProvider
                    //      Determines the culture information for a request via the value of the Accept-Language header.
                    //      See the browsers language settings

                    // Uncomment and set to true to use only route culture provider
        ops.UseAllCultureProviders = false;
        ops.ResourcesPath = "LocalizationResources";
        ops.RequestLocalizationOptions = o =>
        {
            o.SupportedCultures = cultures;
            o.SupportedUICultures = cultures;
            o.DefaultRequestCulture = new RequestCulture("vi");
        };
    });

builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    // You might want to only set the application cookies over a secure connection:
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.HttpOnly = true;
    // Make the session cookie essential
    options.Cookie.IsEssential = true;
});

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<ISystemUserBackendAPI, SystemUserBackendAPI>();
builder.Services.AddTransient<IProductBackendAPI, ProductBackendAPI>();
builder.Services.AddTransient<ICategoryBackendAPI, CategoryBackendAPI>();
builder.Services.AddTransient<IOrderBackendAPI, OrderBackendAPI>();
builder.Services.AddTransient<IMessageBackendAPI, MessageBackendAPI>();
IMvcBuilder builder1 = builder.Services.AddRazorPages();
var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
if (env == Environments.Development)
{
    builder1.AddRazorRuntimeCompilation();
}

builder.Services.AddControllersWithViews().AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<LoginRequestValidator>()); ;


var app = builder.Build();
//Middleware 
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.UseSession();
app.UseRequestLocalization();
app.UseEndpoints(endpoints =>
{

    endpoints.MapControllerRoute(
         name: "Categories EN",
         pattern: "{culture}/categories/{id?}", new
         {
             controller = "Category",
             action = "Detail"
         });

    endpoints.MapControllerRoute(
        name: "Categories VN",
        pattern: "{culture}/danh-muc/{id?}", new
        {
            controller = "Category",
            action = "Detail"
        });

    endpoints.MapControllerRoute(
         name: "Products EN",
         pattern: "{culture}/products/{id?}", new
         {
             controller = "Product",
             action = "Detail"
         });

    endpoints.MapControllerRoute(
        name: "Products VN",
        pattern: "{culture}/san-pham/{id?}", new
        {
            controller = "Product",
            action = "Detail"
        });


    endpoints.MapControllerRoute(
     name: "default",
     pattern: "{culture=vi}/{controller=Home}/{action=Index}/{id?}");
});

app.Run();