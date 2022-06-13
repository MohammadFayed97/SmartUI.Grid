namespace SmartUI.Grid.Services
{
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceExtentions
    {
        public static void AddSmartGrid(this IServiceCollection services)
        {
            services.AddScoped(typeof(IHttpFeatureService<>), typeof(HttpFeatureService<>));
        }
    }
}
