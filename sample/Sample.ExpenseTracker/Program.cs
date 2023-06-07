using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/", () => "Hello World");

app.MapGet("/{amount}", (int amount, [FromQuery] string name) => $"Amount {amount} {name}");

//// app.MapGet("/name", ([FromQuery] string name, [FromServices] IMyHandler handler) => handler.Handle(name));

app.Run();
