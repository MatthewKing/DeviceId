using System;
using System.Collections.Generic;

#if NETFRAMEWORK
using System.Management;
#elif (NETSTANDARD || NET5_0_OR_GREATER)
using Microsoft.Management.Infrastructure;
#endif

namespace DeviceId.Internal
{
    internal static class Wmi
    {
        public static List<string> GetValues(string className, string propertyName)
        {
            var values = new List<string>();

#if NETFRAMEWORK
            try
            {
                using var managementObjectSearcher = new ManagementObjectSearcher($"SELECT {propertyName} FROM {className}");
                using var managementObjectCollection = managementObjectSearcher.Get();
                foreach (var managementObject in managementObjectCollection)
                {
                    try
                    {
                        if (managementObject[propertyName] is string value)
                        {
                            values.Add(value);
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
#elif (NETSTANDARD || NET5_0_OR_GREATER)
            try
            {
                using var session = CimSession.Create(null);

                var instances = session.QueryInstances(@"root\cimv2", "WQL", $"SELECT {propertyName} FROM {className}");
                foreach (var instance in instances)
                {
                    try
                    {
                        var value = instance.CimInstanceProperties[propertyName].Value as string;
                        if (value != null)
                        {
                            values.Add(value);
                        }
                    }
                    finally
                    {
                        instance.Dispose();
                    }
                }
            }
            catch
            {

            }
#endif

            values.Sort();

            return values;
        }

        public static string GetSystemDriveSerialNumber()
        {
            var systemLogicalDiskDeviceId = Environment.GetFolderPath(Environment.SpecialFolder.System).Substring(0, 2);

#if NETFRAMEWORK
            var queryString = $"SELECT * FROM Win32_LogicalDisk where DeviceId = '{systemLogicalDiskDeviceId}'";
            using var searcher = new ManagementObjectSearcher(queryString);

            foreach (ManagementObject disk in searcher.Get())
            {
                foreach (ManagementObject partition in disk.GetRelated("Win32_DiskPartition"))
                {
                    foreach (ManagementObject drive in partition.GetRelated("Win32_DiskDrive"))
                    {
                        var serialNumber = drive["SerialNumber"] as string;
                        return serialNumber;
                    }
                }
            }
#elif (NETSTANDARD || NET5_0_OR_GREATER)
            using var session = CimSession.Create(null);

            foreach (var logicalDiskAssociator in session.QueryInstances(@"root\cimv2", "WQL", $"ASSOCIATORS OF {{Win32_LogicalDisk.DeviceID=\"{systemLogicalDiskDeviceId}\"}}"))
            {
                if (logicalDiskAssociator.CimClass.CimSystemProperties.ClassName == "Win32_DiskPartition")
                {
                    if (logicalDiskAssociator.CimInstanceProperties["DeviceId"].Value is string diskPartitionDeviceId)
                    {
                        foreach (var diskPartitionAssociator in session.QueryInstances(@"root\cimv2", "WQL", $"ASSOCIATORS OF {{Win32_DiskPartition.DeviceID=\"{diskPartitionDeviceId}\"}}"))
                        {
                            if (diskPartitionAssociator.CimClass.CimSystemProperties.ClassName == "Win32_DiskDrive")
                            {
                                if (diskPartitionAssociator.CimInstanceProperties["SerialNumber"].Value is string diskDriveSerialNumber)
                                {
                                    return diskDriveSerialNumber;
                                }
                            }
                        }
                    }
                }
            }
#endif

            return null;
        }

        public static List<string> GetMacAddressesUsingWin32NetworkAdapter(bool excludeNonPhysical)
        {
            var values = new List<string>();

#if NETFRAMEWORK
            try
            {
                using var managementObjectSearcher = new ManagementObjectSearcher("select MACAddress, PhysicalAdapter from Win32_NetworkAdapter");
                using var managementObjectCollection = managementObjectSearcher.Get();
                foreach (var managementObject in managementObjectCollection)
                {
                    try
                    {
                        // Skip non-physical adapters if instructed to do so.
                        var isPhysical = (bool)managementObject["PhysicalAdapter"];
                        if (excludeNonPhysical && !isPhysical)
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
#elif (NETSTANDARD || NET5_0_OR_GREATER)

            using var session = CimSession.Create(null);
            foreach (var instance in session.QueryInstances(@"root\cimv2", "WQL", "select MACAddress, PhysicalAdapter from Win32_NetworkAdapter"))
            {
                if (instance.CimInstanceProperties["PhysicalAdapter"].Value is bool isPhysical)
                {
                    if (excludeNonPhysical && !isPhysical)
                    {
                        continue;
                    }
                }

                if (instance.CimInstanceProperties["MACAddress"].Value is string macAddress)
                {
                    if (!string.IsNullOrEmpty(macAddress))
                    {
                        values.Add(macAddress);
                    }
                }
            }
#endif

            values.Sort();

            return values;
        }

        public static List<string> GetMacAddressesUsingMSFTNetAdapter(bool excludeNonPhysical, bool excludeWireless)
        {
            var values = new List<string>();

#if NETFRAMEWORK
            using var managementClass = new ManagementClass("root/StandardCimv2", "MSFT_NetAdapter", new ObjectGetOptions { });

            foreach (var managementInstance in managementClass.GetInstances())
            {
                try
                {
                    // Skip non-physical adapters if instructed to do so.
                    if (managementInstance["ConnectorPresent"] is bool isPhysical)
                    {
                        if (excludeNonPhysical && !isPhysical)
                        {
                            continue;
                        }
                    }

                    // Skip wireless adapters if instructed to do so.
                    if (managementInstance["NdisPhysicalMedium"] is uint ndisMedium)
                    {
                        if (excludeWireless && ndisMedium == 9) // Native802_11
                        {
                            continue;
                        }
                    }

                    // Add the MAC address to the list of values.
                    if (managementInstance["PermanentAddress"] is string value)
                    {
                        // Ensure the hardware addresses are formatted as MAC addresses if possible.
                        // This is a discrepancy between the MSFT_NetAdapter and Win32_NetworkAdapter interfaces.
                        value = MacAddressFormatter.FormatMacAddress(value);
                        values.Add(value);
                    }
                }
                finally
                {
                    managementInstance.Dispose();
                }
            }
#elif (NETSTANDARD || NET5_0_OR_GREATER)
            using var session = CimSession.Create(null);

            foreach (var instance in session.EnumerateInstances("root/StandardCimv2", "MSFT_NetAdapter"))
            {
                // Skip non-physical adapters if instructed to do so.
                if (instance.CimInstanceProperties["ConnectorPresent"].Value is bool connectorPresent)
                {
                    if (excludeNonPhysical && !connectorPresent)
                    {
                        continue;
                    }
                }

                // Skip wireless adapters if instructed to do so.
                if (instance.CimInstanceProperties["NdisPhysicalMedium"].Value is uint ndisMedium)
                {
                    if (excludeWireless && ndisMedium == 9) // Native802_11
                    {
                        continue;
                    }
                }

                if (instance.CimInstanceProperties["PermanentAddress"].Value is string macAddress)
                {
                    if (!string.IsNullOrEmpty(macAddress))
                    {
                        values.Add(MacAddressFormatter.FormatMacAddress(macAddress));
                    }
                }
            }
#endif

            values.Sort();

            return values;
        }


    }
}
