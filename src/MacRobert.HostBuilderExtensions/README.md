# HostBuilder Extensions

The Hostbuilder exctensions utility allows developers to decompose large appsettings.json files into smaller, environment-specific config files.
This permites greater control in deployments as well as facilitating re-use of configurations between projects, so such configurations need only be modified once.

## Example

```csharp
public static class MyApplicationHostBuilderExtensions
{
    public static IHostBuilder AddMyApplicationSettings(this IHostBuilder hostBuilder)
    {
        return hostBuilder
            .AddAllowedHostsSettings()
            .AddConnectionStrings()
            .AddOtherSettings()...;
    }

    public const string AppSettingsRoot = "appsettings";

    //////////////////////////////////////////////////////////////////////////////////////////
    // Allowed Hosts

    private const string AllowedHostsSettingsJsonPathFormat = "allowed_hosts.{0}.json";
    private const string AllowedHostsSettingsJsonPath = "allowed_hosts.json";

    private static IHostBuilder AddAllowedHostsSettings(
        this IHostBuilder hostBuilder,
        bool optional = true,
        bool reloadOnChange = true)
    {
        return hostBuilder.AddSettings(
            AllowedHostsSettingsJsonPath,
            AllowedHostsSettingsJsonPathFormat,
            AppSettingsRoot,
            optional,
            reloadOnChange);
    }

    //////////////////////////////////////////////////////////////////////////////////////////
    // Connection Strings

    private const string ConnectionStringsJsonPathFormat = "connection_strings.{0}.json";
    private const string ConnectionStringsJsonPath = "connection_strings.json";

    private static IHostBuilder AddConnectionStrings(
        this IHostBuilder hostBuilder,
        bool optional = true,
        bool reloadOnChange = true)
    {
        return hostBuilder.AddSettings(
            ConnectionStringsJsonPath,
            ConnectionStringsJsonPathFormat,
            AppSettingsRoot,
            optional,
            reloadOnChange);
    }

    // ...
}
```

We can have different ```allowedhosts``` as well as different ```connection strings``` by environment each prescribing unique settings, all stored in a common 'appsettings' folder.

So for allowed-hosts we might say
```appsettings/allowed_hosts.json```

```json
{
  "AllowedHosts": "*"
}
```

For the production environment:
```appsettings/allowed_hosts.Production.json```
```json
{
  "AllowedHosts": "*.myapplication.io;myapplication.io"
}
```

In the example above, where an environment file is not defined, it will defer/cascade to the default, so Developent and Staging environments will yield ```*``` where production will only permit subdomain and main domain requests.

The resulting ```BuilderExtension``` extension method would be invoked as shown:

```csharp
    var builder = WebApplication.CreateBuilder(args);
    builder.Host
        .AddXxx()
        ...
        .AddMyApplicationSettings();
```

Which will load the appropriate appsetting files by environment.