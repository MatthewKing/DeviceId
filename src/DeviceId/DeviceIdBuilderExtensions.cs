using System;
using System.Runtime.InteropServices;
using DeviceId.Components;

namespace DeviceId
{
    /// <summary>
    /// Extension methods for <see cref="DeviceIdBuilder"/>.
    /// </summary>
    public static class DeviceIdBuilderExtensions
    {
        /// <summary>
        /// Use the specified formatter.
        /// </summary>
        /// <param name="builder">The <see cref="DeviceIdBuilder"/> to use the formatter.</param>
        /// <param name="formatter">The <see cref="IDeviceIdFormatter"/> to use.</param>
        /// <returns>The <see cref="DeviceIdBuilder"/> instance.</returns>
        public static DeviceIdBuilder UseFormatter(this DeviceIdBuilder builder, IDeviceIdFormatter formatter)
        {
            builder.Formatter = formatter;
            return builder;
        }

        /// <summary>
        /// Adds the specified component to the device identifier.
        /// </summary>
        /// <param name="builder">The <see cref="DeviceIdBuilder"/> to add the component to.</param>
        /// <param name="component">The <see cref="IDeviceIdComponent"/> to add.</param>
        /// <returns>The <see cref="DeviceIdBuilder"/> instance.</returns>
        public static DeviceIdBuilder AddComponent(this DeviceIdBuilder builder, IDeviceIdComponent component)
        {
            builder.Components.Add(component);
            return builder;
        }

        /// <summary>
        /// Adds the current user name to the device identifier. 
        /// </summary>
        /// <param name="builder">The <see cref="DeviceIdBuilder"/> to add the component to.</param>
        /// <returns>The <see cref="DeviceIdBuilder"/> instance.</returns>
        public static DeviceIdBuilder AddUserName(this DeviceIdBuilder builder)
        {
            return builder.AddComponent(new DeviceIdComponent("UserName", Environment.UserName));
        }

        /// <summary>
        /// Adds the machine name to the device identifier.
        /// </summary>
        /// <param name="builder">The <see cref="DeviceIdBuilder"/> to add the component to.</param>
        /// <returns>The <see cref="DeviceIdBuilder"/> instance.</returns>
        public static DeviceIdBuilder AddMachineName(this DeviceIdBuilder builder)
        {
            return builder.AddComponent(new DeviceIdComponent("MachineName", Environment.MachineName));
        }

        /// <summary>
        /// Adds the operating system version to the device identifier.
        /// </summary>
        /// <param name="builder">The <see cref="DeviceIdBuilder"/> to add the component to.</param>
        /// <returns>The <see cref="DeviceIdBuilder"/> instance.</returns>
        public static DeviceIdBuilder AddOSVersion(this DeviceIdBuilder builder)
        {
#if NETFRAMEWORK
            return builder.AddComponent(new DeviceIdComponent("OSVersion", Environment.OSVersion.ToString()));
#else
            return builder.AddComponent(new DeviceIdComponent("OSVersion", Microsoft.DotNet.PlatformAbstractions.RuntimeEnvironment.OperatingSystem + " " + Microsoft.DotNet.PlatformAbstractions.RuntimeEnvironment.OperatingSystemVersion)); // examples are "Ubuntu 18.04"
#endif
        }

        /// <summary>
        /// Adds the MAC address to the device identifier.
        /// </summary>
        /// <param name="builder">The <see cref="DeviceIdBuilder"/> to add the component to.</param>
        /// <returns>The <see cref="DeviceIdBuilder"/> instance.</returns>
        public static DeviceIdBuilder AddMacAddress(this DeviceIdBuilder builder)
        {
#if NETFRAMEWORK
            return builder.AddComponent(new WmiDeviceIdComponent("MACAddress", "Win32_NetworkAdapterConfiguration", "MACAddress"));
#else
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return builder.AddComponent(new WmiDeviceIdComponent("MACAddress", "Win32_NetworkAdapterConfiguration", "MACAddress"));

            return builder.AddComponent(new NetworkAdapterDeviceIdComponent(true, true));
#endif
        }

        /// <summary>
        /// Adds the MAC address to the device identifier, optionally excluding non-physical adapters and/or wireless adapters.
        /// </summary>
        /// <param name="builder">The <see cref="DeviceIdBuilder"/> to add the component to.</param>
        /// <param name="excludeNonPhysical">A value indicating whether non-physical adapters should be excluded.</param>
        /// <param name="excludeWireless">A value indicating whether wireless adapters should be excluded.</param>
        /// <returns>The <see cref="DeviceIdBuilder"/> instance.</returns>
        public static DeviceIdBuilder AddMacAddress(this DeviceIdBuilder builder, bool excludeNonPhysical = false, bool excludeWireless = false)
        {
            return builder.AddComponent(new NetworkAdapterDeviceIdComponent(excludeNonPhysical, excludeWireless));
        }

        /// <summary>
        /// Adds the processor ID to the device identifier.
        /// </summary>
        /// <param name="builder">The <see cref="DeviceIdBuilder"/> to add the component to.</param>
        /// <returns>The <see cref="DeviceIdBuilder"/> instance.</returns>
        public static DeviceIdBuilder AddProcessorId(this DeviceIdBuilder builder)
        {
#if NETFRAMEWORK
            return builder.AddComponent(new WmiDeviceIdComponent("ProcessorId", "Win32_Processor", "ProcessorId"));
#else
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return builder.AddComponent(new WmiDeviceIdComponent("ProcessorId", "Win32_Processor", "ProcessorId"));
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return builder.AddComponent(new FileDeviceIdComponent("ProcessorId", "/proc/cpuinfo", true));

            return builder.AddComponent(new UnsupportedDeviceIdComponent("ProcessorId"));
#endif
        }

        /// <summary>
        /// Adds the motherboard serial number to the device identifier. On Linux, this requires root privilege.
        /// </summary>
        /// <param name="builder">The <see cref="DeviceIdBuilder"/> to add the component to.</param>
        /// <returns>The <see cref="DeviceIdBuilder"/> instance.</returns>
        public static DeviceIdBuilder AddMotherboardSerialNumber(this DeviceIdBuilder builder)
        {
#if NETFRAMEWORK
            return builder.AddComponent(new WmiDeviceIdComponent("MotherboardSerialNumber", "Win32_BaseBoard", "SerialNumber"));
#else
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return builder.AddComponent(new WmiDeviceIdComponent("MotherboardSerialNumber", "Win32_BaseBoard", "SerialNumber"));
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return builder.AddComponent(new FileDeviceIdComponent("MotherboardSerialNumber", "/sys/class/dmi/id/board_serial"));           

            return builder.AddComponent(new UnsupportedDeviceIdComponent("MotherboardSerialNumber"));
#endif
        }

        /// <summary>
        /// Adds the system UUID to the device identifier. On Linux, this requires root privilege.
        /// </summary>
        /// <param name="builder">The <see cref="DeviceIdBuilder"/> to add the component to.</param>
        /// <returns>The <see cref="DeviceIdBuilder"/> instance.</returns>
        public static DeviceIdBuilder AddSystemUUID(this DeviceIdBuilder builder)
        {
#if NETFRAMEWORK
            return builder.AddComponent(new WmiDeviceIdComponent("SystemUUID", "Win32_ComputerSystemProduct", "UUID"));
#else
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return builder.AddComponent(new WmiDeviceIdComponent("SystemUUID", "Win32_ComputerSystemProduct", "UUID"));
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return builder.AddComponent(new FileDeviceIdComponent("SystemUUID", "/sys/class/dmi/id/product_uuid"));        

            return builder.AddComponent(new UnsupportedDeviceIdComponent("SystemUUID"));
#endif
        }

        /// <summary>
        /// Adds the system drive's serial number to the device identifier.
        /// </summary>
        /// <param name="builder">The <see cref="DeviceIdBuilder"/> to add the component to.</param>
        /// <returns>The <see cref="DeviceIdBuilder"/> instance.</returns>
        public static DeviceIdBuilder AddSystemDriveSerialNumber(this DeviceIdBuilder builder)
        {
#if NETFRAMEWORK
            return builder.AddComponent(new SystemDriveSerialNumberDeviceIdComponent());
#else
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return builder.AddComponent(new SystemDriveSerialNumberDeviceIdComponent());
            
            return builder.AddComponent(new UnsupportedDeviceIdComponent("SystemDriveSerialNumber"));
#endif
        }

        /// <summary>
        /// Adds the an identifier tied to the installation of the OS
        /// </summary>
        /// <param name="builder">The <see cref="DeviceIdBuilder"/> to add the component to.</param>
        /// <returns>The <see cref="DeviceIdBuilder"/> instance.</returns>
        public static DeviceIdBuilder AddOSInstallationID(this DeviceIdBuilder builder)
        {
#if NETFRAMEWORK
            return builder.AddComponent(new RegistryValueDeviceIdComponent("OSInstallationID", @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Cryptography", "MachineGuid"));
#else
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return builder.AddComponent(new RegistryValueDeviceIdComponent("OSInstallationID", @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Cryptography", "MachineGuid"));
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return builder.AddComponent(new FileDeviceIdComponent("OSInstallationID", new string[] {"/var/lib/dbus/machine-id", "/etc/machine-id"}));

            return builder.AddComponent(new UnsupportedDeviceIdComponent("OSInstallationID"));
#endif
       }

        /// <summary>
        /// Adds a file-based token to the device identifier.
        /// </summary>
        /// <param name="builder">The <see cref="DeviceIdBuilder"/> to add the component to.</param>
        /// <param name="path">The path of the token.</param>
        /// <returns>The <see cref="DeviceIdBuilder"/> instance.</returns>
        public static DeviceIdBuilder AddFileToken(this DeviceIdBuilder builder, string path)
        {
            var name = string.Concat("FileToken", path.GetHashCode());
            return builder.AddComponent(new FileTokenDeviceIdComponent(name, path));
        }
    }
}
