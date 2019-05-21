using System;
using System.Management;

namespace DeviceId.Components
{
    /// <summary>
    /// An implementation of <see cref="IDeviceIdComponent"/> that uses the UUID number.
    /// </summary>
    public class UUIDComponent : IDeviceIdComponent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UUIDComponent"/> class.
        /// </summary>
        public UUIDComponent() { }

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
            var result = "";
            var mc = new ManagementClass("Win32_ComputerSystemProduct");
            var moc = mc.GetInstances();
            foreach (var mo in moc)
            {
                //Only get the first one
                if (result != "") continue;
                try
                {
                    var moWmi = mo["UUID"];
                    if (moWmi == null)
                        continue;
                    result = moWmi.ToString();
                    break;
                }
                catch
                {
                    // ignored
                }
            }

            return result;
        }
    }
}
