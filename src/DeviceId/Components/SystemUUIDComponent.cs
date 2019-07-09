using System;
using System.Management;

namespace DeviceId.Components
{
    /// <summary>
    /// An implementation of <see cref="IDeviceIdComponent"/> that uses the system UUID.
    /// </summary>
    public class SystemUUIDComponent : IDeviceIdComponent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemUUIDComponent"/> class.
        /// </summary>
        public SystemUUIDComponent() { }

        /// <summary>
        /// Gets the name of the component.
        /// </summary>
        public string Name => "SystemUUID";

        /// <summary>
        /// Gets the component value.
        /// </summary>
        /// <returns>The component value.</returns>
        public string GetValue()
        {
            using (ManagementClass mc = new ManagementClass("Win32_ComputerSystemProduct"))
            {
                foreach (ManagementObject mo in mc.GetInstances())
                {
                    try
                    {
                        string value = mo["UUID"] as string;
                        if (!String.IsNullOrEmpty(value))
                        {
                            return value;
                        }
                    }
                    finally
                    {
                        mo.Dispose();
                    }
                }
            }

            return null;
        }
    }
}
