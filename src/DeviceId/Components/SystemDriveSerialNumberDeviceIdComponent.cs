using DeviceId.Internal;

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
            return Wmi.GetSystemDriveSerialNumber();
        }
    }
}
