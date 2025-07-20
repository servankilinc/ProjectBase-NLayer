using Autofac;
using Autofac.Extensions.DependencyInjection;
using Business;
using Core;
using Core.Utils.Auth;
using DataAccess;
using DataAccess.Contexts;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Model;
using Model.Entities;
using Serilog;
using Serilog.Filters;
using System.Threading.RateLimiting;
using WebUI.ExceptionHandler;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters(); 



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
        slidingOptions.PermitLimit = 15;
        slidingOptions.Window = TimeSpan.FromSeconds(3);
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
builder.Services.AddSingleton<TokenSettings>(new TokenSettings());

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


// ------- Cookie Options -------
builder.Services.ConfigureApplicationCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.SlidingExpiration = true;
    options.AccessDeniedPath = "/Error/Forbidden";
    options.LogoutPath = "/Account/Logout";
    options.LoginPath = "/Account/Login";
    options.Cookie = new()
    {
        Name = "IdentityCookie",
        HttpOnly = true,
        SameSite = SameSiteMode.Lax,
        SecurePolicy = CookieSecurePolicy.Always
    };
});
// ------- Cookie Options -------


// ------- Action Filters -------
builder.Services.AddScoped(typeof(WebUI.Utils.ActionFilters.ValidationFilter<>));
// ------- Action Filters -------


var app = builder.Build();

app.UseMiddleware<ExceptionHandleMiddleware>();

//app.UseStaticFiles();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/InvalidProcess");

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/Error/NotFound");

app.UseHttpsRedirection();

app.UseCors("policy_cors");

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.UseRateLimiter();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets()
    .RequireRateLimiting("policy_rate_limiter");


app.Run();
