namespace ProjectManagement.ProjectAPI.Configuration;

[ExcludeFromCodeCoverage]
public class TelemetrySettings
{
    public string Endpoint { get; set; } = string.Empty;

    public string ServiceName { get; set; } = string.Empty;

    public string ServiceVersion { get; set; } = string.Empty;

    public bool EnableAlwaysOnSampler { get; set; } = false;

    public double SampleProbability { get; set; } = 0.2;

    public bool EnableConsoleExporter { get; set; } = false;
}