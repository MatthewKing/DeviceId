using System.Collections.Generic;

namespace DeviceId.Linux.Serialization;

internal sealed class LsblkDevice
{
    public string Name { get; set; } = string.Empty;
    public string MountPoint { get; set; } = string.Empty;
    public List<LsblkDevice> Children { get; set; } = new List<LsblkDevice>();
}
