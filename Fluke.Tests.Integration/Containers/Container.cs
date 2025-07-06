using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Networks;

namespace Fluke.Tests.Integration.Containers;

public class Container(string image, INetwork network, int port) : IAsyncDisposable
{
    private IContainer? _container;
    private int Port { get; } = port;
    private string Image { get; } = image;
    private INetwork Network { get; } = network;
    public string? Id => _container?.Id;
    public TestcontainersStates State => _container?.State ?? TestcontainersStates.Undefined;

    public async Task<ushort> StartContainerAsync()
    {
        _container ??= new ContainerBuilder()
            .WithImage(Image)
            .WithNetwork(Network)
            .WithPortBinding(Port, true)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(Port))
            .Build();
        
        if (_container.State != TestcontainersStates.Running || _container.State != TestcontainersStates.Restarting)
            await _container.StartAsync().ConfigureAwait(false);
        
        var port = _container.GetMappedPublicPort(Port);
        return port;
    }

    public async Task StopContainerAsync()
    {
        if (_container == null && _container?.State == TestcontainersStates.Exited)
            return;
        await _container!.StopAsync().ConfigureAwait(false);
    }

    public async ValueTask DisposeAsync()
    {
        if (_container != null) await _container.DisposeAsync();
        await Network.DisposeAsync();
    }
}