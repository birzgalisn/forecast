using Api;
using Api.Contexts;
using Api.Hubs;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services
    .AddSignalR()
    .AddStackExchangeRedis(
        Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING")
        ?? builder.Configuration.GetConnectionString("Redis")
        ?? ""
    );

builder.Services
    .AddDbContextPool<ForecastDbContext>(options =>
        options.UseNpgsql(
            Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")
            ?? builder.Configuration.GetConnectionString("Db")
            ?? ""
        )
    );
builder.Services.AddTransient<IStartupFilter, DataContextAutomaticMigrationStartupFilter<ForecastDbContext>>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(options => options
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true)
    .AllowCredentials());

app.UseAuthorization();

app.MapControllers();

app.MapHub<ForecastHub>("/hub");

app.Urls.Add("http://+:4000");

app.Run();
