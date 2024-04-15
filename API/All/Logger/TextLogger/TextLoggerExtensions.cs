using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging.Configuration;

namespace API.All.Logger.TextLogger
{
    public static class TextLoggerExtensions
    {
        public static ILoggingBuilder AddTextLogger(this ILoggingBuilder builder, Action<TextLoggerOptions> configure)
        {
            builder.AddConfiguration();

            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, TextLoggerProvider>());
            builder.Services.Configure(configure);

            LoggerProviderOptions.RegisterProviderOptions<TextLoggerConfiguration, TextLoggerProvider>(builder.Services);

            return builder;
        }

    }
}
