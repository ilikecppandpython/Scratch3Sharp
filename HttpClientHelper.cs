using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Tests")]

namespace Scratch3Sharp.Core;

internal static class HttpClientHelper
{
    internal static HttpClient Client = new HttpClient();
}