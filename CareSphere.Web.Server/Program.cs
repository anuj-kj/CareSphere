
using CareSphere.Services.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;
using CareSphere.Domains.Events;
using CareSphere.Domains.Orders;
using CareSphere.Services.Orders.Events.Handlers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using CareSphere.Web.Server.Configs;
using Microsoft.EntityFrameworkCore;
using CareSphere.Data.Configurations;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Load configuration settings based on the environment
if (builder.Environment.IsDevelopment())
{
    builder.Configuration
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables();
}
else
{
    builder.Configuration
        .AddEnvironmentVariables();
}
// Configure Logging (Serilog)
SerilogConfiguration.ConfigureSerilog(builder);
// Configure OpenTelemetry Tracing
OpenTelemetryConfiguration.ConfigureOpenTelemetry(builder);
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDataLayer(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
//builder.Services.AddDataLayer(builder.Configuration.GetConnectionString("DefaultConnection"));
builder.Services.AddServiceLayer();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// Register IDomainEventPublisher
builder.Services.AddSingleton<IDomainEventPublisher, DomainEventPublisher>();

// Register OrderEventHandlers
builder.Services.AddScoped<OrderEventHandlers>();

// Configure JWT authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine("OnAuthenticationFailed: " + context.Exception.Message);
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            Console.WriteLine("OnTokenValidated: " + context.SecurityToken);
            return Task.CompletedTask;
        }
    };
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
.AddGoogle(options =>
{
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
});

try
{
    var app = builder.Build();

    // Create a scope to resolve scoped services
    using (var scope = app.Services.CreateScope())
    {
        var scopedServices = scope.ServiceProvider;
        var domainEventPublisher = scopedServices.GetRequiredService<IDomainEventPublisher>();
        var orderEventHandlers = scopedServices.GetRequiredService<OrderEventHandlers>();

        // Register event handlers with the DomainEventPublisher
        domainEventPublisher.RegisterHandler<OrderStatusChangedEvent>(orderEventHandlers.HandleOrderStatusChanged);
        domainEventPublisher.RegisterHandler<OrderItemAddedEvent>(orderEventHandlers.HandleOrderItemAdded);
    }

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }

    app.UseHttpsRedirection();
    app.UseRouting();

    // Enable CORS
    app.UseCors("AllowAllOrigins");

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();
    app.UseStaticFiles();

    app.Run();
}
catch (Exception ex)
{
    var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred during application startup.");
    Console.WriteLine($"An error occurred during application startup: {ex.Message}");
    Console.WriteLine(ex.StackTrace);
    throw;
}

