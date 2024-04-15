using Microsoft.Extensions.Options;

namespace API.All.Logger.TextLogger
{
    public sealed class TextLogger : ILogger
    {
        private readonly IWebHostEnvironment _env;
        private readonly AppSettings _appSettings;
        readonly TextLoggerProvider _textLoggerProvider;

        private readonly string name;
        private readonly Func<TextLoggerConfiguration> getCurrentConfig;

        public TextLogger(IWebHostEnvironment env, IOptions<AppSettings> options, TextLoggerProvider textLoggerProvider, string _name, Func<TextLoggerConfiguration> _getCurrentConfig) {
            _env = env;
            _appSettings = options.Value;
            _textLoggerProvider = textLoggerProvider;

            name = _name;
            getCurrentConfig = _getCurrentConfig;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => default!;

        public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {

            if (!IsEnabled(logLevel))
            {
                return;
            }

            var fullFilePath = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Logs, _textLoggerProvider.Options.FileName.Replace("{date}", DateTimeOffset.UtcNow.ToString("yyyyMMdd")));
            var logRecord = string.Format("{0} [{1}] {2} {3}", "[" + DateTimeOffset.UtcNow.ToString("yyyy-MM-dd HH:mm:ss+00:00") + "]", logLevel.ToString(), formatter(state, exception), exception != null ? exception.StackTrace : "");

            try
            {
                using var streamWriter = new StreamWriter(fullFilePath, true);
                streamWriter.WriteLine(logRecord);
            }
            catch (Exception)
            {
            }


        }
    }
}
