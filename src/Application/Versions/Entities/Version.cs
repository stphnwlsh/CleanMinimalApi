namespace CleanMinimalApi.Application.Versions.Entities;

/// <summary>
/// The version details of the running application.
/// </summary>
public class Version
{
    /// <summary>
    /// The assembly file version.
    /// </summary>
    /// <example>1.1.0.0</example>
    public required string FileVersion { get; init; }

    /// <summary>
    /// The informational version, which can include a pre-release label or commit hash.
    /// </summary>
    /// <example>1.1.0+e30eba8</example>
    public required string InformationalVersion { get; init; }
}
