namespace CleanMinimalApi.Presentation.Extensions;

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Application;
using Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Serilog;

[ExcludeFromCodeCoverage]
public static class ProgramExtensions
{
    public static WebApplicationBuilder ConfigureBuilder(this WebApplicationBuilder builder)
    {
        #region Logging

        _ = builder.Host.UseSerilog((hostContext, loggerConfiguration) =>
        {
            var assembly = Assembly.GetEntryAssembly();

            _ = loggerConfiguration.ReadFrom.Configuration(hostContext.Configuration)
                    .Enrich.WithProperty("Assembly Version", assembly?.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version)
                    .Enrich.WithProperty("Assembly Informational Version", assembly?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion);
        });

        #endregion Logging

        #region Serialisation

        _ = builder.Services.Configure<JsonOptions>(opt =>
        {
            opt.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            opt.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            opt.SerializerOptions.PropertyNameCaseInsensitive = true;
            opt.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            opt.SerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
        });

        #endregion Serialisation

        #region Swagger

        var ti = CultureInfo.CurrentCulture.TextInfo;

        _ = builder.Services.AddEndpointsApiExplorer();
        _ = builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1",
                new OpenApiInfo
                {
                    Version = "v1",
                    Title = $"CleanMinimalApi API - {ti.ToTitleCase(builder.Environment.EnvironmentName)} ",
                    Description = "An example to share an implementation of Minimal API in .NET 6.",
                    Contact = new OpenApiContact
                    {
                        Name = "CleanMinimalApi API",
                        Email = "cleanminimalapi@stphnwlsh.dev",
                        Url = new Uri("https://github.com/stphnwlsh/cleanminimalapi")
                    },
                    License = new OpenApiLicense()
                    {
                        Name = "CleanMinimalApi API - License - MIT",
                        Url = new Uri("https://opensource.org/licenses/MIT")
                    },
                    TermsOfService = new Uri("https://github.com/stphnwlsh/cleanminimalapi")
                });

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

            options.EnableAnnotations();
            options.DocInclusionPredicate((name, api) => true);
        });

        #endregion Swagger

        #region Project Dependencies

        _ = builder.Services.AddInfrastructure();
        _ = builder.Services.AddApplication();

        #endregion Project Dependencies

        return builder;
    }

    public static WebApplication ConfigureApplication(this WebApplication app)
    {
        #region Exceptions

        _ = app.UseGlobalExceptionHandler();

        #endregion Exceptions

        #region Logging

        _ = app.UseSerilogRequestLogging();

        #endregion Logging

        #region Swagger

        var ti = CultureInfo.CurrentCulture.TextInfo;

        _ = app.UseSwagger();
        _ = app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", $"CleanMinimalApi - {ti.ToTitleCase(app.Environment.EnvironmentName)} - V1"));

        #endregion Swagger

        #region Security

        _ = app.UseHsts();

        #endregion Security

        #region API Configuration

        _ = app.UseHttpsRedirection();

        #endregion API Configuration

        return app;
    }
}
