using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using DeviceId.Internal;

namespace DeviceId.Components
{
    /// <summary>
    /// An implementation of <see cref="IDeviceIdComponent"/> that retrieves data from installed network adapters.
    /// </summary>
    /// <remarks>
    /// Based on Win32_NetworkAdapter WMI class or using the CIMv2 based MSFT_NetAdapter WMI class (Windows 8 and up only).
    /// Optionally filters out non physical network adapters (not related to virtual machines) and wireless adapters.
    /// </remarks>
    public class NetworkAdapterDeviceIdComponent : IDeviceIdComponent
    {
        /// <summary>
        /// Gets the name of the component.
        /// </summary>
        public string Name { get; } = "MACAddress";

        /// <summary>
        /// A value indicating whether non-physical adapters should be excluded.
        /// </summary>

        private readonly bool _excludeNonPhysical;

        /// <summary>
        /// A value indicating whether wireless adapters should be excluded.
        /// </summary>

        private readonly bool _excludeWireless;

        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkAdapterDeviceIdComponent"/> class.
        /// </summary>
        /// <param name="excludeNonPhysical">A value indicating whether non-physical adapters should be excluded.</param>
        /// <param name="excludeWireless">Indicates if wireless adapters should be excluded.</param>
        /// <remarks>
        /// Non physical adapters are unlikely to have a stable MAC address.
        /// For wireless adapters MAC randomization is a frequently offered function, making it unsuitable for identifying a device.
        /// </remarks>
        public NetworkAdapterDeviceIdComponent(bool excludeNonPhysical, bool excludeWireless)
        {
            _excludeNonPhysical = excludeNonPhysical;
            _excludeWireless = excludeWireless;
        }

        /// <summary>
        /// Gets the component value.
        /// </summary>
        /// <returns>The component value.</returns>
        public string GetValue()
        {
            List<string> values = null;

            if (OS.IsWindows)
            {
                try
                {
                    values = Wmi.GetMacAddressesUsingMSFTNetAdapter(_excludeNonPhysical, _excludeWireless);
                }
                catch
                {
                    try
                    {
                        values = Wmi.GetMacAddressesUsingWin32NetworkAdapter(_excludeNonPhysical);
                    }
                    catch
                    {

                    }
                }
            }

            // If we're on a non-Windows OS, or if the above two methods failed, we have the following fallback:
            if (values == null)
            {
                try
                {
                    values = NetworkInterface.GetAllNetworkInterfaces()
                        .Where(x => !_excludeWireless || x.NetworkInterfaceType != NetworkInterfaceType.Wireless80211)
                        .Select(x => x.GetPhysicalAddress().ToString())
                        .Where(x => x != "000000000000")
                        .Select(x => MacAddressFormatter.FormatMacAddress(x))
                        .ToList();
                }
                catch
                {

                }
            }

            if (values != null)
            {
                values.Sort();
            }

            return (values != null && values.Count > 0)
                ? string.Join(",", values.ToArray())
                : null;
        }
    }
}
