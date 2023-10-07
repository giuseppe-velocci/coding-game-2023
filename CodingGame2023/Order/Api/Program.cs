using Core;
using Order.Api;
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

app.MapGet("/order/$id", (string id) => 
    new Key[] { app.Services.GetRequiredService<OrderEndpoints>().GetOrder(new Key(id)) }
);

app.MapPost("/order", () => 
    new OperationResult<Key>[] { app.Services.GetRequiredService<OrderEndpoints>().CreateOrder() }
);

app.Run();
