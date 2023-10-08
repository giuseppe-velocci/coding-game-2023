using Core;
using Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Order.Core.Interfaces;
using Order.Service.Aggregates;
using Order.Service.CommandHandlers;
using Order.Service.Stores;

namespace Order.Service.DependencyInjection
{
    public static class HostBuilderExtension
    {
        public static IServiceCollection AddOrderService(this IServiceCollection services)
        {
            return services
                .AddSingleton<IEventStore<IOrder>, InMemoryEventStore<IOrder>>()
                .AddSingleton<IProductStore, ProductStore>()
                .AddSingleton<IPaymentStore, PaymentStore>()

                .AddTransient<ICommandHandler<IOrder>, OrderCommandHandler>()
                .AddTransient<IAggregate<IOrder>, OrderAggregate>()
                .AddTransient<IOrderStore, OrderStore>()
                ;
        }
    }
}
