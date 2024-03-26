using Api;
using Api.Contexts;
using Api.Hubs;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSignalR();

builder.Services.AddDbContext<ForecastDbContext>(options => options.UseNpgsql(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ?? builder.Configuration.GetConnectionString("Db")));
builder.Services.AddTransient<IStartupFilter, DataContextAutomaticMigrationStartupFilter<ForecastDbContext>>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(options => options
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // Allow any origin
    .AllowCredentials()); // Allow credentials

app.UseAuthorization();

app.MapControllers();

app.MapHub<ForecastHub>("/hub");

app.Run();
