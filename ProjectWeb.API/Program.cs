using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProjectWeb.Bussiness.Services.AppRoles;
using ProjectWeb.Bussiness.Services.Categories;
using ProjectWeb.Bussiness.Services.Commons;
using ProjectWeb.Bussiness.Services.Messages;
using ProjectWeb.Bussiness.Services.Orders;
using ProjectWeb.Bussiness.Services.Products;
using ProjectWeb.Bussiness.Services.SystemUsers;
using ProjectWeb.Common.Enums;
using ProjectWeb.Common.Extensions;
using ProjectWeb.Common.IServices;
using ProjectWeb.Common.UnitOfWorks;
using ProjectWeb.Data.Entities;
using ProjectWeb.Data.EntityFamework;
using ProjectWeb.Models.FluentValidations.SystemUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using Serilog.Formatting.Json;
using ProjectWeb.Models.CommonModels.Caches;
using ProjectWeb.Bussiness.Caches;
using ProjectWeb.Bussiness.Services.Payments;

var builder = WebApplication.CreateBuilder(args);



//Read Configuration from appSettings
var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();
//Initialize Logger
//Cấu hình = file
Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(config).CreateLogger();

//Cấu hình = tay
/*Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File(new JsonFormatter(),"logs/log.txt", 
        restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
        rollingInterval: RollingInterval.Day)
    .WriteTo.File("logs/errorLog.txt", restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Warning)
    .CreateLogger();*/

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddDbContext<ProjectWebDBContext>(options =>
               options.UseSqlServer(builder.Configuration.GetConnectionString(EnumConstants.SystemsConstants.ConnectionString)));

builder.Services.AddIdentity<SystemUser, AppRole>()
    .AddEntityFrameworkStores<ProjectWebDBContext>().AddDefaultTokenProviders();


//Xài này mới convert json đc.
builder.Services.AddControllers().AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<LoginRequestValidator>()).AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);


//Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Swagger - WebAPI", Version = "v1" });


    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header,
                },
                new List<string>()
            }
    });

});


string issuer = builder.Configuration.GetValue<string>("Tokens:Issuer");
string signingKey = builder.Configuration.GetValue<string>("Tokens:Key");
byte[] signingKeyBytes = System.Text.Encoding.UTF8.GetBytes(signingKey);

//Config Token
var tokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuer = false,
    ValidateAudience = false,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    ClockSkew = System.TimeSpan.Zero,
    IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes),
    RequireExpirationTime = false,
};

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = tokenValidationParameters;
});

//Redis
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["RedisCacheSettings:ConnectionString"];
});



//DI
//services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddLazyResolution();

builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IProductServices, ProductServices>();
builder.Services.AddTransient<IStorageServices, StorageServices>();
builder.Services.AddTransient<ICategoryServices, CategoryServices>();
builder.Services.AddTransient<ISystemUserServices, SystemUserServices>();
builder.Services.AddTransient<IAppRoleServices, AppRoleServices>();
builder.Services.AddTransient<IOrderServices, OrderServices>();
builder.Services.AddTransient<ISendMailServices, SendMailServices>();
builder.Services.AddTransient<IMessageServices, MessageServices>();
builder.Services.AddTransient<IProductCategoriesServices, ProductCategoriesServices>();
builder.Services.AddTransient<IPaymentServices, PaymentServices>();
builder.Services.AddTransient<IPaymentSignatureServices, PaymentSignatureServices>();
builder.Services.AddTransient<UserManager<SystemUser>, UserManager<SystemUser>>();
builder.Services.AddTransient<SignInManager<SystemUser>, SignInManager<SystemUser>>();
builder.Services.AddTransient<RoleManager<AppRole>, RoleManager<AppRole>>();
builder.Services.AddSingleton(tokenValidationParameters);

var redisCacheSettings = new RedisCacheSettings();
builder.Configuration.GetSection(nameof(RedisCacheSettings)).Bind(redisCacheSettings);
builder.Services.AddSingleton(redisCacheSettings);


builder.Services.AddSingleton<IResponseCacheService, ResponseCacheService>();

builder.Services.AddHttpClient();

var app = builder.Build();

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

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger - WebAPI version 1");
});

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});


try
{
    Log.Information("Starting our service....");
    app.Run();
}
catch (Exception ex)
{

    Log.Fatal(ex, "Exception in application");
}
finally
{
    Log.Information("Exiting service");
    Log.CloseAndFlush();
}

