using Core;
using Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Order.Core.Interfaces;
using Order.Service.Aggregates;

namespace Order.Service.DependencyInjection
{
    public static class HostBuilderExtension
    {
        public static IServiceCollection AddOrderService(this IServiceCollection services)
        {
            services
                .AddTransient<IOrderAggregate, OrderAggregate>()
                .AddSingleton<IEventStore<IOrder>, InMemoryEventStore<IOrder>>()
                .AddTransient<ICommandHandler<IOrder>, OrderCommandHandler>();
            return services;
        }
    }
}
