using backend.Contracts;
using backend.Database;
using backend.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<OwtBoatsDbContext>(o =>
{
    o.UseNpgsql(builder.Configuration.GetConnectionString("OwtBoatsDb"));
});

// Auth
builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<OwtBoatsUser>()
    .AddEntityFrameworkStores<OwtBoatsDbContext>();

// Developer Tools, misc
builder.Services.AddOpenApi();

// Our app: Endpoints
builder.Services
    .AddScoped<backend.Endpoints.Index>()
    .AddScoped<backend.Endpoints.Create>()
    .AddScoped<backend.Endpoints.Get>()
    .AddScoped<backend.Endpoints.Update>()
    .AddScoped<backend.Endpoints.Delete>();

// Our app: Validators
builder.Services
    .AddScoped<IValidator<BoatInfo>, BoatInfoValidator>()
    .AddScoped<IValidator<BoatCreateRequest>, BoatCreateValidator>();

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

// our endpoints
var boatsGroup = app.MapGroup("/boats").RequireAuthorization();

boatsGroup.MapGet("/", ([FromServices] backend.Endpoints.Index endpoint) => endpoint.ListBoats());
boatsGroup.MapPost("/", ([FromBody] BoatCreateRequest boat, [FromServices] backend.Endpoints.Create endpoint) => endpoint.CreateBoat(boat));
boatsGroup.MapGet("/{id:Guid}", (Guid id, [FromServices] backend.Endpoints.Get endpoint) => endpoint.GetBoat(id));
boatsGroup.MapPut("/{id:Guid}", (Guid id, [FromBody] BoatInfo boat, [FromServices] backend.Endpoints.Update endpoint) => endpoint.UpdateBoat(id, boat));
boatsGroup.MapDelete("/{id:Guid}", (Guid id, [FromServices] backend.Endpoints.Delete endpoint) => endpoint.DeleteBoat(id));

// Apply migrations automatically on startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<OwtBoatsDbContext>();
    await dbContext.Database.MigrateAsync();
}

await app.RunAsync();