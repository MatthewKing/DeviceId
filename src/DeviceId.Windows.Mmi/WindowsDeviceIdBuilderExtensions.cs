﻿using DeviceId.Windows.Mmi.Components;

namespace DeviceId.Windows.Mmi
{
    /// <summary>
    /// Extension methods for <see cref="WindowsDeviceIdBuilder"/>.
    /// </summary>
    public static class WindowsDeviceIdBuilderExtensions
    {
        /// <summary>
        /// Adds the MAC address to the device identifier, optionally excluding wireless adapters and/or non-physical adapters.
        /// </summary>
        /// <param name="builder">The <see cref="WindowsDeviceIdBuilder"/> to add the component to.</param>
        /// <param name="excludeWireless">A value indicating whether wireless adapters should be excluded.</param>
        /// <param name="excludeNonPhysical">A value indicating whether non-physical adapters should be excluded.</param>
        /// <returns>The <see cref="WindowsDeviceIdBuilder"/> instance.</returns>
        public static WindowsDeviceIdBuilder AddMacAddressFromMmi(this WindowsDeviceIdBuilder builder, bool excludeWireless, bool excludeNonPhysical)
        {
            return builder.AddComponent("MACAddress", new MmiMacAddressDeviceIdComponent(excludeWireless, excludeNonPhysical));
        }

        /// <summary>
        /// Adds the processor ID to the device identifier.
        /// </summary>
        /// <param name="builder">The <see cref="WindowsDeviceIdBuilder"/> to add the component to.</param>
        /// <returns>The <see cref="WindowsDeviceIdBuilder"/> instance.</returns>
        public static WindowsDeviceIdBuilder AddProcessorId(this WindowsDeviceIdBuilder builder)
        {
            return builder.AddComponent("ProcessorId", new MmiWqlDeviceIdComponent("Win32_Processor", "ProcessorId"));
        }

        /// <summary>
        /// Adds the motherboard serial number to the device identifier.
        /// </summary>
        /// <param name="builder">The <see cref="WindowsDeviceIdBuilder"/> to add the component to.</param>
        /// <returns>The <see cref="WindowsDeviceIdBuilder"/> instance.</returns>
        public static WindowsDeviceIdBuilder AddMotherboardSerialNumber(this WindowsDeviceIdBuilder builder)
        {
            return builder.AddComponent("MotherboardSerialNumber", new MmiWqlDeviceIdComponent("Win32_BaseBoard", "SerialNumber"));
        }

        /// <summary>
        /// Adds the system UUID to the device identifier.
        /// </summary>
        /// <param name="builder">The <see cref="WindowsDeviceIdBuilder"/> to add the component to.</param>
        /// <returns>The <see cref="WindowsDeviceIdBuilder"/> instance.</returns>
        public static WindowsDeviceIdBuilder AddSystemUuid(this WindowsDeviceIdBuilder builder)
        {
            return builder.AddComponent("SystemUUID", new MmiWqlDeviceIdComponent("Win32_ComputerSystemProduct", "UUID"));
        }

        /// <summary>
        /// Adds the system serial drive number to the device identifier.
        /// </summary>
        /// <param name="builder">The <see cref="WindowsDeviceIdBuilder"/> to add the component to.</param>
        /// <returns>The <see cref="WindowsDeviceIdBuilder"/> instance.</returns>
        public static WindowsDeviceIdBuilder AddSystemSerialDriveNumber(this WindowsDeviceIdBuilder builder)
        {
            return builder.AddComponent("SystemDriveSerialNumber", new MmiSystemDriveSerialNumberDeviceIdComponent());
        }
    }
}