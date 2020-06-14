﻿using System.Collections.Generic;
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

            using var mc = new ManagementObjectSearcher($"SELECT {_wmiProperty} FROM {_wmiClass}");

            foreach (var mo in mc.Get())
            {
                try
                {
                    var value = mo[_wmiProperty] as string;
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

            values.Sort();

            return (values != null && values.Count > 0)
                ? string.Join(",", values)
                : null;
        }
    }
}
