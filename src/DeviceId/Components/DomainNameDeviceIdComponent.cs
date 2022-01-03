using System.Net.NetworkInformation;

namespace DeviceId.Components;

/// <summary>
/// An implementation of <see cref="IDeviceIdComponent"/> that uses the Domain Name of the PC.
/// </summary>
public class DomainNameDeviceIdComponent : IDeviceIdComponent
{
    /// <summary>
    /// Gets the component value.
    /// </summary>
    /// <returns>The component value.</returns>
    public string GetValue()
    {
        return IPGlobalProperties.GetIPGlobalProperties().DomainName;
    }
}
