namespace CleanMinimalApi.Presentation.Tests.Integration.Authentication;

using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Shouldly;
using Xunit;

public class AuthenticationTests : IDisposable
{
    private const string Authority = "https://login.example.com";
    private const string Audience = "https://api.example.com";

    private static readonly SymmetricSecurityKey SigningKey = new(Encoding.UTF8.GetBytes("a-test-signing-key-that-is-long-enough-for-hs256"));

    private readonly MinimalApiApplication application = new(
        settings: new Dictionary<string, string>
        {
            ["Authentication:Authority"] = Authority,
            ["Authentication:Audience"] = Audience
        },
        configureServices: services => services.PostConfigure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
        {
            options.Configuration = new OpenIdConnectConfiguration { Issuer = Authority };
            options.TokenValidationParameters.IssuerSigningKey = SigningKey;
            options.TokenValidationParameters.ValidIssuer = Authority;
        }));

    [Theory]
    [InlineData("/api/movie")]
    [InlineData("/api/author")]
    [InlineData("/api/review")]
    public async Task Get_WithoutToken_ShouldReturn_Unauthorized(string path)
    {
        // Arrange
        using var client = this.application.CreateClient();

        // Act
        using var response = await client.GetAsync(path);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        response.Headers.WwwAuthenticate.ToString().ShouldStartWith("Bearer");
    }

    [Fact]
    public async Task Get_WithWrongAudience_ShouldReturn_Unauthorized()
    {
        // Arrange
        using var client = this.application.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CreateToken("https://someone-else.example.com"));

        // Act
        using var response = await client.GetAsync("/api/movie");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Theory]
    [InlineData("/api/movie")]
    [InlineData("/api/author")]
    [InlineData("/api/review")]
    public async Task Get_WithToken_ShouldReturn_Ok(string path)
    {
        // Arrange
        using var client = this.application.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CreateToken(Audience));

        // Act
        using var response = await client.GetAsync(path);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetVersion_WithoutToken_ShouldReturn_Ok()
    {
        // Arrange
        using var client = this.application.CreateClient();

        // Act
        using var response = await client.GetAsync("/api/version");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetOpenApiDocument_ShouldDescribe_BearerScheme()
    {
        // Arrange
        using var client = this.application.CreateClient();

        // Act
        using var document = JsonDocument.Parse(await client.GetStringAsync("/openapi/v1.json"));

        // Assert
        var scheme = document.RootElement.GetProperty("components").GetProperty("securitySchemes").GetProperty("Bearer");
        scheme.GetProperty("type").GetString().ShouldBe("http");
        scheme.GetProperty("scheme").GetString().ShouldBe("bearer");

        var getMovies = document.RootElement.GetProperty("paths").GetProperty("/api/movie").GetProperty("get");
        getMovies.GetProperty("security")[0].TryGetProperty("Bearer", out _).ShouldBeTrue();
        getMovies.GetProperty("responses").TryGetProperty("401", out _).ShouldBeTrue();

        var getVersion = document.RootElement.GetProperty("paths").GetProperty("/api/version").GetProperty("get");
        getVersion.TryGetProperty("security", out _).ShouldBeFalse();
        getVersion.GetProperty("responses").TryGetProperty("401", out _).ShouldBeFalse();
    }

    [Fact]
    public async Task GetOpenApiDocument_WithoutAuthentication_ShouldNotDescribe_Security()
    {
        // Arrange
        using var application = new MinimalApiApplication();
        using var client = application.CreateClient();

        // Act
        using var document = JsonDocument.Parse(await client.GetStringAsync("/openapi/v1.json"));

        // Assert
        document.RootElement.GetProperty("components").TryGetProperty("securitySchemes", out _).ShouldBeFalse();
        document.RootElement.GetProperty("paths").GetProperty("/api/movie").GetProperty("get").TryGetProperty("security", out _).ShouldBeFalse();
    }

    public void Dispose()
    {
        this.application.Dispose();
        GC.SuppressFinalize(this);
    }

    private static string CreateToken(string audience)
    {
        return new JsonWebTokenHandler().CreateToken(new SecurityTokenDescriptor
        {
            Issuer = Authority,
            Audience = audience,
            Subject = new ClaimsIdentity([new Claim("sub", "test-user")]),
            Expires = DateTime.UtcNow.AddMinutes(5),
            SigningCredentials = new SigningCredentials(SigningKey, SecurityAlgorithms.HmacSha256)
        });
    }
}
