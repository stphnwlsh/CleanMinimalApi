using CleanMinimalApi.Presentation.Endpoints.Authors;
using CleanMinimalApi.Presentation.Endpoints.Movies;
using CleanMinimalApi.Presentation.Endpoints.Reviews;
using CleanMinimalApi.Presentation.Endpoints.Version;
using CleanMinimalApi.Presentation.Extensions;
using Serilog;

var builder = WebApplication
                .CreateBuilder(args)
                .ConfigureBuilder();
var app = builder
            .Build()
            .ConfigureApplication();

_ = app.MapVersionEndpoints();
_ = app.MapAuthorEndpoints();
_ = app.MapMovieEndpoints();
_ = app.MapReviewEndpoints();

try
{
    Log.Information("Starting host");
    app.Run();
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}
