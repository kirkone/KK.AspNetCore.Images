namespace KK.AspNetCore.Images.Processing.Internal.Helpers
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Http;

    internal static class GenericHelpers
    {
        internal static bool IsGetOrHeadMethod(string method)
            => HttpMethods.IsGet(method) || HttpMethods.IsHead(method);

        internal static bool TryMatchPath(PathString path, PathString matchUrl)
        {
            if (path.StartsWithSegments(matchUrl))
            {
                return true;
            }
            return false;
        }

        internal static bool IsRequestFileTypeSupported(string extension, string[] supportetFileTypes)
            => supportetFileTypes.Contains(extension.Trim('.'));
    }
}
