using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProjectWeb.APIServices.IServiceBackendAPIs;
using ProjectWeb.APIServices.Services;
using ProjectWeb.Models.FluentValidations.SystemUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);
//Add Services
builder.Services.AddHttpClient();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/SystemUser/Login/";
        options.AccessDeniedPath = "/SystemUser/Forbidden";
        options.Cookie.IsEssential = true;
        options.Cookie.HttpOnly = true;

    });

builder.Services.AddControllersWithViews().AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<LoginRequestValidator>()); ;

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
builder.Services.AddTransient<IRoleBackendAPI, RoleBackendAPI>();
builder.Services.AddTransient<IProductBackendAPI, ProductBackendAPI>();
builder.Services.AddTransient<ICategoryBackendAPI, CategoryBackendAPI>();

IMvcBuilder builder1 = builder.Services.AddRazorPages();
var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
if (env == Environments.Development)
{
    builder1.AddRazorRuntimeCompilation();
}


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
app.UseCookiePolicy();
app.UseSession();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();