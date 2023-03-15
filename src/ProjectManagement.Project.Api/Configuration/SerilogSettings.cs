namespace ProjectManagement.ProjectAPI.Configuration;

[ExcludeFromCodeCoverage]
public class SerilogSettings
{
    public SerilogElasticSearchSettings ElasticSearchSettings { get; set; } = new ()
    {
        Password = string.Empty,
        IndexFormat = string.Empty,
        Uri = string.Empty,
        Username = string.Empty,
    };

    public class SerilogElasticSearchSettings
    {
        required public string Uri { get; set; }

        required public string Username { get; set; }

        required public string Password { get; set; }

        public string IndexFormat { get; set; } = string.Empty;
    }
}