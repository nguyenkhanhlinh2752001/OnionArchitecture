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
            service.AddScoped<IProductRepository, ProductRepository>();
            service.AddScoped<IOrderRespository, OrderRepository>();
            service.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
            service.AddScoped<ICartRepository, CartRepository>();
            service.AddScoped<ICartDetailRepository, CartDetailRepository>();
            service.AddScoped<IReviewRepository, ReviewRepository>();
            service.AddScoped<IProductDetailRepository, ProductDetailRepository>();
            service.AddScoped<IImageProductRepository, ImageProductRepository>();
            service.AddScoped<IImageReviewRepository, ImageReviewRepsitory>();

            service.AddAutoMapper(Assembly.GetExecutingAssembly());
            service.AddTransient(typeof(Interfaces.Repositories.RepositoryAsync<,>), typeof(Repositories.RepositoryAsync<,>));
            service.AddTransient(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
        }
    }
}