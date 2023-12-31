using Order.Api;
using Order.Service.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Add business services
builder.Services
    .AddOrderService()
    .AddTransient<OrderApiServiceAdapter>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

EndpointsBinder.BindOrderEndpoints(app, () => app.Services.GetRequiredService<OrderApiServiceAdapter>());

app.Run();
