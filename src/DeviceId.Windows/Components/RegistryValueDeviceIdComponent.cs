using Microsoft.Win32;

namespace DeviceId.Windows.Components;

/// <summary>
/// An implementation of <see cref="IDeviceIdComponent"/> that retrieves its value from the Windows registry.
/// </summary>
public class RegistryValueDeviceIdComponent : IDeviceIdComponent
{
#if !NET35
    /// <summary>
    /// The registry hive.
    /// </summary>
    private readonly RegistryHive _registryHive;

    /// <summary>
    /// The registry views.
    /// </summary>
    private readonly RegistryView _registryView;
#endif

    /// <summary>
    /// The name of the registry key.
    /// </summary>
    private readonly string _keyName;

    /// <summary>
    /// The name of the registry value.
    /// </summary>
    private readonly string _valueName;

#if NET35
    /// <summary>
    /// Initializes a new instance of the <see cref="RegistryValueDeviceIdComponent"/> class.
    /// </summary>
    /// <param name="keyName">The name of the registry key.</param>
    /// <param name="valueName">The name of the registry value.</param>
    public RegistryValueDeviceIdComponent(string keyName, string valueName)
    {
        _keyName = keyName;
        _valueName = valueName;
    }
#else
    /// <summary>
    /// Initializes a new instance of the <see cref="RegistryValueDeviceIdComponent"/> class.
    /// </summary>
    /// <param name="registryView">The registry view.</param>
    /// <param name="registryHive">The registry hive.</param>
    /// <param name="keyName">The name of the registry key.</param>
    /// <param name="valueName">The name of the registry value.</param>
    public RegistryValueDeviceIdComponent(RegistryView registryView, RegistryHive registryHive, string keyName, string valueName)
    {
        _registryHive = registryHive;
        _registryView = registryView;
        _keyName = keyName;
        _valueName = valueName;
    }
#endif

    /// <summary>
    /// Gets the component value.
    /// </summary>
    /// <returns>The component value.</returns>
    public string GetValue()
    {
#if NET35
        try
        {
            return Registry.GetValue(_keyName, _valueName, null)?.ToString();
        }
        catch { }
#else
        try
        {
            using var registry = RegistryKey.OpenBaseKey(_registryHive, _registryView);
            using var subKey = registry.OpenSubKey(_keyName);
            if (subKey != null)
            {
                var value = subKey.GetValue(_valueName);
                return value?.ToString();
            }
        }
        catch { }
#endif

        return null;
    }
}
