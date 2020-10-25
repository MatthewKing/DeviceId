using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;

namespace DeviceId.Components
{
    /// <summary>
    /// An implementation of <see cref="IDeviceIdComponent"/> that retrieves data from installed network adapaters.
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
                    // First attempt to retrieve the addresses using the CIMv2 interface.
                    values = GetMacAddressesUsingCimV2();
                }
                catch (ManagementException ex)
                {
                    // In case we are notified of an invalid namespace, attempt to lookup the adapters using WMI.
                    // Could avoid this catch by manually checking for the CIMv2 namespace.

                    if (ex.ErrorCode == ManagementStatus.InvalidNamespace)
                    {
                        values = GetMacAddressesUsingWmi();
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
                        .Select(x => FormatMacAddress(x))
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

        /// <summary>
        /// Retrieves the MAC addresses using the (old) Win32_NetworkAdapter WMI class.
        /// </summary>
        /// <returns>A list of MAC addresses.</returns>
        internal List<string> GetMacAddressesUsingWmi()
        {
            var values = new List<string>();

            try
            {
                using var managementObjectSearcher = new ManagementObjectSearcher("select MACAddress, PhysicalAdapter from Win32_NetworkAdapter");
                using var managementObjectCollection = managementObjectSearcher.Get();
                foreach (var managementObject in managementObjectCollection)
                {
                    try
                    {
                        // Skip non physcial adapters if instructed to do so.
                        var isPhysical = (bool)managementObject["PhysicalAdapter"];
                        if (_excludeNonPhysical && !isPhysical)
                        {
                            continue;
                        }

                        var macAddress = (string)managementObject["MACAddress"];
                        if (!string.IsNullOrEmpty(macAddress))
                        {
                            values.Add(macAddress);
                        }
                    }
                    finally
                    {
                        managementObject.Dispose();
                    }
                }
            }
            catch
            {

            }

            return values;
        }

        /// <summary>
        /// Retrieves the MAC addresses using the CIMv2 based MSFT_NetAdapter interface (Windows 8 and up).
        /// </summary>
        /// <returns>A list of MAC addresses.</returns>
        internal List<string> GetMacAddressesUsingCimV2()
        {
            var values = new List<string>();

            using var managementClass = new ManagementClass("root/StandardCimv2", "MSFT_NetAdapter", new ObjectGetOptions { });

            foreach (var managementInstance in managementClass.GetInstances())
            {
                try
                {
                    // Skip non physcial adapters if instructed to do so.
                    var isPhysical = (bool)managementInstance["ConnectorPresent"];
                    if (_excludeNonPhysical && !isPhysical)
                    {
                        continue;
                    }

                    // Skip wireless adapters if instructed to do so.
                    var ndisMedium = (uint)managementInstance["NdisPhysicalMedium"];
                    if (_excludeWireless && ndisMedium == 9) // Native802_11
                    {
                        continue;
                    }

                    // Add the MAC address to the list of values.
                    var value = managementInstance["PermanentAddress"] as string;
                    if (value != null)
                    {
                        // Ensure the hardware addresses are formatted as MAC addresses if possible.
                        // This is a discrepancy between the MSFT_NetAdapter and Win32_NetworkAdapter interfaces.
                        value = FormatMacAddress(value);
                        values.Add(value);
                    }
                }
                finally
                {
                    managementInstance.Dispose();
                }
            }

            return values;
        }

        /// <summary>
        /// Formats the specified MAC address.
        /// </summary>
        /// <param name="input">The MAC address to format.</param>
        /// <returns>The formatted MAC address.</returns>
        internal static string FormatMacAddress(string input)
        {
            // Check if this can be a hex formatted EUI-48 or EUI-64 identifier.
            if (input.Length != 12 && input.Length != 16)
            {
                return input;
            }

            // Chop up input in 2 character chunks.
            var partSize = 2;
            var parts = Enumerable.Range(0, input.Length / partSize).Select(x => input.Substring(x * partSize, partSize));

            // Put the parts in the AA:BB:CC format.
            var result = string.Join(":", parts.ToArray());

            return result;
        }
    }
}
