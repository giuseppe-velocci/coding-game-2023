using Core;
using Order.Api;
using Order.Api.Models;
using Order.Service.DependencyInjection;

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
{
    var result = app.Services.GetRequiredService<OrderEndpoints>().CreateOrder();
    return result.Success ? Results.Ok(result) : Results.BadRequest(result);
}
);

app.MapPost("/add-product-to-basket/{order}", (string order, ProductRequest product) =>
{
    if (product is null)
    {
        return Results.BadRequest("Invalid body");
    }

    var result = app.Services.GetRequiredService<OrderEndpoints>().AddToBasket(order, product);
    return result.Success? Results.Ok(result) : Results.BadRequest(result);
});

app.Run();
