using CretaceousApi.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<CretaceousApiContext>(
    DbContextOptions => DbContextOptions.UseMySql(
        builder.Configuration["ConnectionStrings:DefaultConnection"],
        ServerVersion.AutoDetect(builder.Configuration["ConnectionStrings:DefaultConnection"]
        )
    )
);
builder.Services.AddEndpointsApiExplorer();
// enabling Swagger doc to proceed, exposing API's endpoint for docs 
// its tools will be sued in order for developers to control what endpoints to be visible to Swagger doc 
builder.Services.AddSwaggerGen();
// Swagger: a service that authomatically documents the available endpoints in application 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else 
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
