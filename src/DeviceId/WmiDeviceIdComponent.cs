namespace DeviceId
{
    using System;
    using System.Collections.Generic;
    using System.Management;

    /// <summary>
    /// An implementation of IDeviceIdComponent that retrieves data from a WMI class.
    /// </summary>
    public sealed class WmiDeviceIdComponent : IDeviceIdComponent
    {
        /// <summary>
        /// The name of the component.
        /// </summary>
        private readonly string name;

        /// <summary>
        /// The WMI class name.
        /// </summary>
        private readonly string wmiClass;

        /// <summary>
        /// The WMI property name.
        /// </summary>
        private readonly string wmiProperty;

        /// <summary>
        /// Initializes a new instance of the WmiDeviceIdComponent class.
        /// </summary>
        /// <param name="name">The name of the component..</param>
        /// <param name="wmiClass">The WMI class name.</param>
        /// <param name="wmiProperty">The WMI property name.</param>
        public WmiDeviceIdComponent(string name, string wmiClass, string wmiProperty)
        {
            this.name = name;
            this.wmiClass = wmiClass;
            this.wmiProperty = wmiProperty;
        }

        /// <summary>
        /// Gets the name of the component.
        /// </summary>
        public string Name { get { return this.name; } }

        /// <summary>
        /// Gets the component value.
        /// </summary>
        /// <returns>The component value.</returns>
        public string GetValue()
        {
            List<string> values = new List<string>();

            using (ManagementClass mc = new ManagementClass(this.wmiClass))
            {
                foreach (ManagementObject mo in mc.GetInstances())
                {
                    try
                    {
                        string value = mo[this.wmiProperty] as string;
                        if (value != null)
                        {
                            values.Add(value);
                        }
                    }
                    finally
                    {
                        mo.Dispose();
                    }
                }
            }

            return String.Join(",", values);
        }
    }
}
