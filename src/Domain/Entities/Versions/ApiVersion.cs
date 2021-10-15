namespace CleanMinimalApi.Domain.Entities.Versions;

public class ApiVersion
{
    public string FileVersion { get; set; }
    public string InformationalVersion { get; set; }

    public ApiVersion(string fileVersion, string informationalVersion)
    {
        this.FileVersion = fileVersion;
        this.InformationalVersion = informationalVersion;
    }
}
