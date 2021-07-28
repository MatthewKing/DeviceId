using Microsoft.Win32;

namespace DeviceId.Windows.Components
{
    /// <summary>
    /// An implementation of <see cref="IDeviceIdComponent"/> that retrieves its value from the Windows registry.
    /// </summary>
    public class RegistryValueDeviceIdComponent : IDeviceIdComponent
    {
        /// <summary>
        /// The path of the registry key to look at.
        /// </summary>
        private readonly string _keyName;

        /// <summary>
        /// The name of the registry value.
        /// </summary>
        private readonly string _valueName;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryValueDeviceIdComponent"/> class.
        /// </summary>
        /// <param name="keyName">The full path of the registry key.</param>
        /// <param name="valueName">The name of the registry value.</param>
        public RegistryValueDeviceIdComponent(string keyName, string valueName)
        {
            _keyName = keyName;
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
                var value = Registry.GetValue(_keyName, _valueName, null);
                return value?.ToString();
            }
            catch { }

            return null;
        }
    }
}
