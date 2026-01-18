using System.Collections.Generic;
using Microsoft.Management.Infrastructure;

namespace DeviceId.Windows.Mmi.Components;

/// <summary>
/// An implementation of <see cref="IDeviceIdComponent"/> that retrieves the processor ID using MMI.
/// On ARM64 systems where ProcessorId is not available, it falls back to a combination of
/// Manufacturer, Name, and NumberOfCores.
/// </summary>
public class MmiProcessorIdDeviceIdComponent : IDeviceIdComponent
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
            using var session = CimSession.Create(null);

            var instances = session.QueryInstances(@"root\cimv2", "WQL", "SELECT ProcessorId, Manufacturer, Name, NumberOfCores FROM Win32_Processor");
            foreach (var instance in instances)
            {
                try
                {
                    // First try to get ProcessorId (available on x86/x64)
                    var processorIdProperty = instance.CimInstanceProperties["ProcessorId"];
                    if (processorIdProperty?.Value is string processorId && !string.IsNullOrEmpty(processorId))
                    {
                        values.Add(processorId);
                    }
                    else
                    {
                        // Fallback for ARM64: combine Manufacturer, Name, and NumberOfCores
                        var manufacturer = instance.CimInstanceProperties["Manufacturer"]?.Value?.ToString();
                        var name = instance.CimInstanceProperties["Name"]?.Value?.ToString();
                        var numberOfCores = instance.CimInstanceProperties["NumberOfCores"]?.Value?.ToString();

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
                    instance.Dispose();
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
