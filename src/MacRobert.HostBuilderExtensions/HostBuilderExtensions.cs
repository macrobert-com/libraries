using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace MacRobert.HostBuilderExtensions;

public static class HostBuilderExtensions
{
    private static readonly string[] _Environments =
    {
        Environments.Production,
        Environments.Staging,
        Environments.Development
    };

    public static bool IsValidEnvironment(string environmentName) => _Environments.Contains(environmentName);

    private const string Secrets = "secrets";

    public static IHostBuilder AddSettings(
        this IHostBuilder hostBuilder,
        string path,
        string pathFormat,
        string appSettingsRoot,
        bool optional = true,
        bool reloadOnChange = true)
    {
        path = Path.Combine(appSettingsRoot, path);
        pathFormat = Path.Combine(appSettingsRoot, pathFormat);

        return hostBuilder.ConfigureAppConfiguration((context, builder) =>
        {
            builder.AddJsonFile(
                path: path,
                optional: optional,
                reloadOnChange: reloadOnChange
            );

            var environmentName = context.HostingEnvironment.EnvironmentName;
            if (!IsValidEnvironment(environmentName))
            {
                return;
            }

            var envPath = string.Format(pathFormat, environmentName);
            builder.AddJsonFile(
                path: envPath,
                optional: optional,
                reloadOnChange: reloadOnChange
            );

            var secretsPath = string.Format(pathFormat, Secrets);
            builder.AddJsonFile(
                path: secretsPath,
                optional: optional,
                reloadOnChange: reloadOnChange
            );
        });
    }
}

