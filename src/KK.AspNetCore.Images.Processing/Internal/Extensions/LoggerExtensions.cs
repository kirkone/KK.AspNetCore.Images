namespace KK.AspNetCore.Images.Processing.Internal.Extensions
{
    using System;
    using Microsoft.Extensions.Logging;

    internal static class LoggerExtensions
    {
        private static readonly Action<ILogger, string, Exception> LogMethodNotSupportedAction;
        private static readonly Action<ILogger, string, Exception> LogPathMismatchAction;
        private static readonly Action<ILogger, string, Exception> LogProcessingImageAction;
        private static readonly Action<ILogger, string, Exception> LogSizeNotSupportedAction;
        private static readonly Action<ILogger, string, Exception> LogSourceImageNotFoundAction;
        private static readonly Action<ILogger, string, Exception> LogRequestFileTypeNotSupportedAction;
        private static readonly Action<ILogger, string, Exception> LogSourceFileTypeNotSupportedAction;

        static LoggerExtensions()
        {
            LogMethodNotSupportedAction = LoggerMessage.Define<string>(
                logLevel: LogLevel.Debug,
                eventId: 1,
                formatString: "{Method} requests are not supported");
            LogPathMismatchAction = LoggerMessage.Define<string>(
                logLevel: LogLevel.Debug,
                eventId: 2,
                formatString: "The request path {Path} does not match the path filter");
            LogProcessingImageAction = LoggerMessage.Define<string>(
               logLevel: LogLevel.Information,
               eventId: 3,
               formatString: "Processing Image. Image path: '{Path}'.");
            LogSizeNotSupportedAction = LoggerMessage.Define<string>(
               logLevel: LogLevel.Information,
               eventId: 4,
               formatString: "Size not supported: '{Size}'.");
            LogSourceImageNotFoundAction = LoggerMessage.Define<string>(
               logLevel: LogLevel.Information,
               eventId: 5,
               formatString: "Source image not found: '{Path}'.");
            LogRequestFileTypeNotSupportedAction = LoggerMessage.Define<string>(
               logLevel: LogLevel.Information,
               eventId: 6,
               formatString: "Requested file type not supported: '{Extension}'.");
            LogSourceFileTypeNotSupportedAction = LoggerMessage.Define<string>(
               logLevel: LogLevel.Information,
               eventId: 6,
               formatString: "Source file type not supported: '{Extension}'.");
        }

        public static void LogRequestMethodNotSupported(this ILogger logger, string method)
            => LogMethodNotSupportedAction(logger, method, null);

        public static void LogPathMismatch(this ILogger logger, string path)
            => LogPathMismatchAction(logger, path, null);

        public static void LogProcessingImage(this ILogger logger, string path)
            => LogProcessingImageAction(logger, path, null);

        public static void LogSizeNotSupported(this ILogger logger, string size)
            => LogSizeNotSupportedAction(logger, size, null);
        public static void LogSourceImageNotFound(this ILogger logger, string path)
            => LogSourceImageNotFoundAction(logger, path, null);
        public static void LogRequestFileTypeNotSupported(this ILogger logger, string extension)
            => LogRequestFileTypeNotSupportedAction(logger, extension, null);
        public static void LogSourceFileTypeNotSupported(this ILogger logger, string extension)
            => LogSourceFileTypeNotSupportedAction(logger, extension, null);

    }
}
