using System;
using DeviceId.Components;
using DeviceId.Internal;

namespace DeviceId;

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
    /// Adds the current user name to the device identifier.
    /// </summary>
    /// <param name="builder">The <see cref="DeviceIdBuilder"/> to add the component to.</param>
    /// <returns>The <see cref="DeviceIdBuilder"/> instance.</returns>
    public static DeviceIdBuilder AddUserName(this DeviceIdBuilder builder)
    {
        return builder.AddComponent("UserName", new DeviceIdComponent(Environment.UserName));
    }

    /// <summary>
    /// Adds the machine name to the device identifier.
    /// </summary>
    /// <param name="builder">The <see cref="DeviceIdBuilder"/> to add the component to.</param>
    /// <returns>The <see cref="DeviceIdBuilder"/> instance.</returns>
    public static DeviceIdBuilder AddMachineName(this DeviceIdBuilder builder)
    {
        return builder.AddComponent("MachineName", new DeviceIdComponent(Environment.MachineName));
    }

    /// <summary>
    /// Adds the operating system version to the device identifier.
    /// </summary>
    /// <param name="builder">The <see cref="DeviceIdBuilder"/> to add the component to.</param>
    /// <returns>The <see cref="DeviceIdBuilder"/> instance.</returns>
    public static DeviceIdBuilder AddOsVersion(this DeviceIdBuilder builder)
    {
        return builder.AddComponent("OSVersion", new DeviceIdComponent(OS.Version));
    }

    /// <summary>
    /// Adds the MAC address to the device identifier, optionally excluding wireless adapters.
    /// </summary>
    /// <param name="builder">The <see cref="DeviceIdBuilder"/> to add the component to.</param>
    /// <param name="excludeWireless">A value indicating whether wireless adapters should be excluded.</param>
    /// <returns>The <see cref="DeviceIdBuilder"/> instance.</returns>
    public static DeviceIdBuilder AddMacAddress(this DeviceIdBuilder builder, bool excludeWireless = false)
    {
        return builder.AddComponent("MACAddress", new MacAddressDeviceIdComponent(excludeWireless));
    }

    /// <summary>
    /// Adds the Domain Name to the device identifier.
    /// </summary>
    /// <param name="builder">The <see cref="DeviceIdBuilder"/> to add the component to.</param>
    /// <returns>The <see cref="DeviceIdBuilder"/> instance.</returns>
    public static DeviceIdBuilder AddDomainName(this DeviceIdBuilder builder)
    {
        return builder.AddComponent("DomainName", new DomainNameDeviceIdComponent());
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
        return builder.AddComponent(name, new FileTokenDeviceIdComponent(path));
    }
}
