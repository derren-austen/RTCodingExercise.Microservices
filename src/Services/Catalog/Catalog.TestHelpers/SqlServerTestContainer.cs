using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;

using Xunit;

namespace Catalog.TestHelpers;

public class SqlServerTestContainer : IAsyncLifetime
{
    private const string Username = "sa";
    private const string Password = "Pass@word";
    private const ushort MsSqlPort = 1433;

    private readonly IContainer _container =
        new ContainerBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .WithPortBinding(MsSqlPort, true)
            .WithEnvironment("ACCEPT_EULA", "Y")
            .WithEnvironment("SQLCMDUSER", Username)
            .WithEnvironment("SQLCMDPASSWORD", Password)
            .WithEnvironment("MSSQL_SA_PASSWORD", Password)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(MsSqlPort))
            .Build();

    public string Host => _container.Hostname;
    public ushort Port => _container.GetMappedPublicPort(MsSqlPort);

    public Task InitializeAsync() => _container.StartAsync();
    public Task DisposeAsync() => _container.DisposeAsync().AsTask();
}
