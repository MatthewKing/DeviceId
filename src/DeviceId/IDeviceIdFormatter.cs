using System.Collections.Generic;

namespace DeviceId
{
    /// <summary>
    /// Provides a method to combine a number of <see cref="IDeviceIdComponent"/> instances
    /// into a single device identifier string.
    /// </summary>
    public interface IDeviceIdFormatter
    {
        /// <summary>
        /// Returns the device identifier string created by combining the specified <see cref="IDeviceIdComponent"/> instances.
        /// </summary>
        /// <param name="components">A sequence containing the <see cref="IDeviceIdComponent"/> instances to combine into the device identifier string.</param>
        /// <returns>The device identifier string.</returns>
        string GetDeviceId(IEnumerable<IDeviceIdComponent> components);
    }
}
