using System;
using System.Collections.Generic;

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
            try
            {
                var systemLogicalDiskDeviceId = Environment.GetFolderPath(Environment.SpecialFolder.System).Substring(0, 2);

                var logicalDiskToPartition = Array.Find(WmiHelper.GetWMIInstances(@"root\cimv2", "Win32_LogicalDiskToPartition"), logicalDriveToPartition => (logicalDriveToPartition["Dependent"] as IDictionary<string, object>)["DeviceID"] as string == systemLogicalDiskDeviceId);
                var partition = Array.Find(WmiHelper.GetWMIInstances(@"root\cimv2", "Win32_DiskDriveToDiskPartition"), partition => (partition["Dependent"] as IDictionary<string, object>)["DeviceID"] as string == (logicalDiskToPartition["Antecedent"] as IDictionary<string, object>)["DeviceID"] as string);
                var diskdrive = Array.Find(WmiHelper.GetWMIInstances(@"root\cimv2", "Win32_DiskDrive "), diskDriveToPartition => diskDriveToPartition["DeviceID"] as string == (partition["Antecedent"] as IDictionary<string, object>)["DeviceID"] as string);
                return diskdrive["SerialNumber"] as string;
            }
            catch(Exception e)
            {
                throw new DeviceIdComponentFailedToObtainValueException("Failed to GetValue() in SystemDriveSerialNumberDeviceIdComponent", e);
            }
        }
    }
}
