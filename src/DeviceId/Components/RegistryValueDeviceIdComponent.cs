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
        /// The path of the registry key to look at.
        /// </summary>
        private readonly string _key;

        /// <summary>
        /// The name of the registry value.
        /// </summary>
        private readonly string _valueName;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileTokenDeviceIdComponent"/> class.
        /// </summary>
        /// <param name="name">The name of the component.</param>
        /// <param name="key">The full path of the registry key.</param>
        /// <param name="valueName">The name of the registry value.</param>
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
            try
            {
                var value = Registry.GetValue(_key, _valueName, null);
                return value?.ToString();
            }
            catch
            {
                return null;
            }
        }
    }
}
