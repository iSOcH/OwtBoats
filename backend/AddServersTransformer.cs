using Microsoft.AspNetCore.OpenApi;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace backend;

// "Borrowed" from https://github.com/martincostello/openapi-extensions/blob/6b5b0a4b85cc5135fcc00377ce4d4d2f0f55470c/src/OpenApi.Extensions/Transformers/AddServersTransformer.cs
// The default mechanism only adds localhost urls to the swagger spec (at least in the development environment) which does not work when working in Github Codespaces
internal sealed class AddServersTransformer(
    IHttpContextAccessor? accessor,
    IOptions<ForwardedHeadersOptions>? forwardedHeadersOptions) : IOpenApiDocumentTransformer
{
    /// <inheritdoc/>
    public Task TransformAsync(
        OpenApiDocument document,
        OpenApiDocumentTransformerContext context,
        CancellationToken cancellationToken)
    {
        if (GetServerUrl() is { } url)
        {
            document.Servers = [new() { Url = url }];
        }

        return Task.CompletedTask;
    }

    private string? GetServerUrl()
    {
        if (accessor?.HttpContext?.Request is not { } request)
            throw new InvalidOperationException("Cannot transform non-http request");

        if (forwardedHeadersOptions?.Value is not { } options)
            return null;

        var scheme = TryGetFirstHeader(options.ForwardedProtoHeaderName) ?? request.Scheme;
        var host = TryGetFirstHeader(options.ForwardedHostHeaderName) ?? request.Host.ToString();

        return new Uri($"{scheme}://{host}").ToString().TrimEnd('/');

        string? TryGetFirstHeader(string name)
            => request.Headers.TryGetValue(name, out var values) ? values.FirstOrDefault() : null;
    }
}