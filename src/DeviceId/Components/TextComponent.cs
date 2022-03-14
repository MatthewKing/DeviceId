namespace DeviceId.Components;

/// <summary>
/// An implementation of <see cref="IDeviceIdComponent"/> that executes a command.
/// </summary>
public class TextComponent : IDeviceIdComponent
{
    private readonly string _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="TextComponent"/> class.
    /// </summary>
    /// <param name="value">The value of this component.</param>
    internal TextComponent(string value)
    {
        _value = value;
    }

    /// <summary>
    /// Gets the component value.
    /// </summary>
    /// <returns>The component value.</returns>
    public string GetValue()
    {
        return _value;
    }
}
