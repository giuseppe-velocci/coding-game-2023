using Core;
using Order.Core.Dto;

namespace Order.Api
{
    public static class EndpointsBinder
    {
        public static void BindOrderEndpoints(WebApplication app, Func<OrderApiServiceAdapter> getService)
        {
            //GET
            app.MapGet("/order/{id}", async (string id) => await getService().GetOrderAsync(new Key(id)));

            app.MapGet("/drinks", async () => Results.Ok(await getService().GetDrinksAsync()));

            app.MapGet("/payments", async () => Results.Ok(await getService().GetPaymentsAsync()));

            //POST
            app.MapPost("/order", async () => await getService().CreateOrderAsync());

            app.MapPost("/order/add-payment/{id}", async (string id, PaymentRequest payment) =>
                await getService().AddPaymentAsync(id, payment));

            app.MapPost("/order/add-product/{order}", async (string order, ProductRequest product) =>
                await getService().AddToBasketAsync(order, product));
        }
    }
}
