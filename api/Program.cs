using Api;
using Api.Clients;
using Api.Contexts;
using Api.Hubs;
using Api.Services;
using Microsoft.EntityFrameworkCore;

var mutex = new Mutex(true, "ForecastApi");

if (!mutex.WaitOne(TimeSpan.Zero, true))
{
    return;
}

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddControllers();

    builder.Services.AddSignalR();

    builder.Services.AddDbContext<ForecastDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("Db")));
    builder.Services.AddTransient<IStartupFilter, DataContextAutomaticMigrationStartupFilter<ForecastDbContext>>();

    builder.Services.AddSingleton<IForecastAggregationClient, ForecastAggregationClient>();
    builder.Services.AddSingleton<IForecastAggregationService, ForecastAggregationService>();

    builder.Services.AddSingleton<FahrenheitSignalerService>();
    builder.Services.AddHostedService<FahrenheitWorkerService>();

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
}
finally
{
    mutex.ReleaseMutex();
}
