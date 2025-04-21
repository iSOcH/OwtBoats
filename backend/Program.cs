using backend.Contracts;
using backend.Database;
using backend.Services;
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
builder.Services.AddScoped<IUserService, UserService>();

// Developer Tools, misc
builder.Services.AddOpenApi();
builder.Services.AddHttpContextAccessor();

// Our app: Endpoints
builder.Services
    .AddScoped<backend.Endpoints.Index>()
    .AddScoped<backend.Endpoints.Create>()
    .AddScoped<backend.Endpoints.Get>()
    .AddScoped<backend.Endpoints.Update>()
    .AddScoped<backend.Endpoints.Delete>();

// Our app: Validators
builder.Services
    .AddScoped<IValidator<BoatData>, BoatDataValidator>()
    .AddScoped<IValidator<BoatCreateRequest>, BoatCreateValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGroup("/auth").WithTags("auth").MapIdentityApi<OwtBoatsUser>();

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
var boatsGroup = app.MapGroup("/boats").WithTags("boats").RequireAuthorization();

boatsGroup.MapGet("/", ([FromServices] backend.Endpoints.Index endpoint) => endpoint.ListBoats()).WithName("ListBoats");
boatsGroup.MapPost("/", ([FromBody] BoatCreateRequest boat, [FromServices] backend.Endpoints.Create endpoint) => endpoint.CreateBoat(boat)).WithName("CreateBoat");
boatsGroup.MapGet("/{id:Guid}", (Guid id, [FromServices] backend.Endpoints.Get endpoint) => endpoint.GetBoat(id)).WithName("GetBoat");
boatsGroup.MapPut("/{id:Guid}", (Guid id, [FromBody] BoatData boat, [FromServices] backend.Endpoints.Update endpoint) => endpoint.UpdateBoat(id, boat)).WithName("UpdateBoat");
boatsGroup.MapDelete("/{id:Guid}", (Guid id, [FromServices] backend.Endpoints.Delete endpoint) => endpoint.DeleteBoat(id)).WithName("DeleteBoat");

// Apply migrations automatically on startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<OwtBoatsDbContext>();
    await dbContext.Database.MigrateAsync();
}

await app.RunAsync();