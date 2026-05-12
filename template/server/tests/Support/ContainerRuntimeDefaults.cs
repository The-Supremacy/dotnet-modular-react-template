using System.Runtime.InteropServices;

namespace ModularTemplate.Tests.Support;

internal static class ContainerRuntimeDefaults
{
    public static void Apply()
    {
        if (Environment.GetEnvironmentVariable("DOCKER_HOST") is { Length: > 0 } ||
            !OperatingSystem.IsLinux())
        {
            return;
        }

        foreach (var socketPath in GetRootlessPodmanSocketCandidates())
        {
            if (!File.Exists(socketPath))
            {
                continue;
            }

            Environment.SetEnvironmentVariable("DOCKER_HOST", $"unix://{socketPath}");

            if (Environment.GetEnvironmentVariable("TESTCONTAINERS_RYUK_DISABLED") is not { Length: > 0 })
            {
                Environment.SetEnvironmentVariable("TESTCONTAINERS_RYUK_DISABLED", "true");
            }

            return;
        }
    }

    private static IEnumerable<string> GetRootlessPodmanSocketCandidates()
    {
        if (Environment.GetEnvironmentVariable("XDG_RUNTIME_DIR") is { Length: > 0 } runtimeDirectory)
        {
            yield return Path.Combine(runtimeDirectory, "podman", "podman.sock");
        }

        yield return Path.Combine("/run", "user", GetEffectiveUserId().ToString(), "podman", "podman.sock");
    }

    [DllImport("libc")]
    private static extern uint geteuid();

    private static uint GetEffectiveUserId()
    {
        try
        {
            return geteuid();
        }
        catch (DllNotFoundException)
        {
            return 0;
        }
    }
}
