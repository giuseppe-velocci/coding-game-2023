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
    .AddTransient<OrderEndpoints>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//GET
app.MapGet("/order/{id}", (string id) =>
{
    var result = app.Services.GetRequiredService<OrderEndpoints>().GetOrder(new Key(id));
    return result.Success ? Results.Ok(result) : Results.BadRequest(result);
});

app.MapGet("/drinks", () => Results.Ok(app.Services.GetRequiredService<OrderEndpoints>().GetDrinks()));

app.MapGet("/payments", () => Results.Ok(app.Services.GetRequiredService<OrderEndpoints>().GetPayments()));

//POST
app.MapPost("/order", () =>
{
    var result = app.Services.GetRequiredService<OrderEndpoints>().CreateOrder();
    return result.Success ? Results.Created($"order/{result.Value.Value}", result) : Results.BadRequest(result);
});

app.MapPost("/order/add-payment/{id}", (string id, PaymentRequest payment) =>
{
    var result = app.Services.GetRequiredService<OrderEndpoints>().AddPayment(id, payment);
    return result.Success ? Results.Created($"order/{result.Value.Value}", result) : Results.BadRequest(result);
});

app.MapPost("/order/add-product/{order}", (string order, ProductRequest product) =>
{
    if (product is null)
    {
        return Results.BadRequest("Invalid body");
    }

    var result = app.Services.GetRequiredService<OrderEndpoints>().AddToBasket(order, product);
    return result.Success ? Results.Created($"order/{result.Value.Value}", result) : Results.BadRequest(result);
});

app.Run();
