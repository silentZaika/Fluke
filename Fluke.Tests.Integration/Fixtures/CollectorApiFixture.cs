using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Fluke.Tests.Integration.Containers;
using Fluke.Tests.Integration.Utilities;

namespace Fluke.Tests.Integration.Fixtures;

public class CollectorApiFixture
{
    public async Task<ushort> InitializeAsync()
    {
        var image = await ImageBuilder.BuildImage("Fluke.CollectorAPI");
        var network = new NetworkBuilder().Build();

        var container = new Container(image, network, 7173);
        return await container.StartContainerAsync().ConfigureAwait(false);
    }
}