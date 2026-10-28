namespace CleanMinimalApi.Presentation.OpenApi;

using System.Globalization;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi;

/// <summary>
/// Describes the API and its tags in the generated OpenAPI document.
/// </summary>
public sealed class DocumentInfoTransformer(IHostEnvironment environment) : IOpenApiDocumentTransformer
{
    private static readonly Uri RepositoryUrl = new("https://github.com/stphnwlsh/cleanminimalapi");

    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        var ti = CultureInfo.CurrentCulture.TextInfo;

        document.Info = new OpenApiInfo
        {
            Version = "v1",
            Title = $"CleanMinimalApi API - {ti.ToTitleCase(environment.EnvironmentName)}",
            Description = "An example to share an implementation of Minimal API in .NET 10, built with Clean Architecture, CQRS and Mediator.",
            Contact = new OpenApiContact
            {
                Name = "CleanMinimalApi API",
                Url = RepositoryUrl
            },
            License = new OpenApiLicense
            {
                Name = "CleanMinimalApi API - License - MIT",
                Url = new Uri("https://opensource.org/licenses/MIT")
            }
        };

        document.ExternalDocs = new OpenApiExternalDocs
        {
            Description = "Source code and documentation",
            Url = RepositoryUrl
        };

        document.Tags = new HashSet<OpenApiTag>
        {
            CreateTag("Authors", "Lookup all Authors, search them by name or find them by Id"),
            CreateTag("Movies", "Lookup all Movies, search them by title or find them by Id"),
            CreateTag("Reviews", "Create new Reviews, Lookup all Reviews, search them by movie or author or find them by Id, Update and Delete Reviews"),
            CreateTag("Version", "Lookup the application version details")
        };

        return Task.CompletedTask;
    }

    private static OpenApiTag CreateTag(string name, string description)
    {
        return new OpenApiTag
        {
            Name = name,
            Description = description,
            ExternalDocs = new OpenApiExternalDocs { Url = RepositoryUrl }
        };
    }
}
