using Core;
using Order.Core.Dto;
using Order.Core.Interfaces;

namespace Order.Api
{
    public static class EndpointsBinder
    {
        public static void BindOrderEndpoints(WebApplication app, Func<IOrderEndpointsService> getService)
        {
            //GET
            app.MapGet("/order/{id}", async (string id) =>
            {
                var result = await getService().GetOrderAsync(new Key(id));
                return result.Success ? Results.Ok(result) : Results.BadRequest(result);
            });

            app.MapGet("/drinks", async () => Results.Ok(await getService().GetDrinksAsync()));

            app.MapGet("/payments", async () => Results.Ok(await getService().GetPaymentsAsync()));

            //POST
            app.MapPost("/order", async () =>
            {
                var result = await getService().CreateOrderAsync();
                return result.Success ? Results.Created($"order/{result.Value.Value}", result) : Results.BadRequest(result);
            });

            app.MapPost("/order/add-payment/{id}", async (string id, PaymentRequest payment) =>
            {
                var result = await getService().AddPaymentAsync(id, payment);
                return result.Success ? Results.Created($"order/{result.Value.Value}", result) : Results.BadRequest(result);
            });

            app.MapPost("/order/add-product/{order}", async (string order, ProductRequest product) =>
            {
                if (product is null)
                {
                    return Results.BadRequest("Invalid body");
                }

                var result = await getService().AddToBasketAsync(order, product);
                return result.Success ? Results.Created($"order/{result.Value.Value}", result) : Results.BadRequest(result);
            });
        }
    }
}
