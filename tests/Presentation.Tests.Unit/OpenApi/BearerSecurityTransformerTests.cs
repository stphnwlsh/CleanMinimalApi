namespace CleanMinimalApi.Presentation.Tests.Unit.OpenApi;

using CleanMinimalApi.Presentation.OpenApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;
using NSubstitute;
using Shouldly;
using Xunit;

public class BearerSecurityTransformerTests
{
    private readonly BearerSecurityTransformer transformer = new();

    [Fact]
    public async Task TransformDocument_ShouldAdd_BearerScheme_WhenDocumentHasNoComponents()
    {
        // Arrange
        var document = new OpenApiDocument();

        // Act
        await this.transformer.TransformAsync(document, CreateDocumentContext(), CancellationToken.None);

        // Assert
        var scheme = document.Components.ShouldNotBeNull().SecuritySchemes.ShouldNotBeNull()[BearerSecurityTransformer.SchemeName];
        scheme.Type.ShouldBe(SecuritySchemeType.Http);
        scheme.Scheme.ShouldBe("bearer");
        scheme.BearerFormat.ShouldBe("JWT");
    }

    [Fact]
    public async Task TransformDocument_ShouldKeep_ExistingSchemes()
    {
        // Arrange
        var document = new OpenApiDocument
        {
            Components = new OpenApiComponents
            {
                SecuritySchemes = new Dictionary<string, IOpenApiSecurityScheme> { ["ApiKey"] = new OpenApiSecurityScheme() }
            }
        };

        // Act
        await this.transformer.TransformAsync(document, CreateDocumentContext(), CancellationToken.None);

        // Assert
        document.Components.ShouldNotBeNull().SecuritySchemes.ShouldNotBeNull().Keys.ShouldBe(["ApiKey", BearerSecurityTransformer.SchemeName], ignoreOrder: true);
    }

    [Fact]
    public async Task TransformOperation_ShouldAdd_SecurityAndUnauthorized_WhenEndpointRequiresAuthorization()
    {
        // Arrange
        var operation = new OpenApiOperation();

        // Act
        await this.transformer.TransformAsync(operation, CreateOperationContext(new AuthorizeAttribute()), CancellationToken.None);

        // Assert
        operation.Security.ShouldNotBeNull().Count.ShouldBe(1);
        operation.Security[0].Keys.Single().Reference.Id.ShouldBe(BearerSecurityTransformer.SchemeName);
        operation.Responses.ShouldNotBeNull()["401"].Description.ShouldNotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task TransformOperation_ShouldKeep_ExistingSecurityAndResponses()
    {
        // Arrange
        var operation = new OpenApiOperation
        {
            Security = [new OpenApiSecurityRequirement()],
            Responses = new OpenApiResponses { ["401"] = new OpenApiResponse { Description = "Existing." } }
        };

        // Act
        await this.transformer.TransformAsync(operation, CreateOperationContext(new AuthorizeAttribute()), CancellationToken.None);

        // Assert
        operation.Security.Count.ShouldBe(2);
        operation.Responses["401"].Description.ShouldBe("Existing.");
    }

    [Fact]
    public async Task TransformOperation_ShouldSkip_AnonymousEndpoints()
    {
        // Arrange
        var operation = new OpenApiOperation();

        // Act
        await this.transformer.TransformAsync(operation, CreateOperationContext(new AuthorizeAttribute(), new AllowAnonymousAttribute()), CancellationToken.None);

        // Assert
        operation.Security.ShouldBeNull();
        operation.Responses.ShouldBeEmpty();
    }

    [Fact]
    public async Task TransformOperation_ShouldSkip_EndpointsWithoutAuthorization()
    {
        // Arrange
        var operation = new OpenApiOperation();

        // Act
        await this.transformer.TransformAsync(operation, CreateOperationContext(), CancellationToken.None);

        // Assert
        operation.Security.ShouldBeNull();
        operation.Responses.ShouldBeEmpty();
    }

    private static OpenApiDocumentTransformerContext CreateDocumentContext()
    {
        return new OpenApiDocumentTransformerContext
        {
            DocumentName = "v1",
            DescriptionGroups = [],
            ApplicationServices = Substitute.For<IServiceProvider>()
        };
    }

    private static OpenApiOperationTransformerContext CreateOperationContext(params object[] metadata)
    {
        return new OpenApiOperationTransformerContext
        {
            DocumentName = "v1",
            Description = new ApiDescription { ActionDescriptor = new ActionDescriptor { EndpointMetadata = metadata } },
            ApplicationServices = Substitute.For<IServiceProvider>(),
            Document = new OpenApiDocument()
        };
    }
}
