using Persistence.Interfaces;

namespace WebApi.Extensions
{
    public static class AppExtention
    {
        public static void UseSwaggerExtension(this IApplicationBuilder app)
        {
        }

        public static IApplicationBuilder InitializeDb(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();

            var initializers = serviceScope.ServiceProvider.GetServices<IDatabaseSeeder>();

            foreach (var initializer in initializers)
            {
                initializer.Initialize();
            }

            return app;
        }
    }
}