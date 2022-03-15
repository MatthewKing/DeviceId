using System.IO;
using System.Text.RegularExpressions;

namespace DeviceId.Linux.Components;

/// <summary>
/// An implementation of <see cref="IDeviceIdComponent"/> that uses the cgroup to read the docker container id.
/// </summary>
public class DockerContainerIdComponent : IDeviceIdComponent
{
    private readonly string _cGroupFile;

    /// <summary>
    /// Initializes a new instance of the <see cref="DockerContainerIdComponent"/> class.
    /// </summary>
    public DockerContainerIdComponent()
        :this("/proc/1/cgroup")
    {
    }

    internal DockerContainerIdComponent(string cGroupFile)
    {
        _cGroupFile = cGroupFile;
    }

    /// <summary>
    /// Gets the component value.
    /// </summary>
    /// <returns>The component value.</returns>
    public string GetValue()
    {
        if (string.IsNullOrWhiteSpace(_cGroupFile) || !File.Exists(_cGroupFile))
        {
            return null;
        }
        using (var file = File.OpenText(_cGroupFile))
        {
            if (TryGetContainerId(file, out string containerId))
            {
                return containerId;
            }
        }
        return null;
    }

    private static bool TryGetContainerId(StreamReader reader, out string containerId)
    {
        Regex regex = new("(\\d)+\\:(.)+?\\:(/.+?)??(/docker[-/])([0-9a-f]+)");
        string line;
        while ((line = reader?.ReadLine()) != null)
        {
            Match match = regex.Match(line);
            if (match.Success)
            {
                containerId = match.Groups[5].Value;
                return true;
            }
        }
        containerId = default;
        return false;
    }
}
