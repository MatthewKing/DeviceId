using System.Collections.Generic;

namespace DeviceId.Linux.Serialization;

internal sealed class LsblkOutput
{
    public List<LsblkDevice> BlockDevices { get; set; } = new List<LsblkDevice>();
}
