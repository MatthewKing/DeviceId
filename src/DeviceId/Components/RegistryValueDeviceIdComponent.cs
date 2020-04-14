using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Win32;

namespace DeviceId.Components
{
    /// <summary>
    /// An implementation of <see cref="IDeviceIdComponent"/> that retrieves its value from the Windows registry.
    /// </summary>
    public class RegistryValueDeviceIdComponent : IDeviceIdComponent
    {
        /// <summary>
        /// Gets the name of the component.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The path of the registry key to look at
        /// </summary>
        private readonly string _key;

        /// <summary>
        /// The name of the registry value
        /// </summary>
        private readonly string _valueName;

        /// <summary>
        /// Value to use when a result is not obtainable
        /// </summary>
        private const string NoValue = "NoValue";

        /// <summary>
        /// Initializes a new instance of the <see cref="FileTokenDeviceIdComponent"/> class.
        /// </summary>
        /// <param name="name">The name of the component.</param>
        /// <param name="key">The path of the registry key to look at.</param>
        /// <param name="valueName">the name of the registry value</param>
        public RegistryValueDeviceIdComponent(string name, string key, string valueName)
        {
            Name = name;
            _key = key;
            _valueName = valueName;
        }

        /// <summary>
        /// Gets the component value.
        /// </summary>
        /// <returns>The component value.</returns>
        public string GetValue()
        {
            return Registry.GetValue(_key, _valueName, NoValue).ToString();
        }
    }
}
