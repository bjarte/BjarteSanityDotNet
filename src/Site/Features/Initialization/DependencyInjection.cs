using Microsoft.Extensions.DependencyInjection;
using SanityDotNet.Site.Features.BlogPost;

namespace SanityDotNet.Site.Features.Initialization
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {
            // Services
            services.AddTransient<BlogPostService, BlogPostService>();

            // Repositories
            services.AddTransient<BlogPostRepository, BlogPostRepository>();

            return services;
        }
    }
}