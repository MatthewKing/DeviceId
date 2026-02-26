using System.Collections.Generic;
using System.Management;

namespace DeviceId.Windows.Wmi.Components;

/// <summary>
/// An implementation of <see cref="IDeviceIdComponent"/> that retrieves the processor ID.
/// On ARM64 systems where ProcessorId is not available, it falls back to a combination of
/// Manufacturer, Name, and NumberOfCores.
/// </summary>
public class WmiProcessorIdDeviceIdComponent : IDeviceIdComponent
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
            using var managementObjectSearcher = new ManagementObjectSearcher("SELECT ProcessorId, Manufacturer, Name, NumberOfCores FROM Win32_Processor");
            using var managementObjectCollection = managementObjectSearcher.Get();
            foreach (var managementObject in managementObjectCollection)
            {
                try
                {
                    // First try to get ProcessorId (available on x86/x64)
                    if (managementObject["ProcessorId"] is string processorId && !string.IsNullOrEmpty(processorId))
                    {
                        values.Add(processorId);
                    }
                    else
                    {
                        // Fallback for ARM64: combine Manufacturer, Name, and NumberOfCores
                        var manufacturer = managementObject["Manufacturer"]?.ToString();
                        var name = managementObject["Name"]?.ToString();
                        var numberOfCores = managementObject["NumberOfCores"]?.ToString();

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
                    managementObject.Dispose();
                }
            }
        }
        catch
        {
        }

        values.Sort();

        return values.Count > 0
            ? string.Join(",", values.ToArray())
            : null;
    }
}
