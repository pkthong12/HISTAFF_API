using Microsoft.Extensions.Options;
using System.Collections.Concurrent;

namespace API.All.Logger.TextLogger
{
    [ProviderAlias("Text")]
    public sealed class TextLoggerProvider : ILoggerProvider
    {

        public readonly TextLoggerOptions Options;

        private readonly IDisposable? _onChangeToken;
        private TextLoggerConfiguration _currentConfig;
        private readonly ConcurrentDictionary<string, TextLogger> _loggers =
            new(StringComparer.OrdinalIgnoreCase);

        private readonly IWebHostEnvironment _env;
        private readonly IOptions<AppSettings> _appSettingsOptions;

        public TextLoggerProvider(
            IWebHostEnvironment env, IOptions<TextLoggerOptions> textLoggerOptions, IOptions<AppSettings> appSettingsOptions,
            IOptionsMonitor<TextLoggerConfiguration> config)
        {
            _currentConfig = config.CurrentValue;
            _onChangeToken = config.OnChange(updatedConfig => _currentConfig = updatedConfig);
            Options = textLoggerOptions.Value;

            _env = env;
            _appSettingsOptions = appSettingsOptions;
        }

        public ILogger CreateLogger(string categoryName) =>
            _loggers.GetOrAdd(categoryName, name => new TextLogger(_env, _appSettingsOptions, this, name, GetCurrentConfig));

        private TextLoggerConfiguration GetCurrentConfig() => _currentConfig;

        public void Dispose()
        {
            _loggers.Clear();
            _onChangeToken?.Dispose();
        }
    }
}

