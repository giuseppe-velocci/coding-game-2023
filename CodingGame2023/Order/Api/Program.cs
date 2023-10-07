using Core;
using Order.Api;
using Order.Api.Models;
using Order.Service.DependencyInjection;
using System;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddOrderService()
    .AddSingleton<OrderEndpoints>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/order/{id}", (string id) =>
    new Key[] { app.Services.GetRequiredService<OrderEndpoints>().GetOrder(new Key(id)) }
);

app.MapGet("/drinks", () =>
    app.Services.GetRequiredService<OrderEndpoints>().GetDrinks()
);

app.MapPost("/order", () =>
    new OperationResult<Key>[] { app.Services.GetRequiredService<OrderEndpoints>().CreateOrder() }
);

app.MapPost("/add-product-to-basket/{order}", async (string order, HttpRequest request, Stream body) =>
{
    var readSize = (int?)request.ContentLength ?? (1024);
    var buffer = new byte[readSize];
    var read = await body.ReadAsync(buffer, CancellationToken.None);

    ProductRequest? product = JsonSerializer.Deserialize<ProductRequest>(read);
    if (product == null)
    {
        return Results.BadRequest("Invalid body");
    }
    return Results.Ok(new OperationResult<Key>[] { app.Services.GetRequiredService<OrderEndpoints>().AddToBasket(order, product) });
});

app.Run();
