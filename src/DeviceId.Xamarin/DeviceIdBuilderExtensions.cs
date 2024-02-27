using System;

namespace DeviceId;

/// <summary>
/// Extension methods for <see cref="DeviceIdBuilder"/>.
/// </summary>
public static class DeviceIdBuilderExtensions
{
    /// <summary>
    /// Adds Xamarin-specific components to the device ID.
    /// </summary>
    /// <param name="builder">The device ID builder to add the components to.</param>
    /// <param name="xamarinBuilderConfiguration">An action that adds the Xamarin-specific components.</param>
    /// <returns>The device ID builder.</returns>
    public static DeviceIdBuilder OnLinux(this DeviceIdBuilder builder, Action<XamarinDeviceIdBuilder> xamarinBuilderConfiguration)
    {
        if (xamarinBuilderConfiguration is not null)
        {
            var linuxBuilder = new XamarinDeviceIdBuilder(builder);
            xamarinBuilderConfiguration.Invoke(linuxBuilder);
        }

        return builder;
    }
}
