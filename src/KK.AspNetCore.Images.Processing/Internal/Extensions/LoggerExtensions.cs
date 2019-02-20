namespace KK.AspNetCore.Images.Processing.Internal.Extensions
{
    using System;
    using Microsoft.Extensions.Logging;

    internal static class LoggerExtensions
    {
        private static Action<ILogger, string, Exception> logMethodNotSupported;
        private static Action<ILogger, string, Exception> logPathMismatch;
        private static Action<ILogger, string, Exception> logProcessingImage;
        private static Action<ILogger, string, Exception> logSizeNotSupported;
        private static Action<ILogger, string, Exception> logSourceImageNotFound;
        private static Action<ILogger, string, Exception> logRequestFileTypeNotSupported;
        private static Action<ILogger, string, Exception> logSourceFileTypeNotSupported;

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
            logSizeNotSupported = LoggerMessage.Define<string>(
               logLevel: LogLevel.Information,
               eventId: 4,
               formatString: "Size not supported: '{Size}'.");
            logSourceImageNotFound = LoggerMessage.Define<string>(
               logLevel: LogLevel.Information,
               eventId: 5,
               formatString: "Source image not found: '{Path}'.");
            logRequestFileTypeNotSupported = LoggerMessage.Define<string>(
               logLevel: LogLevel.Information,
               eventId: 6,
               formatString: "Requested file type not supported: '{Extension}'.");
            logSourceFileTypeNotSupported = LoggerMessage.Define<string>(
               logLevel: LogLevel.Information,
               eventId: 6,
               formatString: "Source file type not supported: '{Extension}'.");
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

        public static void LogSizeNotSupported(this ILogger logger, string size)
        {
            logSizeNotSupported(logger, size, null);
        }
        public static void LogSourceImageNotFound(this ILogger logger, string path)
        {
            logSourceImageNotFound(logger, path, null);
        }
        public static void LogRequestFileTypeNotSupported(this ILogger logger, string extension)
        {
            logRequestFileTypeNotSupported(logger, extension, null);
        }
        public static void LogSourceFileTypeNotSupported(this ILogger logger, string extension)
        {
            logSourceFileTypeNotSupported(logger, extension, null);
        }

    }
}