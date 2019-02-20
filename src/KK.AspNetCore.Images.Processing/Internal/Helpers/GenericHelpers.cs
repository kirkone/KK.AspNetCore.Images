namespace KK.AspNetCore.Images.Processing.Internal.Helpers
{
    using System;
    using System.IO;
    using System.Linq;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.FileProviders;

    internal static class GenericHelpers
    {
        internal static bool IsGetOrHeadMethod(string method)
        {
            return HttpMethods.IsGet(method) || HttpMethods.IsHead(method);
        }

        internal static bool TryMatchPath(PathString path, PathString matchUrl)
        {
            if (path.StartsWithSegments(matchUrl))
            {
                return true;
            }
            return false;
        }

        internal static bool IsRequestFileTypeSupported(string extension, string[] supportetFileTypes)
        {
            return supportetFileTypes.Contains(extension.Trim('.'));
        }
    }
}