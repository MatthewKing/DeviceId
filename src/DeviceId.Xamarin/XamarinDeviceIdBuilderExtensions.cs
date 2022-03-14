using System;
using DeviceId.Components;
using Xamarin.Essentials;

namespace DeviceId;

/// <summary>
/// Extension methods for <see cref="XamarinDeviceIdBuilder"/>.
/// </summary>
public static class XamarinDeviceIdBuilderExtensions
{
    /// <summary>
    /// Adds the device manufacturer to the device identifier.
    /// </summary>
    /// <param name="builder">The <see cref="XamarinDeviceIdBuilder"/> to add the component to.</param>
    /// <returns>The <see cref="XamarinDeviceIdBuilder"/> instance.</returns>
    public static XamarinDeviceIdBuilder AddManufacturer(this XamarinDeviceIdBuilder builder)
    {
        return builder.AddComponent("Manufacturer", new TextComponent(DeviceInfo.Manufacturer));
    }

    /// <summary>
    /// Adds the OS-version to the device identifier.
    /// </summary>
    /// <param name="builder">The <see cref="XamarinDeviceIdBuilder"/> to add the component to.</param>
    /// <returns>The <see cref="XamarinDeviceIdBuilder"/> instance.</returns>
    public static XamarinDeviceIdBuilder AddOsVersion(this XamarinDeviceIdBuilder builder)
    {
        return builder.AddComponent("OSVersion", new TextComponent(DeviceInfo.VersionString));
    }

    /// <summary>
    /// Adds the device model to the device identifier.
    /// </summary>
    /// <param name="builder">The <see cref="XamarinDeviceIdBuilder"/> to add the component to.</param>
    /// <returns>The <see cref="XamarinDeviceIdBuilder"/> instance.</returns>
    public static XamarinDeviceIdBuilder AddModel(this XamarinDeviceIdBuilder builder)
    {
        return builder.AddComponent("Model", new TextComponent(DeviceInfo.Model));
    }

    /// <summary>
    /// Adds the device-platform to the device identifier.
    /// </summary>
    /// <param name="builder">The <see cref="XamarinDeviceIdBuilder"/> to add the component to.</param>
    /// <returns>The <see cref="XamarinDeviceIdBuilder"/> instance.</returns>
    public static XamarinDeviceIdBuilder AddPlatform(this XamarinDeviceIdBuilder builder)
    {
        return builder.AddComponent("Platform", new TextComponent(DeviceInfo.Platform.ToString()));
    }

    /// <summary>
    /// Adds the device type (physical/virtual) to the device identifier.
    /// </summary>
    /// <param name="builder">The <see cref="XamarinDeviceIdBuilder"/> to add the component to.</param>
    /// <returns>The <see cref="XamarinDeviceIdBuilder"/> instance.</returns>
    public static XamarinDeviceIdBuilder AddDeviceType(this XamarinDeviceIdBuilder builder)
    {
        return builder.AddComponent("DeviceType", new TextComponent(DeviceInfo.DeviceType.ToString()));
    }

    /// <summary>
    /// Adds the device idiom to the device identifier (phone, tablet, watch , ...).
    /// </summary>
    /// <param name="builder">The <see cref="XamarinDeviceIdBuilder"/> to add the component to.</param>
    /// <returns>The <see cref="XamarinDeviceIdBuilder"/> instance.</returns>
    public static XamarinDeviceIdBuilder AddIdiom(this XamarinDeviceIdBuilder builder)
    {
        return builder.AddComponent("Idiom", new TextComponent(DeviceInfo.Idiom.ToString()));
    }

    /// <summary>
    /// Adds the device name to the device identifier.
    /// </summary>
    /// <param name="builder">The <see cref="XamarinDeviceIdBuilder"/> to add the component to.</param>
    /// <returns>The <see cref="XamarinDeviceIdBuilder"/> instance.</returns>
    public static XamarinDeviceIdBuilder AddDeviceName(this XamarinDeviceIdBuilder builder)
    {
        return builder.AddComponent("DeviceName", new TextComponent(DeviceInfo.Name));
    }

    /// <summary>
    /// Adds a generated guid to the device identifier. The generated value is persistantly stored.
    /// </summary>
    /// <param name="builder">The <see cref="XamarinDeviceIdBuilder"/> to add the component to.</param>
    /// <returns>The <see cref="XamarinDeviceIdBuilder"/> instance.</returns>
    public static XamarinDeviceIdBuilder AddGeneratedGuid(this XamarinDeviceIdBuilder builder)
    {
        const string key = "XamarinDeviceIdBuilder.GeneratedId";
        Func<string> func = () =>
        {
            string guid = Preferences.Get(key, "");
            if (string.IsNullOrEmpty(guid))
            {
                guid = Guid.NewGuid().ToString();
                Preferences.Set(key, guid);
            }
            return guid;
        };
        return builder.AddComponent("GeneratedGuid", new FuncComponent(func));
    }

}
