using Application.Interfaces.Repositories;
using Application.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCoreApplication(this IServiceCollection service)
        {
            service.AddScoped<ICategoryRepository, CategoryRepository>();
            service.AddScoped<IProductRepsitory, ProductRepository>();
            service.AddScoped<IOrderRespository, OrderRepository>();
            service.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
            service.AddScoped<ICartRepository, CartRepository>();
            service.AddScoped<ICartDetailRepository, CartDetailRepository>();
            service.AddScoped<IReviewRepository, ReviewRepository>();

            service.AddAutoMapper(Assembly.GetExecutingAssembly());
            service.AddTransient(typeof(IRepositoryAsync<,>), typeof(RepositoryAsync<,>));
            service.AddTransient(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
        }
    }
}