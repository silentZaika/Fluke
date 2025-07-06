using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;

namespace Fluke.Tests.Integration.Utilities;

public static class ImageBuilder
{
    public static async Task<string> BuildImage(string projectName)
    {
        var image = new ImageFromDockerfileBuilder()
            .WithDockerfileDirectory(CommonDirectoryPath.GetSolutionDirectory(), string.Empty)
            .WithDockerfile(Path.Combine(projectName, "Dockerfile"))
            .WithBuildArgument("RESOURCE_REAPER_SESSION_ID", ResourceReaper.DefaultSessionId.ToString("D"))
            .Build();
        
        await image.CreateAsync().ConfigureAwait(false);
        return image.FullName;
    }
}