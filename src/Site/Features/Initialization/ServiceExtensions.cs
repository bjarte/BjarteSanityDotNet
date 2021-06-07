using Microsoft.Extensions.DependencyInjection;

namespace SanityDotNet.Site.Features.Initialization
{
    public static class ServiceExtensions
    {
        public static IMvcBuilder AddFeatureFolders(this IMvcBuilder builder)
        {
            builder
                .AddRazorOptions(options =>
                    {
                        options.ViewLocationFormats.Add("/Features/{1}/{0}.cshtml");
                        options.ViewLocationFormats.Add("/Features/{1}/Views/{0}.cshtml");
                        options.ViewLocationFormats.Add("/Features/Shared/{0}.cshtml");
                        options.ViewLocationFormats.Add("/Features/Shared/Views/{0}.cshtml");
                    }
                );

            return builder;
        }
    }
}