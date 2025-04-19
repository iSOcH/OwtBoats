using backend.Database;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddDbContext<OwtBoatsDbContext>(o =>
{
    o.UseNpgsql(builder.Configuration.GetConnectionString("OwtBoatsDb"));
});

// Auth
builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<OwtBoatsUser>()
    .AddEntityFrameworkStores<OwtBoatsDbContext>();

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGroup("/auth").MapIdentityApi<OwtBoatsUser>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(swaggerUiOptions =>
    {
        // url needs to be specified since we are using the new "MapOpenApi" and not "old" UseSwagger 
        swaggerUiOptions.SwaggerEndpoint("../openapi/v1.json", "OWT Boats API");
    });
}

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast");

// Apply migrations automatically on startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<OwtBoatsDbContext>();
    await dbContext.Database.MigrateAsync();
}

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}