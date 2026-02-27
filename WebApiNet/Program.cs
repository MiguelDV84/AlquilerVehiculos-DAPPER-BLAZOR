using Dapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MySqlConnector;
using System.Data;
using System.Text;
using WebApiNet.Data;
using WebApiNet.Mappers;
using WebApiNet.Models;
using WebApiNet.Repositories;
using WebApiNet.Servicios;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//Inicio de migracion a DAPPER
builder.Services.AddScoped<IDbConnection>(sp => new MySqlConnection(connectionString));
builder.Services.AddScoped<VehiculoRepository>();
builder.Services.AddScoped<IVehiculoService, VehiculoService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAlquilerService, AlquilerService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MappingProfile>();
});
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});
builder.Services.AddOpenApi();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
      ));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazor", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();


//Routing con dapper
app.MapGet("/api/v2/vehiculos", async (VehiculoRepository repo) =>
{
    var vehiculos = await repo.GetAllAsync();

    return Results.Ok(vehiculos);
});

app.MapGet("/api/v2/vehiculos/{matricula}", async (string matricula, VehiculoRepository repo) =>
{
    var vehiculo = await repo.GetByIdAsync(matricula);
    if (vehiculo == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(vehiculo);
});

app.MapPost("/api/v2/vehiculos", async (Vehiculos vehiculo, VehiculoRepository repo) =>
{
    var nuevoVehiculo = await repo.AddAsync(vehiculo);
    return Results.Created($"/api/v2/vehiculos/{nuevoVehiculo.Matricula}", nuevoVehiculo);
});

app.MapDelete("/api/v2/vehiculos/{matricula}", async (string matricula, VehiculoRepository repo) =>
{
    var success = await repo.DeleteAsync(matricula);
    if (!success)
    {
        return Results.NotFound();
    }
    return Results.NoContent();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("AllowBlazor");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();