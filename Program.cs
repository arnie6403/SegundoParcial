using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using SegundoParcial.Data;

var builder = WebApplication.CreateBuilder(args);

// DB
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("MySqlConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MySqlConnection"))
    )
);

builder.Services.AddControllers();

// 👇 NECESARIO para Scalar (OpenAPI)
builder.Services.AddOpenApi();

var app = builder.Build();

// 👇 MAPEAR DOCUMENTO OPENAPI
app.MapOpenApi();

// 👇 SCALAR UI
app.MapScalarApiReference();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();