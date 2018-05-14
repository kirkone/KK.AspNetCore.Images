namespace KK.AspNetCore.Images.Processing
{
    using System;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.FileProviders;

    internal static class Helpers
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
    }
}