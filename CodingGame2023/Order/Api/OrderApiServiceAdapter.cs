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
            try
            {
                var result = await _service.GetOrderAsync(id);
                return result.Success ? Results.Ok(result) : Results.BadRequest(result);
            }
            catch (Exception _)
            {
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<IResult> GetDrinksAsync()
        {
            try
            {
                return Results.Ok(await _service.GetDrinksAsync());
            }
            catch (Exception _)
            {
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<IResult> GetPaymentsAsync()
        {
            try
            {
                return Results.Ok(await _service.GetPaymentsAsync());
            }
            catch (Exception _)
            {
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<IResult> CreateOrderAsync()
        {
            try
            {
                var result = await _service.CreateOrderAsync();
                return result.Success ? Results.Created($"order/{result.Value.Value}", result) : Results.Conflict(result);
            }
            catch (Exception _)
            {
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<IResult> AddPaymentAsync(string id, PaymentRequest payment)
        {
            try
            {
                if (payment is null)
                {
                    return Results.BadRequest(OperationResult<Key>.CreateFailure("Invalid body"));
                }

                var result = await _service.AddPaymentAsync(id, payment);
                return result.Success ? Results.Created($"order/{result.Value.Value}", result) : Results.BadRequest(result);
            }
            catch (Exception _)
            {
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<IResult> AddToBasketAsync(string id, ProductRequest product)
        {
            try
            {
                if (product is null)
                {
                    return Results.BadRequest(OperationResult<Key>.CreateFailure("Invalid body"));
                }

                var result = await _service.AddToBasketAsync(id, product);
                return result.Success ? Results.Created($"order/{result.Value.Value}", result) : Results.BadRequest(result);
            }
            catch (Exception _)
            {
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
