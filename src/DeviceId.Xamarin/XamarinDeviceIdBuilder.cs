using System;
using System.Runtime.InteropServices;
using DeviceId.Internal;
using Xamarin.Essentials;

namespace DeviceId;

/// <summary>
/// Provides a fluent interface for adding Xamarin-specific components to a device identifier.
/// </summary>
public class XamarinDeviceIdBuilder
{
    /// <summary>
    /// The base device identifier builder.
    /// </summary>
    private readonly DeviceIdBuilder _baseBuilder;

    /// <summary>
    /// Initializes a new instance of the <see cref="XamarinDeviceIdBuilder"/> class.
    /// </summary>
    /// <param name="baseBuilder">The base device identifier builder.</param>
    public XamarinDeviceIdBuilder(DeviceIdBuilder baseBuilder)
    {
        _baseBuilder = baseBuilder ?? throw new ArgumentNullException(nameof(baseBuilder));
    }

    /// <summary>
    /// Adds a component to the device identifier.
    /// If a component with the specified name already exists, it will be replaced with this newly added component.
    /// </summary>
    /// <param name="name">The component name.</param>
    /// <param name="component">The component to add.</param>
    /// <returns>The builder instance.</returns>
    public XamarinDeviceIdBuilder AddComponent(string name, IDeviceIdComponent component)
    {
        _baseBuilder.AddComponent(name, component);

        return this;
    }
}
