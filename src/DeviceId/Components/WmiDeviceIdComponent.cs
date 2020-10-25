using System.Collections.Generic;
using System.Management;

namespace DeviceId.Components
{
    /// <summary>
    /// An implementation of <see cref="IDeviceIdComponent"/> that retrieves data from a WMI class.
    /// </summary>
    public class WmiDeviceIdComponent : IDeviceIdComponent
    {
        /// <summary>
        /// Gets the name of the component.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The WMI class name.
        /// </summary>
        private readonly string _wmiClass;

        /// <summary>
        /// The WMI property name.
        /// </summary>
        private readonly string _wmiProperty;

        /// <summary>
        /// Initializes a new instance of the <see cref="WmiDeviceIdComponent"/> class.
        /// </summary>
        /// <param name="name">The name of the component.</param>
        /// <param name="wmiClass">The WMI class name.</param>
        /// <param name="wmiProperty">The WMI property name.</param>
        public WmiDeviceIdComponent(string name, string wmiClass, string wmiProperty)
        {
            Name = name;
            _wmiClass = wmiClass;
            _wmiProperty = wmiProperty;
        }

        /// <summary>
        /// Gets the component value.
        /// </summary>
        /// <returns>The component value.</returns>
        public string GetValue()
        {
            var values = new List<string>();

            try
            {
                using var managementObjectSearcher = new ManagementObjectSearcher($"SELECT {_wmiProperty} FROM {_wmiClass}");
                using var managementObjectCollection = managementObjectSearcher.Get();
                foreach (var managementObject in managementObjectCollection)
                {
                    try
                    {
                        var value = managementObject[_wmiProperty] as string;
                        if (value != null)
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

            values.Sort();

            return (values != null && values.Count > 0)
                ? string.Join(",", values.ToArray())
                : null;
        }
    }
}
