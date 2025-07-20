using API.ExceptionHandler;
using API.Utils;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Business;
using Core;
using Core.Utils.Auth;
using DataAccess;
using DataAccess.Contexts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using Model;
using Model.Entities;
using Scalar.AspNetCore;
using Serilog;
using Serilog.Filters;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);


// ------- CORS -------
builder.Services.AddCors(options =>
{
    options.AddPolicy("policy_cors", builder =>
    {
        builder
            .AllowAnyOrigin()
            //.WithOrigins("https://www.frontend.com")
            //.AllowCredentials() // AllowAnyOrigin and AllowCredentials cannot using together use with WithOrigins option 
            .WithHeaders("Content-Type", "Authorization")
            .AllowAnyMethod()
            .SetPreflightMaxAge(TimeSpan.FromMinutes(10));
    });
});
// ------- CORS -------


// ------- Rate Limiter -------
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.AddSlidingWindowLimiter(policyName: "policy_rate_limiter", slidingOptions =>
    {
        slidingOptions.PermitLimit = 30;
        slidingOptions.Window = TimeSpan.FromSeconds(5);
        slidingOptions.SegmentsPerWindow = 4;
        slidingOptions.QueueLimit = 5;
        slidingOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });
});
// ------- Rate Limiter -------


// ------- Logger Implementation -------
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Logger(lc => lc
        .Filter.ByIncludingOnly(Matching.WithProperty("Target", (object p) => p.ToString() == "Validation"))
        .WriteTo.File("Logs/Validation/validation.log", rollingInterval: RollingInterval.Day,
            outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}"))
    .WriteTo.Logger(lc => lc
        .Filter.ByIncludingOnly(Matching.WithProperty("Target", (object p) => p.ToString() == "Application"))
        .WriteTo.File("Logs/Application/application.log", rollingInterval: RollingInterval.Day,
            outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}"))
    .WriteTo.Logger(lc => lc
        .Filter.ByIncludingOnly(Matching.WithProperty("Target", (object p) => p.ToString() == "Business"))
        .WriteTo.File("Logs/Business/business.log", rollingInterval: RollingInterval.Day,
            outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}"))
    .WriteTo.Logger(lc => lc
        .Filter.ByIncludingOnly(Matching.WithProperty("Target", (object p) => p.ToString() == "DataAccess"))
        .WriteTo.File("Logs/DataAccess/dataAccess.log", rollingInterval: RollingInterval.Day,
            outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}"))
    .WriteTo.Logger(lc => lc
        .Filter.ByExcluding(Matching.WithProperty<string>("Target", _ => true))
        .WriteTo.File("Logs/Other/others.log", rollingInterval: RollingInterval.Day,
            outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}"))
    .CreateLogger();

builder.Host.UseSerilog();
// ------- Logger Implementation -------


// ------- Layer Registrations -------
builder.Services.AddModelServices();
builder.Services.AddCoreServices(builder.Configuration);
builder.Services.AddDataAccessServices(builder.Configuration);
builder.Services.AddBusinessServices(builder.Configuration);
// ------- Layer Registrations -------


// ------- Autofac Modules -------
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(builder =>
    {
        builder.RegisterModule(new Core.AutofacModule());
        builder.RegisterModule(new DataAccess.AutofacModule());
        builder.RegisterModule(new Business.AutofacModule());
    });
// ------- Autofac Modules -------


// ------- IDENTITY -------
builder.Services
    .AddIdentity<User, IdentityRole<Guid>>(options =>
    {
        // Default Lockout settings.
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.AllowedForNewUsers = true;

        options.SignIn.RequireConfirmedEmail = false;

        options.Password.RequiredLength = 4;
        options.Password.RequireDigit = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;

        options.User.RequireUniqueEmail = false;
        options.User.AllowedUserNameCharacters = "abcçdefgðhiýjklmnoöpqrsþtuüvwxyzABCÇDEFGÐHIÝJKLMNOÖPQRSÞTUÜVWXYZ0123456789-._@+/*|!,;:()&#?[] ";
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthorization();
// ------- IDENTITY -------


// ------- JWT Implementation -------
TokenSettings tokenSettings = builder.Configuration.GetSection("TokenSettings").Get<TokenSettings>()!;
builder.Services.AddSingleton(tokenSettings);

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidIssuer = tokenSettings.Issuer,
            ValidAudience = tokenSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(tokenSettings.SecurityKey))
        };
    });
// ------- JWT Implementation -------


builder.Services.AddHealthChecks();

builder.Services.AddControllers();

builder.Services.AddOpenApi(options => {
    options.AddDocumentTransformer<ScalarSecuritySchemeTransformer>();
});

var app = builder.Build();

app.UseMiddleware<ExceptionHandleMiddleware>();

//app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseCors("policy_cors");

app.UseAuthentication();

app.UseAuthorization();

app.UseRateLimiter();

app.MapControllers().RequireRateLimiting("policy_rate_limiter");

app.MapHealthChecks("/health");

app.Run();
