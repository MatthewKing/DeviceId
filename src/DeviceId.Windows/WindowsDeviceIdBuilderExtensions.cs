using DeviceId.Windows.Components;

namespace DeviceId.Windows
{
    /// <summary>
    /// Extension methods for <see cref="WindowsDeviceIdBuilder"/>.
    /// </summary>
    public static class WindowsDeviceIdBuilderExtensions
    {
        /// <summary>
        /// Adds a registry value to the device identifier.
        /// </summary>
        /// <param name="builder">The <see cref="WindowsDeviceIdBuilder"/> to add the component to.</param>
        /// <param name="componentName">The name of the component.</param>
        /// <param name="registryKeyName">The full path of the registry key.</param>
        /// <param name="registryValueName">The name of the registry value.</param>
        /// <returns>The <see cref="WindowsDeviceIdBuilder"/> instance.</returns>
        public static WindowsDeviceIdBuilder AddRegistryValue(this WindowsDeviceIdBuilder builder, string componentName, string registryKeyName, string registryValueName)
        {
            return builder.AddComponent(componentName, new RegistryValueDeviceIdComponent(registryKeyName, registryValueName));
        }

        /// <summary>
        /// Adds the Machine GUID (from HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Cryptography\MachineGuid) to the device identifier.
        /// </summary>
        /// <param name="builder">The <see cref="WindowsDeviceIdBuilder"/> to add the component to.</param>
        /// <returns>The <see cref="WindowsDeviceIdBuilder"/> instance.</returns>
        public static WindowsDeviceIdBuilder AddMachineGuid(this WindowsDeviceIdBuilder builder)
        {
            return AddRegistryValue(builder,
                "MachineGuid",
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Cryptography",
                "MachineGuid");
        }
    }
}
