namespace KK.AspNetCore.Images.Processing
{
    using System;
    using Microsoft.Extensions.Logging;

    internal static class LoggerExtensions
    {
        private static Action<ILogger, string, Exception> logMethodNotSupported;
        private static Action<ILogger, string, Exception> logPathMismatch;
        private static Action<ILogger, string, Exception> logProcessingImage;

        static LoggerExtensions()
        {
            logMethodNotSupported = LoggerMessage.Define<string>(
                logLevel: LogLevel.Debug,
                eventId: 1,
                formatString: "{Method} requests are not supported");
            logPathMismatch = LoggerMessage.Define<string>(
                logLevel: LogLevel.Debug,
                eventId: 2,
                formatString: "The request path {Path} does not match the path filter");
            logProcessingImage = LoggerMessage.Define<string>(
               logLevel: LogLevel.Information,
               eventId: 3,
               formatString: "Processing Image. Image path: '{Path}'.");
        }

        public static void LogRequestMethodNotSupported(this ILogger logger, string method)
        {
            logMethodNotSupported(logger, method, null);
        }

        public static void LogPathMismatch(this ILogger logger, string path)
        {
            logPathMismatch(logger, path, null);
        }

        public static void LogProcessingImage(this ILogger logger, string path)
        {
            logProcessingImage(logger, path, null);
        }
    }
}