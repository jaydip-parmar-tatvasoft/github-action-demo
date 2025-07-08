using Data.Options;
using Microsoft.Extensions.Options;

namespace GitHubActionDemo.Options;

public class DatabaseOptionsSetup : IConfigureOptions<DatabaseOptions>
{
    private const string DatabaseName = "GitHubActionDemo";
    private const string ConfigurationSectionName = "DatabaseOptions";
    private readonly IConfiguration mConfiguration;

    public DatabaseOptionsSetup(IConfiguration configuration)
    {
        mConfiguration = configuration;
    }
    public void Configure(DatabaseOptions options)
    {
        options.ConnectionString = mConfiguration.GetConnectionString(DatabaseName) ?? DatabaseName;
        mConfiguration.GetSection(ConfigurationSectionName).Bind(options);
    }
}
