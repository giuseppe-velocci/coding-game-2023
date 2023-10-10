using Core;
using Order.Core.Dto;
using Order.Core.Interfaces;

namespace Order.Api
{
    public class OrderApiServiceAdapter
    {
        private readonly IOrderEndpointsService _service;

        public OrderApiServiceAdapter(IOrderEndpointsService service)
        {
            _service = service;
        }

        public async Task<IResult> GetOrderAsync(Key id)
        {
            var result = await _service.GetOrderAsync(id);
            return result.Success ? Results.Ok(result) : Results.BadRequest(result);
        }

        public async Task<IResult> GetDrinksAsync() => Results.Ok(await _service.GetDrinksAsync());

        public async Task<IResult> GetPaymentsAsync() => Results.Ok(await _service.GetPaymentsAsync());

        public async Task<IResult> CreateOrderAsync()
        {
            var result = await _service.CreateOrderAsync();
            return result.Success ? Results.Created($"order/{result.Value.Value}", result) : Results.Conflict(result);
        }

        public async Task<IResult> AddPaymentAsync(string id, PaymentRequest payment)
        {
            if (payment is null)
            {
                return Results.BadRequest(OperationResult<Key>.CreateFailure("Invalid body"));
            }

            var result = await _service.AddPaymentAsync(id, payment);
            return result.Success ? Results.Created($"order/{result.Value.Value}", result) : Results.BadRequest(result);
        }

        public async Task<IResult> AddToBasketAsync(string order, ProductRequest product)
        {
            if (product is null)
            {
                return Results.BadRequest(OperationResult<Key>.CreateFailure("Invalid body"));
            }

            var result = await _service.AddToBasketAsync(order, product);
            return result.Success ? Results.Created($"order/{result.Value.Value}", result) : Results.BadRequest(result);
        }
    }
}
