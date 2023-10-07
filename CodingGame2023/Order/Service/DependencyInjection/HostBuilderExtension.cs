using Core;
using Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Order.Core;
using Order.Service.Aggregate;

namespace Order.Service.DependencyInjection
{
    public static class HostBuilderExtension
    {
        public static IServiceCollection AddOrderService(this IServiceCollection services)
        {
            services
                .AddTransient<IOrderAggregate, OrderAggregate>()
                .AddTransient<IEventStore<IOrder>, InMemoryEventStore<IOrder>>()
                .AddTransient<IService<IOrder>, OrderCommandHandler>();
            return services;
        }
    }
}
