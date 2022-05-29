namespace Homework.Services
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddConvertersServices(this IServiceCollection services)
        {
            services.AddTransient<IFileService, FileService>();
            services.AddSingleton<IDocumentService, DocumentService>();
            services.AddSingleton<IFormatService, FormatService>();

            return services;
        }
    }
}
