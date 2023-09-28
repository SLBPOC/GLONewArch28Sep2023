using Microsoft.EntityFrameworkCore;
using Delfi.Glo.Common.Models;
using Delfi.Glo.Common.Services;
using Delfi.Glo.Entities.Dto;
using Delfi.Glo.PostgreSql.Dal;
using Delfi.Glo.PostgreSql.Dal.Services;
using Delfi.Glo.Repository;
using Serilog;
using Delfi.Glo.Api.Configuration;
using NLog;
using NLog.Web;
using Delfi.Glo.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//configure Serilog
//builder.Services.AddLogging(logginBuilder => logginBuilder.AddSerilog());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

EnvironmentVariables environmentVariables = new();
environmentVariables.DbConnectionString = builder.Configuration.GetConnectionString("PostgreSqlConnectionString");
environmentVariables.ReadEnvironmentVariables();
builder.Services.AddSingleton(environmentVariables);

builder.Services.AddDbContext<ApplicationContext>(option =>
    option.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSqlConnectionString")));

GlobalDiagnosticsContext.Set("connectionString", builder.Configuration.GetConnectionString("PostgreSqlConnectionString"));
// Add NLog for Logging
builder.Logging.ClearProviders();
builder.Host.UseNLog();

// TODO: add custom services to container
builder.Services.AddCoreServices(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder => builder
        .WithOrigins("http://localhost:4200")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials()
        .WithExposedHeaders("Content-Disposition")
        .SetIsOriginAllowed(url => true));
});

builder.Services.AddHealthChecks();

builder.Logging.AddSerilog();

if (environmentVariables.IsAuthenticationRequired)
{
    builder.Services.AddSAuthAuthentication();
    builder.Services.AddSwaggerSAuthentication();
}

builder.Services.AddHsts(options =>
{
    options.MaxAge = TimeSpan.FromHours(1);
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

//app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

//app.UseHttpsRedirection();
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("Content-Security-Policy", "default-src 'self';");
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Add("X-Frame-Options", "DENY");
    context.Response.Headers.Add("X-Xss-Protection", "1");
    await next();
});

app.Run();
