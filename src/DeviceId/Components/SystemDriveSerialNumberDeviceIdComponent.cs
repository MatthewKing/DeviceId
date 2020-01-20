using System;
using System.Management;

namespace DeviceId.Components
{
    /// <summary>
    /// An implementation of <see cref="IDeviceIdComponent"/> that uses the system drive's serial number.
    /// </summary>
    public class SystemDriveSerialNumberDeviceIdComponent : IDeviceIdComponent
    {
        /// <summary>
        /// Gets the name of the component.
        /// </summary>
        public string Name { get; } = "SystemDriveSerialNumber";

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemDriveSerialNumberDeviceIdComponent"/> class.
        /// </summary>
        public SystemDriveSerialNumberDeviceIdComponent() { }

        /// <summary>
        /// Gets the component value.
        /// </summary>
        /// <returns>The component value.</returns>
        public string GetValue()
        {
            var systemLogicalDiskDeviceId = Environment.GetFolderPath(Environment.SpecialFolder.System).Substring(0, 2);

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

            return null;
        }
    }
}
