namespace CleanMinimalApi.Presentation.Serialization;

using System.Text.Json.Serialization;
using Application.Authors.Entities;
using Application.Movies.Entities;
using Application.Reviews.Entities;
using Application.Versions.Entities;
using Requests;

/// <summary>
/// JSON serialization context for compile-time source generation.
/// Provides performance benefits and AOT (Ahead-of-Time) compilation support.
/// </summary>
[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    PropertyNameCaseInsensitive = true,
    WriteIndented = false,
    GenerationMode = JsonSourceGenerationMode.Default)]
[JsonSerializable(typeof(Author))]
[JsonSerializable(typeof(List<Author>))]
[JsonSerializable(typeof(Movie))]
[JsonSerializable(typeof(List<Movie>))]
[JsonSerializable(typeof(Review))]
[JsonSerializable(typeof(List<Review>))]
[JsonSerializable(typeof(ReviewAuthor))]
[JsonSerializable(typeof(ReviewedMovie))]
[JsonSerializable(typeof(Version))]
[JsonSerializable(typeof(List<Version>))]
[JsonSerializable(typeof(CreateReviewRequest))]
[JsonSerializable(typeof(UpdateReviewRequest))]
public partial class AppJsonSerializerContext : JsonSerializerContext
{
}
