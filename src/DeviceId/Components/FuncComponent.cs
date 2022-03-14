using System;

namespace DeviceId.Components;

/// <summary>
/// An implementation of <see cref="IDeviceIdComponent"/> that executes a command.
/// </summary>
public class FuncComponent : IDeviceIdComponent
{
    private readonly Func<string> _func;

    /// <summary>
    /// Initializes a new instance of the <see cref="FuncComponent"/> class.
    /// </summary>
    /// <param name="func">The value-function of this component.</param>
    internal FuncComponent(Func<string> func)
    {
        _func = func;
    }

    /// <summary>
    /// Gets the component value.
    /// </summary>
    /// <returns>The component value.</returns>
    public string GetValue()
    {
        return _func.Invoke();
    }
}
