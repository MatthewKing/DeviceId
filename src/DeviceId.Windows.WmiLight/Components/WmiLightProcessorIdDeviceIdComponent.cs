using System.Collections.Generic;
using WmiLight;

namespace DeviceId.Windows.WmiLight.Components;

/// <summary>
/// An implementation of <see cref="IDeviceIdComponent"/> that retrieves the processor ID using WmiLight.
/// On ARM64 systems where ProcessorId is not available, it falls back to a combination of
/// Manufacturer, Name, and NumberOfCores.
/// </summary>
public class WmiLightProcessorIdDeviceIdComponent : IDeviceIdComponent
{
    /// <summary>
    /// Gets the component value.
    /// </summary>
    /// <returns>The component value.</returns>
    public string GetValue()
    {
        var values = new List<string>();

        try
        {
            using var wmiConnection = new WmiConnection();
            foreach (var wmiObject in wmiConnection.CreateQuery("SELECT * FROM Win32_Processor"))
            {
                try
                {
                    // First try to get ProcessorId (available on x86/x64)
                    if (wmiObject["ProcessorId"] is string processorId && !string.IsNullOrEmpty(processorId))
                    {
                        values.Add(processorId);
                    }
                    else
                    {
                        // Fallback for ARM64: combine Manufacturer, Name, and NumberOfCores
                        var manufacturer = wmiObject["Manufacturer"]?.ToString();
                        var name = wmiObject["Name"]?.ToString();
                        var numberOfCores = wmiObject["NumberOfCores"]?.ToString();

                        var fallbackParts = new List<string>();
                        if (!string.IsNullOrEmpty(manufacturer))
                        {
                            fallbackParts.Add(manufacturer);
                        }
                        if (!string.IsNullOrEmpty(name))
                        {
                            fallbackParts.Add(name);
                        }
                        if (!string.IsNullOrEmpty(numberOfCores))
                        {
                            fallbackParts.Add(numberOfCores);
                        }

                        if (fallbackParts.Count > 0)
                        {
                            values.Add(string.Join("|", fallbackParts));
                        }
                    }
                }
                finally
                {
                    wmiObject.Dispose();
                }
            }
        }
        catch
        {
            // Ignore exceptions
        }

        values.Sort();

        return values.Count > 0
            ? string.Join(",", values.ToArray())
            : null;
    }
}
